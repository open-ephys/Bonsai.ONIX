using OpenCV.Net;
using System.Linq;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides Bonsai-friendly version of an Neuropixels 1.0 Hyperframe
    /// </summary>
    public class NeuropixelsV1DataFrame
    {
        public NeuropixelsV1DataFrame(NeuropixelsV1DataBlock dataBlock, double acq_clk_hz, double data_clk_hz)
        {
            SpikeFrameClock = GetClock(dataBlock.SpikeDataClock);
            LFPFrameClock = GetClock(dataBlock.LFPDataClock);

            SpikeDataClock = GetClock(dataBlock.SpikeDataClock);
            LFPDataClock = GetClock(dataBlock.LFPDataClock);

            SpikeData = GetEphysData(dataBlock.SpikeData);
            LFPData = GetEphysData(dataBlock.LFPData);

            FrameCounter = GetCounter(dataBlock.CounterData);
            FrameType = GetFrameType(dataBlock.FrameType);

            FrameClockHz = acq_clk_hz;
            DataClockHz = data_clk_hz;
        }

        public double FrameClockHz { get; private set; }
        public double DataClockHz { get; private set; }

        Mat GetClock(ulong[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1); // TODO: abusing double to fit uint64_t
        }

        Mat GetTime(ulong[] data, int hardware_clock_hz)
        {
            var ts = new double[data.Count()];
            double period_sec = 1.0 / (double)hardware_clock_hz;

            for (int i = 0; i < data.Count(); i++)
                ts[i] = period_sec * (double)data[i];

            return Mat.FromArray(ts, 1, data.Length, Depth.F64, 1);
        }

        Mat GetEphysData(ushort[,] data)
        {
            if (data.Length == 0) return null;
            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        Mat GetCounter(uint[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.S32, 1); // Abusing S32 to fit U20
        }

        Mat GetFrameType(ushort[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.U16, 1);
        }

        public Mat SpikeFrameClock { get; private set; }

        public Mat LFPFrameClock { get; private set; }

        public Mat SpikeDataClock { get; private set; }

        public Mat LFPDataClock { get; private set; }

        public Mat SpikeData { get; private set; }

        public Mat LFPData { get; private set; }

        public Mat FrameType { get; private set; }

        public Mat FrameCounter { get; private set; }

    }
}
