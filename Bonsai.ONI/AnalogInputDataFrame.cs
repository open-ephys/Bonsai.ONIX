using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONI
{
    /// <summary>
    /// Provides Bonsai-friendly version of an AD7617DataBlock
    /// </summary>
    public class AnalogInputDataFrame
    {
        public AnalogInputDataFrame(AnalogInputDataBlock dataBlock, double hardware_clock_hz)
        {
            Clock = GetClock(dataBlock.Clock);
            Time = GetTime(dataBlock.Clock, hardware_clock_hz);
            Data = GetData(dataBlock.Data);
        }

        Mat GetClock(ulong[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1); // TODO: abusing double to fit uint64_t
        }

        Mat GetTime(ulong[] data, double hardware_clock_hz)
        {
            var ts = new double[data.Count()];
            double period_sec = 1.0 / hardware_clock_hz;

            for(int i = 0; i < data.Count(); i++)
                ts[i] = period_sec * (double)data[i];

            return Mat.FromArray(ts, 1, data.Length, Depth.F64, 1); 
        }

        Mat GetData(short[,] data)
        {
            if (data.Length == 0) return null;
            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat Clock { get; private set; }

        public Mat Time { get; private set; }

        public Mat Data { get; private set; }

    }
}
