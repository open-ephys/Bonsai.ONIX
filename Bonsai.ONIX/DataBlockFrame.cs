using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class DataBlockFrame
    {
        public DataBlockFrame(DataBlock block, double acq_clk_hz, double data_clk_hz)
        {
            FrameClock = GetClock(block.FrameClock);
            DataClock = GetClock(block.DataClock);

            FrameClockHz = acq_clk_hz;
            DataClockHz = data_clk_hz;
        }

        Mat GetClock(ulong[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1); // TODO: abusing double to fit uint64_t
        }

        Mat GetTime(double[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1);
        }

        protected ushort[] sample;

        public double FrameClockHz { get; private set; }
        public double DataClockHz { get; private set; }
        public Mat FrameClock { get; private set; }
        public Mat DataClock { get; private set; }
    }
}
