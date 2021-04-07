using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class LoadTestingBlockDataFrame : DataBlockFrame
    {

        public LoadTestingBlockDataFrame(LoadTestingDataBlock data_block)
            : base(data_block)
        {
            TestData = GetTestData(data_block.TestData);
        }

        Mat GetTestData(ushort[,] data)
        {
            if (data.Length == 0)
            {
                return null;
            }

            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat TestData { get; private set; }
    }

}
