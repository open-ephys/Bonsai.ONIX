using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.NeuropixelsV1)]
    [Description("Acquires a stream of \"ultra-frames\" from a single Neuropixels v1.0 probe.")]
    [DefaultProperty("Configuration")]
    public class NeuropixelsV1Device : ONIFrameReader<NeuropixelsV1DataFrame, ushort>
    {
        public NeuropixelsV1Device() { }

        protected override IObservable<NeuropixelsV1DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {

            return Observable.Concat(

                // First sequence is Empty but starts the probe after context reset so that data is aligned
                Observable.Create<NeuropixelsV1DataFrame>(observer =>
                {
                    using (var probe = new NeuropixelsV1Probe(DeviceAddress))
                    {
                        if (RequireSNMatch && Configuration.ConfigProbeSN != Configuration.FlexProbeSN)
                        {
                            throw new Bonsai.WorkflowRuntimeException("Probe and configuration serial numbers do not match.");
                        }

                        if (Configuration.RefreshNeeded)
                        {
                            probe.FullReset();
                            probe.WriteConfiguration(Configuration, PerformReadCheck);
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
                    observer.OnCompleted();
                    return Disposable.Empty;
                }),

                // Process frame stream
                source
                    .Buffer(BlockSize * NeuropixelsV1DataFrame.SuperframesPerUltraFrame)
                    .Select(block => { return new NeuropixelsV1DataFrame(block, frameOffset); })
            );
        }

        private enum Register
        {
            // Managed
            ENABLE = 0x00008000
        }

        [Category("ONI Configuration")]
        [Description("The full device hardware address consisting of a hardware slot and device table index.")]
        [TypeConverter(typeof(ONIDeviceAddressTypeConverter))]
        public override ONIDeviceAddress DeviceAddress
        {
            get { return Configuration.DeviceAddress; }
            set { Configuration.DeviceAddress = value; }
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        [Category("Configuration")]
        [Description("The probe serial number.")]
        [System.Xml.Serialization.XmlIgnore]
        public ulong? ProbeSN => Configuration.FlexProbeSN;

        [Category("Configuration")]
        [Description("The probe part number.")]
        [System.Xml.Serialization.XmlIgnore]
        public string ProbePartNo => Configuration.ProbePartNo;

        [Category("Configuration")]
        [Description("The flex cable version.")]
        [System.Xml.Serialization.XmlIgnore]
        public string FlexVersion => Configuration.FlexVersion;

        [Category("Configuration")]
        [Description("The probe part number.")]
        [System.Xml.Serialization.XmlIgnore]
        public string FlexPartNo => Configuration.FlexPartNo;

        [Category("Configuration")]
        [Description("Neuropixels probe hardware configuration.")]
        [Editor("Bonsai.ONIX.Design.NeuropixelsV1Editor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Externalizable(false)]
        public NeuropixelsV1Configuration Configuration { get; set; } = new NeuropixelsV1Configuration();

        [Category("Configuration")]
        [Description("Write configuration twice to perform a bit-wise confirmation of configuration values on the probe.")]
        public bool PerformReadCheck => Configuration.PerformReadCheck;

        [Category("Configuration")]
        [Description("Require configuration and probe serial numbers to match to start acqusition.")]
        public bool RequireSNMatch { get; set; } = true;

        [Category("Configuration")]
        [Range(1, 1e6)]
        [Description("The size of data blocks, in units of \"ultra-frames\", that are propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 1;
    }
}
