using OpenCV.Net;
using System;

namespace Bonsai.ONIX
{
    public class LoadTestingDataFrame : DataFrame
    {
        public LoadTestingDataFrame(oni.Frame frame)
            : base(frame)
        {
            var data = new ushort[sample.Length - 4];
            Array.Copy(sample, 4, data, 0, data.Length);
            Payload = Mat.FromArray(data, data.Length, 1, Depth.U16, 1);
        }

        public Mat Payload { get; private set; }
    }
}
