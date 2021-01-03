using System.ComponentModel;

namespace Bonsai.ONIX
{
    public class NeuropixelsConfiguration
    {
        public enum OperationMode : byte
        {
            RECORD,
            CALIBRATE_ADCS,
            CALIBRATE_CHANNELS,
            CALIBRATE_PIXELS,
            DIGITAL_TEST,
        }

        public int? GetElectrode(int channel)
        {
            if (Channels[channel].Bank == NeuropixelsChannel.ElectrodeBank.DISCONNECTED)
            {
                return null;
            }
            else
            {
                return ((int)Channels[channel].Bank * Channels.Length) + channel;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string ProbeType { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public ulong? ProbeSN { get; set; } = 0;

        public OperationMode Mode { get; set; } = OperationMode.RECORD;

        [Category("Configuration")]
        [Description("Channel configuration.")]
        public NeuropixelsChannel[] Channels { set; get; }

        [Category("Configuration")]
        [Description("ADC configuration.")]
        public NeuropixelsADC[] ADCs { set; get; }

        [Category("Configuration")]
        [Description("Electrode configuration.")]
        public NeuropixelsElectrode[] Electrodes { set; get; }

        [Category("Configuration")]
        [Description("Channel indices that are used as references.")]
        public int[] InternalReferenceChannels { get; set; }


    }
}
