namespace Bonsai.ONIX
{
    public class TestDataFrame : DataFrame
    {
        public TestDataFrame(RawDataFrame<ushort> frame) : base(frame)
        {
            Message = (frame.sample[4] << 16) | (frame.sample[5] << 0);
        }

        public int Message { get; private set; }

    }
}
