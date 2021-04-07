using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class LoadTestingDataBlock : DataBlock
    {
        public readonly int numWords;

        readonly ushort[,] testData;

        public LoadTestingDataBlock(int num_words, int samples_per_block) : base (samples_per_block)
        {
            numWords = num_words;
            AllocateArray2D(ref testData, num_words, samples_per_block);
        }

        protected override void FillFromData(ushort[] data)
        {
            for (int word = 0; word < numWords; word++)
            {
                testData[word, index] = data[word + 4];
            }
        }

        protected bool FillFromFrame(LoadTestingDataFrame frame)
        {
            FrameClock[index] = frame.FrameClock;
            DataClock[index] = frame.DataClock;

            IntPtr data = frame.Payload.Data;
            unsafe
            {
                ushort* sample = (ushort*)data;
                for (int word = 0; word < numWords; word++)
                {
                    testData[word, index] = sample[word];
                }
            }


            return ++index == SamplesPerBlock;
        }

        /// <summary>
        /// Gets the array of multidimensional test data samples
        /// </summary>
        public ushort[,] TestData
        {
            get { return testData; }
        }
    }

}
