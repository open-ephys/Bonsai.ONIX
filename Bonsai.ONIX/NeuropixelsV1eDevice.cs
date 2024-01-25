using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.DS90UB9X)]
    [Description("Acquire data from a single probe using Neuropixels 1.0e Headstage.")]
    [DefaultProperty("Configuration")]
    public class NeuropixelsV1eDevice : ONIFrameReader<NeuropixelsV1eDataFrame, ushort>
    {
        private const int NeuropixelsAddress = 0x70;
        private const int EEPROMAddress = 0x51;

        // All GPIO set to locally (I2C) controlled outputs
        private const byte DefaultGPO10Config = 0b0001_0001; // GPIO0 Low, NPs in MUX reset
        private const byte DefaultGPO32Config = 0b1001_0001; // LED off, GPIO1 Low

        private int gpo10Config = DefaultGPO10Config;
        private int gpo32Config = DefaultGPO32Config;

        protected override IObservable<NeuropixelsV1eDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {

            // TODO: Hack
            // Constructor cannot take parameter. Making generic type or interface is too much work
            Configuration.HeadstageType = NeuropixelsV1Configuration.Headstage.ONIXE;

            // Enable NPs (must be done before I2C communication with probes)
            using (var i2cSer = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
            {
                // Hit probe with mux reset to restart PSB stream and frame counter
                gpo10Config &= ~(1 << 3);
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                gpo10Config |= 1 << 3;
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);
            }

            // Configure the probe itself
            using (var probe = new NeuropixelsV1Probe(DeviceAddress))
            {
                if (RequireSNMatch && Configuration.ConfigProbeSN != Configuration.FlexProbeSN)
                {
                    throw new WorkflowRuntimeException("Probe and configuration serial numbers do not match.");
                }

                if (Configuration.RefreshNeeded)
                {
                    probe.FullReset();
                    probe.WriteConfiguration(Configuration);
                }
                else
                {
                    probe.DigitalReset();
                }

                probe.Start();
#if DEBUG
                Console.WriteLine($"Probe {Configuration.FlexProbeSN} started");
#endif
            }

            // Lock in BlockSize so that it cannot be changed during acquisition
            var bufferSize = BlockSize * 12; // 12 super-frames in a ultra-frame

            // Get calibration parameters for current seetings
            // NB: Only using channel 0 since every row is identical
            var apGain = (ushort)(Configuration.Channels[0].APGainCorrection * (1 << 14)); 
            var lfpGain = (ushort)(Configuration.Channels[0].LFPGainCorrection * (1 << 14)); 
            var threshold = Configuration.ADCs.Select(x => (ushort)x.Threshold).ToArray();
            var offset = Configuration.ADCs.Select(x => (ushort)x.Offset).ToArray();

            using (var i2cSer = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
            {
                // Toggle LED
                gpo32Config &= 0b0111_1111;
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)gpo32Config);
            }

            return source
                .Buffer(bufferSize)
                .Select(b => { return new NeuropixelsV1eDataFrame(b, frameOffset, apGain, lfpGain, threshold, offset); })
                .Finally(() =>
                {
                    using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
                    {
                        i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, DefaultGPO10Config);
                        i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, DefaultGPO32Config);
                    }
                });
        }

        private ONIDeviceAddress deviceAddress = new ONIDeviceAddress();
        public override ONIDeviceAddress DeviceAddress
        {
            get { return deviceAddress; }
            set
            {
                deviceAddress = value;

                // Deserializer triggering mode
                WriteRegister((uint)DS90UB9xConfiguration.Register.TriggerOffset, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.Trigger, (uint)DS90UB9xConfiguration.TriggerMode.Continuous);
                WriteRegister((uint)DS90UB9xConfiguration.Register.IncludeSyncBits, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.PixelGate, 0b0000_0001_0001_0011_0000_0000_0000_0001);
                WriteRegister((uint)DS90UB9xConfiguration.Register.MarkMode, (uint)DS90UB9xConfiguration.MarkMode.Disabled);

                // Enable 7-bit magic word-triggered stream
                WriteRegister((uint)DS90UB9xConfiguration.Register.ReadSize, 851973); // 16 frames/superframe, 7x 14-bit words on each serial line per frame
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicMask, 0b11000000000000000000001111111111); // Enable inverse, wait for non-inverse, 14-bit magic word
                WriteRegister((uint)DS90UB9xConfiguration.Register.Magic, 816); // Super-frame sync word
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicWait, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataMode, 913);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines0, 0x3245106B);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines1, 0xFFFFFFFF);

                // Configuration I2C aliases
                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    uint val = 0x4 + (uint)DS90UB9xConfiguration.Mode.Raw12BitHighFrequency; // 0x4 maintains coax mode
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.PortMode, val);

                    val = NeuropixelsAddress << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveID1, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveAlias1, val);

                    val = 0x50 << 1; // Flex EEPROM
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveID2, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveAlias2, val);

                    val = EEPROMAddress << 1; // Headstage EEPROM
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveID3, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveAlias3, val);
                }

                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
                {
                    // Change all the GPIOs to locally-controlled outputs; output state set to default
                    i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)gpo32Config);
                }

                // Read the probe metadata
                Configuration.DeviceAddress = deviceAddress;

                // TODO: Headstage EEPROM
                //using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, EEPROMAddress))
                //{
                //    // TODO: i2c.WriteByte(0, 42, true);
                //    // TODO: i2c.ReadByte(i, true)
                //}

                // Make sure that we have lock after all that config
                if (ReadRegister((uint)DS90UB9xConfiguration.Register.LinkStatus) == 0)
                {
                    throw new WorkflowBuildException("Unable to obtain data link with Neuropixels headstage.");
                }
            }
        }

        [Category("Configuration")]
        [Range(1, 10000)]
        [Description("The number of \"Ultra Frames\" to collect per probe before data is propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 30; // 1 millisecond

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.Enable) > 0;
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.Enable, value ? (uint)1 : 0);
            }
        }

        [Category("Configuration")]
        [Description("Neuropixels probe hardware configuration.")]
        [Editor("Bonsai.ONIX.Design.NeuropixelsV1Editor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Externalizable(false)]
        public NeuropixelsV1Configuration Configuration { get; set; } = new NeuropixelsV1Configuration() { HeadstageType = NeuropixelsV1Configuration.Headstage.ONIXE };

        [Category("Configuration")]
        [Description("Require configuration and probe serial numbers to match to start acqusition.")]
        public bool RequireSNMatch { get; set; } = true;

        [Category("Configuration")]
        [Description("The probe serial number.")]
        [XmlIgnore]
        public ulong? ProbeSN => Configuration.FlexProbeSN;

        [Category("Configuration")]
        [Description("The probe part number.")]
        [XmlIgnore]
        public string ProbePartNo => Configuration.ProbePartNo;

        [Category("Configuration")]
        [Description("The flex cable version.")]
        [XmlIgnore]
        public string FlexVersion => Configuration.FlexVersion;

        [Category("Configuration")]
        [Description("The probe part number.")]
        [XmlIgnore]
        public string FlexPartNo => Configuration.FlexPartNo;

    }
}
