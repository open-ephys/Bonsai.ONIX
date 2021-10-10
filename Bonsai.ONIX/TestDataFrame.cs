namespace Bonsai.ONIX
{
    public class TestDataFrame : U16DataFrame
    {
        public TestDataFrame(ONIManagedFrame<ushort> frame, ulong frameOffset) : base(frame, frameOffset)
        {
            Message = (frame.Sample[4] << 16) | (frame.Sample[5] << 0);
        }

        public int Message { get; private set; }

    }
}
