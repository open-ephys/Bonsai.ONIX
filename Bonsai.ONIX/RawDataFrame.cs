using OpenCV.Net;
using System;
using System.Linq;

namespace Bonsai.ONIX
{
    public class RawDataFrame : DataFrame
    {
        public RawDataFrame(oni.Frame frame)
            : base(frame)
        {
            var data = new ArraySegment<ushort>(sample, 4, sample.Length - 4);
            Data = Mat.FromArray(data.ToArray(), sample.Length - 4, 1, Depth.U16, 1);
        }

        public Mat Data { get; private set; }
    }
}
