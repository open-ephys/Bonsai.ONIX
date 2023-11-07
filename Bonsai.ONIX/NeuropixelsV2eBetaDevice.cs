using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.DS90UB9X)]
    [Description("Acquire data from two probes using Neuropixels 2.0e Headstage.")]
    public class NeuropixelsV2eBetaDevice : ONIFrameReader<NeuropixelsV2BetaDataFrame, ushort>
    {
        private const int NeuropixelsAddress = 0x70;

        // All GPIO set to locally (I2C) controlled outputs
        private const byte DefaultGPO10Config = 0b0001_0001; // NPs in MUX reset, NPs in RST
        private const byte DefaultGPO32Config = 0b1001_1001; // LED off, NP_A selected

        private int gpo10Config = DefaultGPO10Config;
        private int gpo32Config = DefaultGPO32Config;

        protected override IObservable<NeuropixelsV2BetaDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            // Toggle LED and enable NPs (must be done before I2C communication with probes)
            using (I2CRegisterConfiguration 
                   i2cSer = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress),
                   i2cNP = new I2CRegisterConfiguration(DeviceAddress, ID, NeuropixelsAddress))
            {
                // Toggle LED
                gpo32Config = (gpo32Config & 0b0111_1111) | (EnableLED ? 0 : 1 << 7);
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)gpo32Config);

                // REC_NRESET and NRESET go high on both probes to take the ASIC out of reset
                // TODO: not sure if REC_NRESET and NRESET are tied together on flex
                gpo10Config |= (1 << 3 | 1 << 7);
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                // Wait 20 milliseconds
                System.Threading.Thread.Sleep(20);

                // TODO: Handle case where one or no probes are attached.
                if (ProbeAMetadata.ProbeSN == null && ProbeBMetadata.ProbeSN == null)
                {
                    throw new WorkflowRuntimeException("No neuropixel probes available.");
                }

                if (ProbeAMetadata.ProbeSN != null)
                {
                    SelectProbeA(i2cSer);

                    // TODO: Apply configuration to NP A
                    i2cNP.WriteByte(0, 0b0100_0000); // Activate recording mode on NP A
                }

                if (ProbeBMetadata.ProbeSN != null)
                {
                    SelectProbeB(i2cSer);

                    // TODO: Apply configuration to NP B
                    i2cNP.WriteByte(0, 0b0100_0000); // Activate recording mode on NP B
                }

                // Both probes are now streaming, hit them with a mux reset to (roughly) sync.
                // NB: We have found that this gives PCLK-level synchronization MOST of the time.
                // However, this is not required since we have a decoder that can handle async streams.
                // Still its good to get them roughly (i.e. within 10 PCLKs) started at the same time.
                gpo10Config &= ~(1 << 7);
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

                gpo10Config |= 1 << 7;
                i2cSer.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);

            }

            // Lock in BlockSize so that it cannot be changed during acquisition
            var bufferSize = BlockSize;


            //return source
            //  .GroupBy(f => f.Sample[4]) // Group by NP index
            //  .Select(group => new { group.Key, Events = group })
            //  .Select(group => new { group.Key, Buffer = group.Events.Buffer(bufferSize) })
            //  //.Merge()
            //  .Select(b => { return new NeuropixelsV2DataFrame(b.Buffer, b.Key, frameOffset); })
            //  .Finally(() =>
            //  {
            //      using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
            //      {
            //          i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)DefaultGPO10Config);
            //          i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)DefaultGPO32Config);
            //      }
            //  });


            return source
                .GroupBy(f => f.Sample[4]) // Group by NP index
                .SelectMany(group => group.Buffer(bufferSize))
                .Select(b => { return new NeuropixelsV2BetaDataFrame(b, frameOffset); })
                .Finally(() =>
                {
                    using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.SerializerDefaultAddress))
                    {
                        i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)DefaultGPO10Config); 
                        i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)DefaultGPO32Config); 
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
                WriteRegister((uint)DS90UB9xConfiguration.Register.ReadSize, 0x0010_0007); // 16 frames/superframe, 7x 14-bit words on each serial line per frame
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicMask, 0b1100000000000000_0011111111111111); // Enable inverse, wait for non-inverse, 14-bit magic word
                WriteRegister((uint)DS90UB9xConfiguration.Register.Magic, 0b0011_0011_0011_0000); // Super-frame sync word
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicWait, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataMode, 0b10_1101_0101);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines0, 0x00007654); // NP A
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines1, 0x00000123); // NP B

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
                    // Change all the GPIOs to locally-controlled outputs; output state set to default
                    i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO10, (uint)gpo10Config);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)gpo32Config);

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

        void SelectProbeA(I2CRegisterConfiguration i2c)
        {
            gpo32Config |= (1 << 3);
            i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)gpo32Config);

            // Wait 20 milliseconds. Apparently the analog mux has some delay to settle. We need to check the values properly
            System.Threading.Thread.Sleep(20);
        }

        void SelectProbeB(I2CRegisterConfiguration i2c)
        {
            gpo32Config &= ~(1 << 3);
            i2c.WriteByte((uint)DS90UB9xConfiguration.SerI2CRegister.GPIO32, (uint)gpo32Config);

            // Wait 20 milliseconds. Apparently the analog mux has some delay to settle. We need to check the values properly
            System.Threading.Thread.Sleep(20);
        }
    }
}
