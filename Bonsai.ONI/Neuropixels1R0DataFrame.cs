    using System.Linq;
    using OpenCV.Net;

namespace Bonsai.ONI
{

    /// <summary>
    /// Provides Bonsai-friendly version of an Neuropixels6BBlock
    /// </summary>
    public class Neuropixels1R0DataFrame
    {
        public Neuropixels1R0DataFrame(Neuropixels1R0DataBlock dataBlock, int hardware_clock_hz)
        {
            LFPClock = GetClock(dataBlock.SpikeClock);
            LFPClock = GetClock(dataBlock.LFPClock);

            SpikeTime = GetTime(dataBlock.SpikeClock, hardware_clock_hz);
            LFPTime = GetTime(dataBlock.LFPClock, hardware_clock_hz);

            SpikeData = GetEphysData(dataBlock.SpikeData);
            LFPData = GetEphysData(dataBlock.LFPData);

            FrameCounter = GetCounter(dataBlock.CounterData);
            FrameType = GetFrameType(dataBlock.FrameType);
        }

        Mat GetClock(ulong[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1); // TODO: abusing double to fit uint64_t
        }

        Mat GetTime(ulong[] data, int hardware_clock_hz)
        {
            var ts = new double[data.Count()];
            double period_sec = 1.0 / (double)hardware_clock_hz;

            for(int i = 0; i < data.Count(); i++)
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

        public Mat SpikeClock { get; private set; }

        public Mat LFPClock { get; private set; }

        public Mat SpikeTime { get; private set; }

        public Mat LFPTime { get; private set; }

        public Mat SpikeData { get; private set; }

        public Mat LFPData { get; private set; }

        public Mat FrameType { get; private set; }

        public Mat FrameCounter { get; private set; }

    }
}
