using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class AnalogInputDataFrame : U16DataBlockFrame
    {
        public const int NumberOfChannels = 12;
        public readonly int NumberOfSamples;

        public AnalogInputDataFrame(IList<ONIManagedFrame<ushort>> frameBlock)
            : base(frameBlock)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("Analog input frame buffer is empty.");
            }

            NumberOfSamples = frameBlock.Count;
            var data = new short[NumberOfChannels, NumberOfSamples];

            for (int i = 0; i < NumberOfSamples; i++)
            {
                int chan = 4; // Data starts at index 4

                for (int j = 0; j < NumberOfChannels; j++, chan++)
                {
                    data[j, i] = (short)frameBlock[i].Sample[chan];
                }
            }

            Data = GetData(data);
        }

        Mat GetData(short[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat Data { get; private set; }

    }
}
