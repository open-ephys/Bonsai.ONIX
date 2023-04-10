using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{
    public class RHS2116StimulusSequence
    {
        
        public RHS2116StimulusSequence()
        {
            Stimuli = new RHS2116Stimulus[16];
            for (var i = 0; i < Stimuli.Length; i++)
            {
                Stimuli[i] = new RHS2116Stimulus();
            }
        }

        public enum StepSize
        {
            Step10nA,
            Step20nA,
            Step50nA,
            Step100nA,
            Step200nA,
            Step500nA,
            Step1000nA,
            Step2000nA,
            Step5000nA,
            Step10000nA
        }

        public RHS2116Stimulus[] Stimuli { get; set; }

        public StepSize CurrentStepSize { get; set; } = StepSize.Step5000nA;

        /// <summary>
        /// Maximum length of the sequence across all channels
        /// </summary>
        [XmlIgnore]
        public uint SequenceLengthSamples
        {
            get
            {
                uint max = 0;

                foreach (var stim in Stimuli)
                {
                    var len = stim.DelaySamples + stim.NumberOfStimuli * (stim.AnodicWidthSamples + stim.CathodicWidthSamples + stim.DwellSamples + stim.InterStimulusIntervalSamples);
                    max = len > max ? len : max;

                }

                return max;
            }
        }

        /// <summary>
        /// Maximum peak to peak amplitude of the sequence across all channels.
        /// </summary>
        [XmlIgnore]
        public int MaximumPeakToPeakAmplitudeSteps
        {
            get
            {
                int max = 0;

                foreach (var stim in Stimuli)
                {
                    var p2p = stim.CathodicAmplitudeSteps + stim.AnodicAmplitudeSteps;
                    max = p2p > max ? p2p : max;

                }

                return max;
            }
        }

        /// <summary>
        /// Is the stimulus sequence well define
        /// </summary>
        [XmlIgnore]
        public bool Valid => Stimuli.ToList().All(s => s.Valid);

        /// <summary>
        /// Does the sequence fit in hardware
        /// </summary>
        [XmlIgnore]
        public bool FitsInHardware => StimulusSlotsRequired <= RHS2116Device.StimMemorySlotsAvailable;

        /// <summary>
        /// Number of hardware memory slots required by the sequence
        /// </summary>
        [XmlIgnore]
        public int StimulusSlotsRequired => DeltaTable.Count;

        [XmlIgnore]
        public double CurrentStepSizeuA
        {
            get
            {
                switch (CurrentStepSize)
                {
                    case StepSize.Step10nA:
                        return 0.01;
                    case StepSize.Step20nA:
                        return 0.02;
                    case StepSize.Step50nA:
                        return 0.05;
                    case StepSize.Step100nA:
                        return 0.1;
                    case StepSize.Step200nA:
                        return 0.2;
                    case StepSize.Step500nA:
                        return 0.5;
                    case StepSize.Step1000nA:
                        return 1.0;
                    case StepSize.Step2000nA:
                        return 2.0;
                    case StepSize.Step5000nA:
                        return 5.0;
                    case StepSize.Step10000nA:
                        return 10.0;
                    default:
                        throw new ArgumentException("Invalid stimulus step size selection.");
                }
            }
        }

        [XmlIgnore]
        public double MaxPossibleAmplitudePerPhaseMicroAmps  => CurrentStepSizeuA * 255;

        internal IEnumerable<byte> ToAmplitudes(bool Anodic)
        {
            return Anodic ? Stimuli.ToList().Select(x => x.AnodicAmplitudeSteps) :
                            Stimuli.ToList().Select(x => x.CathodicAmplitudeSteps);
        }

        /// <summary>
        /// Generate the delta-table  representation of this stimulus sequence that can be uploaded to the RHS2116 device.
        /// The resultant dictionary has a time, in samples as the key and a combimed [polary, enable] bit field as the value.
        /// </summary>
        [XmlIgnore]
        internal Dictionary<uint, uint> DeltaTable
        {
            get
            {
                var table = new Dictionary<uint, BitArray>();

                // Cycle through electrodes
                for (int i = 0; i < Stimuli.Length; i++)
                {
                    var s = Stimuli[i];

                    var e0 = s.AnodicFirst ? s.AnodicAmplitudeSteps > 0 : s.CathodicAmplitudeSteps > 0;
                    var e1 = s.AnodicFirst ? s.CathodicAmplitudeSteps > 0 : s.AnodicAmplitudeSteps > 0;
                    var d0 = s.AnodicFirst ? s.AnodicWidthSamples : s.CathodicWidthSamples;
                    var d1 = d0 + s.DwellSamples;
                    var d2 = d1 + (s.AnodicFirst ? s.CathodicWidthSamples : s.AnodicWidthSamples);

                    var t0 = s.DelaySamples;

                    for (int j = 0; j < s.NumberOfStimuli; j++)
                    {
                        AddOrInsert(ref table, i, t0, s.AnodicFirst, e0);
                        AddOrInsert(ref table, i, t0 + d0, s.AnodicFirst, false);
                        AddOrInsert(ref table, i, t0 + d1, !s.AnodicFirst, e1);
                        AddOrInsert(ref table, i, t0 + d2, !s.AnodicFirst, false);

                        t0 += d2 + s.InterStimulusIntervalSamples;
                    }
                }

                return table.ToDictionary(d => d.Key, d =>
                    {
                        int[] i = new int[1];
                        d.Value.CopyTo(i, 0);
                        return (uint)i[0];
                    });
            }
        }

        private static void AddOrInsert(ref Dictionary<uint, BitArray> table, int channel, uint key, bool pol, bool en)
        {
            if (table.ContainsKey(key))
            {
                table[key][channel] = en;
                table[key][channel + 16] = pol;
            } else
            {
                table.Add(key, new BitArray(32, false));
                table[key][channel] = en;
                table[key][channel + 16] = pol;
            }
        }
    }
}
