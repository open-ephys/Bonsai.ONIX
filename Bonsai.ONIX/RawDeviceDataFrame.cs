using OpenCV.Net;
using System;
using System.Linq;

namespace Bonsai.ONIX
{
    public class RawDeviceDataFrame : DataFrame
    {
        public RawDeviceDataFrame(RawDataFrame<ushort> frame)
            : base(frame)
        {
            var data = new ArraySegment<ushort>(frame.sample, 4, frame.sample.Length - 4);
            Data = Mat.FromArray(data.ToArray(), frame.sample.Length - 4, 1, Depth.U16, 1);
        }

        public Mat Data { get; private set; }
    }
}
