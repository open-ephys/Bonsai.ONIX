using OpenCV.Net;
using System;

namespace Bonsai.ONIX
{
    public class LoadTestingDataFrame : DataFrame
    {
        public LoadTestingDataFrame(oni.Frame frame, double acq_clk_hz, double data_clk_hz)
            : base(frame, acq_clk_hz, data_clk_hz)
        {
            var data = new ushort[sample.Length - 4];
            Array.Copy(sample, 4, data, 0, data.Length);
            Payload = Mat.FromArray(data, data.Length, 1, Depth.U16, 1);
        }

        public Mat Payload { get; private set; }
    }
}
