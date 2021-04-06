using OpenCV.Net;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides Bonsai-friendly version of an AD7617DataBlock
    /// </summary>
    public class AnalogInputDataFrame : DataBlockFrame
    {
        public AnalogInputDataFrame(AnalogInputDataBlock data_block)
            : base(data_block)
        {
            Data = GetData(data_block.Data);
        }

        Mat GetData(short[,] data)
        {
            if (data.Length == 0)
            {
                return null;
            }

            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat Data { get; private set; }

    }
}
