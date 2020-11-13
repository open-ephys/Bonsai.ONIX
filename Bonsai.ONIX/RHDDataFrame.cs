    using OpenCV.Net;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides Bonsai-friendly version of an RHDDataBlock
    /// </summary>
    public class RHDDataFrame : DataBlockFrame
    {
        public RHDDataFrame(RHDDataBlock data_block, double acq_clk_hz, double data_clk_hz) 
            : base(data_block, acq_clk_hz, data_clk_hz)
        {
            EphysData = GetEphysData(data_block.EphysData);
            AuxiliaryData = GetAuxiliaryData(data_block.AuxiliaryData);
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

        public Mat EphysData { get; private set; }

        public Mat AuxiliaryData { get; private set; }
    }
}
