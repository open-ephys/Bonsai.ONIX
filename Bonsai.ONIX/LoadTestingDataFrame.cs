using OpenCV.Net;
using System;

namespace Bonsai.ONIX
{
    public class LoadTestingDataFrame : U16DataFrame
    {
        public LoadTestingDataFrame(ONIManagedFrame<ushort> frame, ulong frameOffset)
            : base(frame, frameOffset)
        {

            Delta = ((ulong)frame.Sample[4] << 48) |
                ((ulong)frame.Sample[5] << 32) |
                ((ulong)frame.Sample[6] << 16) |
                ((ulong)frame.Sample[7] << 0);

            var data = new ushort[frame.Sample.Length - 8];
            Array.Copy(frame.Sample, 8, data, 0, data.Length);
            Payload = Mat.FromArray(data, 1, data.Length, Depth.U16, 1);
        }

        /// <summary>
        /// Delta
        /// </summary>
        public ulong Delta { get; private set; }

        /// <summary>
        /// Matrix containing the payload used for stress testing host communication.
        /// This matrix contains the number 42 over and over again.
        /// </summary>
        public Mat Payload { get; private set; }
    }
}
