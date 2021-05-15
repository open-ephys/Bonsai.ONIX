using System;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class NeuropixelsV1Channel
    {
        public enum Gain
        {
            x50 = 50,
            x125 = 125,
            x250 = 250,
            x500 = 500,
            x1000 = 1000,
            x1500 = 1500,
            x2000 = 2000,
            x3000 = 3000,
        }

        public enum ElectrodeBank : uint
        {
            ZERO,
            ONE,
            TWO,
            DISCONNECTED
        }

        public enum Ref : uint
        {
            EXTERNAL = 0,
            TIP = 1,
            INTERNAL = 2
        }

        ElectrodeBank bank = ElectrodeBank.ZERO;
        List<int> electrodeIndicies;

        private NeuropixelsV1Channel()
        {
            GainCorrectrions = new NeuropixelsV1GainCorrection[] {
                new NeuropixelsV1GainCorrection(),
                new NeuropixelsV1GainCorrection(),
                new NeuropixelsV1GainCorrection()
            };
        }

        public NeuropixelsV1Channel(int index)
        {
            Index = index;
            electrodeIndicies = new List<int>();
            electrodeIndicies.Add(index);
            electrodeIndicies.Add(index + NeuropixelsV1Probe.CHANNEL_COUNT);

            if (index < NeuropixelsV1Probe.INTERNAL_REF_CHANNEL)
            {
                electrodeIndicies.Add(index + 2 * NeuropixelsV1Probe.CHANNEL_COUNT);
            }

            GainCorrectrions = new NeuropixelsV1GainCorrection[] {
                new NeuropixelsV1GainCorrection(),
                new NeuropixelsV1GainCorrection(),
                new NeuropixelsV1GainCorrection()
            };
        }

        public void UpdateGainCorrections(NeuropixelsV1GainCorrection[] gainCorrections)
        {
            for (int i = 0; i <= (int)ElectrodeBank.TWO; i++)
            {
                var e = (i * NeuropixelsV1Probe.CHANNEL_COUNT) + Index;
                if (e < gainCorrections.Length)
                {
                    GainCorrectrions[i] = gainCorrections[e];
                }
            }
        }

        public int Index { get; set; }

        public ElectrodeBank Bank
        {
            get
            {
                return bank;
            }
            set
            {
                if (value == ElectrodeBank.TWO && Index > NeuropixelsV1Probe.INTERNAL_REF_CHANNEL)
                {
                    throw new IndexOutOfRangeException("Bank 2 is only available for channels 0-191");
                }
                else
                {
                    bank = value;
                }
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int? ElectrodeNumber
        {
            get
            {
                if (Bank == ElectrodeBank.DISCONNECTED)
                {
                    return null;
                }
                else
                {
                    return ((int)Bank * NeuropixelsV1Probe.CHANNEL_COUNT) + Index;
                }
            }
        }

        public Gain APGain { get; set; } = Gain.x1000;

        public Gain LFPGain { get; set; } = Gain.x50;

        [System.Xml.Serialization.XmlIgnore]
        public double APGainCorrection
        {
            get { return GainCorrectrions[(int)Bank].APGainCorrection(APGain); }
        }

        [System.Xml.Serialization.XmlIgnore]
        public double LFPGainCorrection
        {
            get { return GainCorrectrions[(int)Bank].LFPGainCorrection(LFPGain); }
        }

        public bool Standby { get; set; } = false;

        public bool APFilter { get; set; } = false;

        public Ref Reference { get; set; } = Ref.EXTERNAL;

        public NeuropixelsV1GainCorrection[] GainCorrectrions { get; set; }
    }
}
