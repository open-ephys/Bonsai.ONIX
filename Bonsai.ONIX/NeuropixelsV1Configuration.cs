using System.ComponentModel;

namespace Bonsai.ONIX
{
    public class NeuropixelsV1Configuration
    {
        public enum OperationMode : byte
        {
            RECORD,
            CALIBRATE_ADCS,
            CALIBRATE_CHANNELS,
            CALIBRATE_PIXELS,
            DIGITAL_TEST,
        }

        public NeuropixelsV1Configuration()
        {
            InternalReferenceChannels = new int[] { NeuropixelsV1Probe.INTERNAL_REF_CHANNEL };

            Channels = new NeuropixelsV1Channel[NeuropixelsV1Probe.CHANNEL_COUNT];
            for (int i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new NeuropixelsV1Channel(i);
            }

            ADCs = new NeuropixelsV1ADC[NeuropixelsV1Probe.ADC_COUNT];
            for (int i = 0; i < ADCs.Length; i++)
            {
                ADCs[i] = new NeuropixelsV1ADC();
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool RefreshNeeded { get; set; } = true;

        private ONIDeviceAddress dev_address = new ONIDeviceAddress();

        public ONIDeviceAddress DeviceAddress
        {
            get
            {
                return dev_address;
            }
            set
            {
                dev_address = value;

                using (var flex = new NeuropixelsV1Flex(dev_address))
                {
                    FlexProbeSN = flex.ProbeSN;
                    ProbePartNo = flex.ProbePartNo;

                    FlexPartNo = flex.PartNo;
                    FlexVersion = flex.Version;
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public ulong? FlexProbeSN { get; internal set; } = null;

        [System.Xml.Serialization.XmlIgnore]
        public string FlexVersion { get; internal set; }

        [System.Xml.Serialization.XmlIgnore]
        public string FlexPartNo { get; internal set; }

        [System.Xml.Serialization.XmlIgnore]
        public string ProbePartNo { get; internal set; }

        public ulong? ConfigProbeSN { get; set; } = null;

        public bool PerformReadCheck { get; set; } = false;

        public string CalibrationFolderPath { get; set; }

        [Category("Configuration")]
        [Description("Proper operation mode.")]
        public OperationMode Mode { get; set; } = OperationMode.RECORD;

        [Category("Configuration")]
        [Description("Channel configuration.")]
        public NeuropixelsV1Channel[] Channels { get; set; }

        [Category("Configuration")]
        [Description("ADC configuration.")]
        public NeuropixelsV1ADC[] ADCs { get; set; }

        [Category("Configuration")]
        [Description("Channel indices that are used as references.")]
        public readonly int[] InternalReferenceChannels;
    }
}
