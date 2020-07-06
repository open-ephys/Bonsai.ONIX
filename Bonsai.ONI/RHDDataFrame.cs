    using System.Linq;
    using OpenCV.Net;

namespace Bonsai.ONI
{
    /// <summary>
    /// Provides Bonsai-friendly version of an RHDDataBlock
    /// </summary>
    public class RHDDataFrame
    {
        public RHDDataFrame(RHDDataBlock dataBlock, int hardware_clock_hz)
        {
            Clock = GetClock(dataBlock.Clock);
            Time = GetTime(dataBlock.Clock, hardware_clock_hz);
            EphysData = GetEphysData(dataBlock.EphysData);
            AuxiliaryData = GetAuxiliaryData(dataBlock.AuxiliaryData);
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

        Mat GetAuxiliaryData(int[,] data)
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

        public Mat Clock { get; private set; }

        public Mat Time { get; private set; }

        public Mat EphysData { get; private set; }

        public Mat AuxiliaryData { get; private set; }

    }
}
