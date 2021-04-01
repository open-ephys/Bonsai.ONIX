using OpenCV.Net;
using System;
using System.Linq;

namespace Bonsai.ONIX
{
    public class RawDataFrame : U16DataFrame
    {
        public RawDataFrame(ONIManagedFrame<ushort> frame)
            : base(frame)
        {
            var data = new ArraySegment<ushort>(frame.Sample, 4, frame.Sample.Length - 4);
            Data = Mat.FromArray(data.ToArray(), frame.Sample.Length - 4, 1, Depth.U16, 1);
        }

        public Mat Data { get; private set; }
    }
}
