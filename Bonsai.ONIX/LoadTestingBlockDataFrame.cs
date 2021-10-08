using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class LoadTestingBlockDataFrame : U16DataBlockFrame
    {
        public readonly int NumberOfFrames;
        public readonly int FrameWords;

        public LoadTestingBlockDataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset, int frameWords)
            : base(frameBlock, frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new WorkflowRuntimeException("Load testing input frame buffer is empty.");
            }

            NumberOfFrames = frameBlock.Count;
            FrameWords = frameWords;

            var delta = new double[NumberOfFrames];

            //var payload = new ushort[FrameWords, NumberOfFrames];

            for (int j = 0; j < NumberOfFrames; j++)
            {
                delta[j] = ((ulong)frameBlock[j].Sample[4] << 48) |
                           ((ulong)frameBlock[j].Sample[5] << 32) |
                           ((ulong)frameBlock[j].Sample[6] << 16) |
                           ((ulong)frameBlock[j].Sample[7] << 0);
            }

            Delta = GetDelta(delta);
            //Payload = GetPayload(payload);
        }

        private Mat GetDelta(double[] data)
        {
            var output = new Mat(1, NumberOfFrames, Depth.F64, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        //Mat GetPayload(ushort[,] data)
        //{
        //    var output = new Mat(FrameWords, NumberOfFrames, Depth.U16, 1);
        //    using (var header = Mat.CreateMatHeader(data))
        //    {
        //        CV.Convert(header, output);
        //    }

        //    return output;
        //}

        public Mat Delta { get; private set; }

        //public Mat Payload { get; private set; }
    }

}
