using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class LoadTestingBlockDataFrame : U16DataBlockFrame
    {
        public readonly int NumberOfFrames;
        public readonly int FrameWords;

        public LoadTestingBlockDataFrame(IList<ONIManagedFrame<ushort>> frameBlock, int frameWords)
            : base(frameBlock)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("Load testing input frame buffer is empty.");
            }

            NumberOfFrames = frameBlock.Count;
            FrameWords = frameWords;
            var payload = new ushort[FrameWords, NumberOfFrames];

            for (int j = 0; j < NumberOfFrames; j++)
            {
                for (int i = 0; i < FrameWords; i++)
                {
                    payload[i, j] = frameBlock[j].Sample[i + 4];
                }

            }
            Payload = GetPayload(payload);
        }

        Mat GetPayload(ushort[,] data)
        {
            var output = new Mat(FrameWords, NumberOfFrames, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat Payload { get; private set; }
    }

}
