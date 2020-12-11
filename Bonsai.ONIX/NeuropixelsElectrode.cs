using System.ComponentModel;

namespace Bonsai.ONIX
{
    public class NeuropixelsElectrode
    {
        // This is terrible, but...
        // Using dictionaries results in _huge_ performance drop when doing deep copy inside the editor

        public double APx50Correction { get; set; } = 1.0;
        public double APx125Correction { get; set; } = 1.0;
        public double APx250Correction { get; set; } = 1.0;
        public double APx500Correction { get; set; } = 1.0;
        public double APx1000Correction { get; set; } = 1.0;
        public double APx1500Correction { get; set; } = 1.0;
        public double APx2000Correction { get; set; } = 1.0;
        public double APx3000Correction { get; set; } = 1.0;

        public double LFPx50Correction { get; set; } = 1.0;
        public double LFPx125Correction { get; set; } = 1.0;
        public double LFPx250Correction { get; set; } = 1.0;
        public double LFPx500Correction { get; set; } = 1.0;
        public double LFPx1000Correction { get; set; } = 1.0;
        public double LFPx1500Correction { get; set; } = 1.0;
        public double LFPx2000Correction { get; set; } = 1.0;
        public double LFPx3000Correction { get; set; } = 1.0;

        public double APGainCorrection(NeuropixelsChannel.Gain gain)
        {
            switch (gain)
            {
                case NeuropixelsChannel.Gain.x50:
                    return APx50Correction;
                case NeuropixelsChannel.Gain.x125:
                    return APx125Correction;
                case NeuropixelsChannel.Gain.x250:
                    return APx250Correction;
                case NeuropixelsChannel.Gain.x500:
                    return APx500Correction;
                case NeuropixelsChannel.Gain.x1000:
                    return APx1000Correction;
                case NeuropixelsChannel.Gain.x1500:
                    return APx1500Correction;
                case NeuropixelsChannel.Gain.x2000:
                    return APx2000Correction;
                case NeuropixelsChannel.Gain.x3000:
                    return APx3000Correction;
                default:
                    throw new InvalidEnumArgumentException("gain", (int)gain, typeof(NeuropixelsChannel.Gain));
            }
        }

        public double LFPGainCorrection(NeuropixelsChannel.Gain gain)
        {
            switch (gain)
            {
                case NeuropixelsChannel.Gain.x50:
                    return LFPx50Correction;
                case NeuropixelsChannel.Gain.x125:
                    return LFPx125Correction;
                case NeuropixelsChannel.Gain.x250:
                    return LFPx250Correction;
                case NeuropixelsChannel.Gain.x500:
                    return LFPx500Correction;
                case NeuropixelsChannel.Gain.x1000:
                    return LFPx1000Correction;
                case NeuropixelsChannel.Gain.x1500:
                    return LFPx1500Correction;
                case NeuropixelsChannel.Gain.x2000:
                    return LFPx2000Correction;
                case NeuropixelsChannel.Gain.x3000:
                    return LFPx3000Correction;
                default:
                    throw new InvalidEnumArgumentException("gain", (int)gain, typeof(NeuropixelsChannel.Gain));
            }
        }

    }
}
