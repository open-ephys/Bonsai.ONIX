using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.DS90UB9X)]
    [Description("Acquire data from two probes using Neuropixels 2.0e Headstage.")]
    public class NeuropixelsV2eDevice : ONIFrameReader<NeuropixelsV2DataFrame, ushort>
    {
        private const int NeuropixelsAddress = 0x10;

        // All GPIO set to locally (I2C) controlled outputs
        private const byte DefaultGPO10Config = 0b0001_0001; // NPs in reset, VDDA not enabled
        private const byte NoProbeSelected = 0b0001_0001; // No probes selected
        private const byte ProbeASelected = 0b0001_1001; // TODO: Changes in Rev. B of headstage
        private const byte ProbeBSelected = 0b1001_1001;

        private int gpo10Config = DefaultGPO10Config;

        private void ConfigureProbeStreaming(I2CRegisterConfiguration i2cNP)
        {
            // Write super sync bits into ASIC
            i2cNP.WriteByte(0x15, 0b00011000);
            i2cNP.WriteByte(0x14, 0b01100001);
            i2cNP.WriteByte(0x13, 0b10000110);
            i2cNP.WriteByte(0x12, 0b00011000);
            i2cNP.WriteByte(0x11, 0b01100001);
            i2cNP.WriteByte(0x10, 0b10000110);
            i2cNP.WriteByte(0x0F, 0b00011000);
            i2cNP.WriteByte(0x0E, 0b01100001);
            i2cNP.WriteByte(0x0D, 0b10000110);
            i2cNP.WriteByte(0x0C, 0b00011000);
            i2cNP.WriteByte(0x0B, 0b01100001);
            i2cNP.WriteByte(0x0A, 0b10111001);


            // Activate recording mode on NP
            i2cNP.WriteByte(0, 0b0100_0000);
        }

        protected override IObservable<NeuropixelsV2DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            // Toggle LED and enable NPs (must be done before I2C communication with probes)
            using (I2CRegisterConfiguration
                   i2cSer = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress),
                   i2cNP = new I2CRegisterConfiguration(DeviceAddress, ID, NeuropixelsAddress))
            {
                // Turn on analog supply
                gpo10Config |= 1 << 3;
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                // Wait 20 milliseconds
                System.Threading.Thread.Sleep(20);

                // Issue full reset to both probes
                gpo10Config &= ~(1 << 7);
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                gpo10Config |= 1 << 7;
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                // TODO: Handle case where one or no probes are attached.

                if (ProbeAMetadata.ProbeSN == null && ProbeBMetadata.ProbeSN == null)
                {
                    throw new WorkflowRuntimeException("No neuropixel probes available.");
                }

                if (ProbeAMetadata.ProbeSN != null)
                {
                    // Configure Probe A
                    SelectProbeA(i2cSer);
                    ConfigureProbeStreaming(i2cNP);
                }

                if (ProbeBMetadata.ProbeSN != null)
                {
                    // Configure Probe B
                    SelectProbeB(i2cSer);
                    ConfigureProbeStreaming(i2cNP);
                }

                // TODO: Remove
                //for (uint i = 0; i <= 0x1f; i++)
                //    Console.WriteLine("{0}: {1}", i, i2cNP.ReadByte(i));

                //SelectProbeB(i2cSer);
                // TODO: Apply configuration to NP B
                //i2cNP.WriteByte(0, 0b0100_0000); // Activate recording mode on NP B

            }

            // Lock in BlockSize so that it cannot be changed during acquisition
            var bufferSize = BlockSize;

            return source
                .GroupBy(f => f.Sample[4]) // Group by NP index
                .SelectMany(group => group.Buffer(bufferSize))
                .Select(b => { return new NeuropixelsV2DataFrame(b, frameOffset); })
                .Finally(() =>
                {
                    using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
                    {
                        i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, DefaultGPO10Config);
                        DeselectProbes(i2c);
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
                WriteRegister((uint)DS90UB9xConfiguration.Register.PixelGate, (uint)DS90UB9xConfiguration.PixelGate.Disabled);
                WriteRegister((uint)DS90UB9xConfiguration.Register.MarkMode, (uint)DS90UB9xConfiguration.MarkMode.Disabled);

                // Enable two 4-bit magic word-triggered streams, one for each probe
                WriteRegister((uint)DS90UB9xConfiguration.Register.ReadSize, 0x0010_0009); // 16 frames/superframe, 8x 12-bit words + magic bits
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicMask, 0xC000003F); // Enable inverse, wait for non-inverse, 14-bit magic word
                WriteRegister((uint)DS90UB9xConfiguration.Register.Magic, 0b0000_0000_0010_1110); // Super-frame sync word
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicWait, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataMode, 0b0010_0000_0000_0000_0000_0010_1011_0101);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines0, 0xFFFFF8A6); // NP A
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines1, 0xFFFFF97B); // NP B

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
                }

                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
                {
                    // Deselect both probes
                    DeselectProbes(i2c);

                    // Turn on analog supply
                    gpo10Config |= 1 << 3;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                    SelectProbeA(i2c);
                    ProbeAMetadata = new NeuropixelsV2Flex(DeviceAddress);

                    SelectProbeB(i2c);
                    ProbeBMetadata = new NeuropixelsV2Flex(DeviceAddress);

                }

                // Make sure that we have lock after all that config
                if (ReadRegister((uint)DS90UB9xConfiguration.Register.LinkStatus) == 0)
                {
                    //throw new WorkflowBuildException("Unable to obtain data link with Neuropixels headstage.");
                }
            }
        }

        [Category("Configuration")]
        [Range(1, 10000)]
        [Description("The number of \"Super Frames\" to collect per probe before data is propagated in the observable sequence.")]
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

        [Description("Probe A metadata.")]
        [Category("Configuration")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Externalizable(false)]
        [XmlIgnore]
        public NeuropixelsV2Flex ProbeAMetadata { get; private set; }

        [Description("Probe B metadata.")]
        [Category("Configuration")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Externalizable(false)]
        [XmlIgnore]
        public NeuropixelsV2Flex ProbeBMetadata { get; private set; }


        [Description("Enable headstage LED when aquiring data.")]
        [Category("Configuration")]
        public bool EnableLED { get; set; } = true;

        static void SelectProbeA(I2CRegisterConfiguration i2c)
        {
            i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, ProbeASelected);
            System.Threading.Thread.Sleep(20);
        }

        static void SelectProbeB(I2CRegisterConfiguration i2c)
        {
            i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, ProbeBSelected);
            System.Threading.Thread.Sleep(20);
        }

        static void DeselectProbes(I2CRegisterConfiguration i2c)
        {
            i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, NoProbeSelected);
        }
    }
}
