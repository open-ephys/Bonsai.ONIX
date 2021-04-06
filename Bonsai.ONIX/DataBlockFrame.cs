using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class DataBlockFrame
    {
        public DataBlockFrame(DataBlock block)
        {
            FrameClock = GetClock(block.FrameClock);
            DataClock = GetClock(block.DataClock);
        }

        Mat GetClock(ulong[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1); // TODO: abusing double to fit uint64_t
        }

        protected ushort[] sample;
        public Mat FrameClock { get; private set; }
        public Mat DataClock { get; private set; }
    }
}
