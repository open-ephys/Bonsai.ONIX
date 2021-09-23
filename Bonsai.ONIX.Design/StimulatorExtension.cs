using System.Linq;
using ZedGraph;

namespace Bonsai.ONIX.Design
{
    internal static class StimulatorExtension
    {
        #region ElectricalStimulationDevice

        public static string[] WaveformAxisLabels(this ElectricalStimulationDevice _)
        {
            return new[] { "Time (msec)", "Current (uA)" };
        }

        public static int NumberOfChannels(this ElectricalStimulationDevice _)
        {
            return 1;
        }

        public static PointPairList Waveform(this ElectricalStimulationDevice device, int _)
        {
            var phaseOneCurrent = device.PhaseOneCurrent;
            var interPhaseCurrent = device.InterPhaseCurrent;
            var phaseTwoCurrent = device.PhaseTwoCurrent;
            var phaseOneDuration = device.PhaseOneDuration;
            var interPhaseDuration = device.InterPhaseDuration;
            var phaseTwoDuration = device.PhaseTwoDuration;
            var pulsePeriod = device.PulsePeriod;
            var interBurstInterval = device.InterBurstInterval;
            var burstPulseCount = device.BurstPulseCount;
            var trainBurstCount = device.TrainBurstCount;

            var waveform = new PointPairList { new PointPair(0, 0), new PointPair(device.TrainDelay, 0) };

            for (int i = 0; i < trainBurstCount; i++)
            {
                for (int j = 0; j < burstPulseCount; j++)
                {
                    waveform.Add(new PointPair(waveform.Last().X, phaseOneCurrent));
                    waveform.Add(new PointPair(waveform.Last().X + phaseOneDuration, phaseOneCurrent));
                    waveform.Add(new PointPair(waveform.Last().X, interPhaseCurrent));
                    waveform.Add(new PointPair(waveform.Last().X + interPhaseDuration, interPhaseCurrent));
                    waveform.Add(new PointPair(waveform.Last().X, phaseTwoCurrent));
                    waveform.Add(new PointPair(waveform.Last().X + phaseTwoDuration, phaseTwoCurrent));
                    waveform.Add(new PointPair(waveform.Last().X, 0));

                    if (j != device.BurstPulseCount - 1)
                    {
                        waveform.Add(new PointPair(waveform.Last().X + pulsePeriod - (phaseOneDuration + interPhaseDuration + phaseTwoDuration), 0));
                    }
                }

                if (i != device.TrainBurstCount - 1)
                {
                    waveform.Add(new PointPair(waveform.Last().X + interBurstInterval, 0));
                }
            }

            return waveform;
        }
        #endregion

        #region OpticalStimulationDevice

        public static string[] WaveformAxisLabels(this OpticalStimulationDevice _)
        {
            return new[] { "Time (msec)", "Current (mA)" };
        }

        public static int NumberOfChannels(this OpticalStimulationDevice _)
        {
            return 2;
        }

        public static PointPairList Waveform(this OpticalStimulationDevice device, int channel)
        {
            // Cache parameters to prevent redundant hardware access
            var stimulusCurrent = (device.MaxCurrent * (channel == 0 ? device.ChannelZeroCurrent : device.ChannelOneCurrent) / 100.0);
            var pulseDuration = device.PulseDuration;
            var pulsePeriod = device.PulsePeriod;
            var interBurstInterval = device.InterBurstInterval;
            var burstPulseCount = device.BurstPulseCount;
            var trainBurstCount = device.TrainBurstCount;

            // Initial delay
            var waveform = new PointPairList { new PointPair(0, 0), new PointPair(device.TrainDelay, 0) };

            // Stimulus train
            for (int i = 0; i < trainBurstCount; i++)
            {
                for (int j = 0; j < burstPulseCount; j++)
                {
                    waveform.Add(new PointPair(waveform.Last().X, stimulusCurrent));
                    waveform.Add(new PointPair(waveform.Last().X + pulseDuration, stimulusCurrent));
                    waveform.Add(new PointPair(waveform.Last().X, 0));

                    if (j != device.BurstPulseCount - 1)
                    {
                        waveform.Add(new PointPair(waveform.Last().X + pulsePeriod - pulseDuration, 0));
                    }
                }

                if (i != device.TrainBurstCount - 1)
                {
                    waveform.Add(new PointPair(waveform.Last().X + interBurstInterval, 0));
                }
            }

            return waveform;
        }
        #endregion
    }
}
