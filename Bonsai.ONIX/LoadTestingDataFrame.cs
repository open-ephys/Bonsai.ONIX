using OpenCV.Net;
using System;

namespace Bonsai.ONIX
{
    public class LoadTestingDataFrame : U16DataFrame
    {
        public LoadTestingDataFrame(ONIManagedFrame<ushort> frame)
            : base(frame)
        {
            var data = new ushort[frame.Sample.Length - 4];
            Array.Copy(frame.Sample, 4, data, 0, data.Length);
            Payload = Mat.FromArray(data, data.Length, 1, Depth.U16, 1);
        }

        public Mat Payload { get; private set; }
    }
}
