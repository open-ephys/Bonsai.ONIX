namespace Bonsai.ONIX
{
    public class TestDataFrame : DataFrame
    {
        public TestDataFrame(oni.Frame frame) : base(frame)
        {
            Message = (sample[4] << 16) | (sample[5] << 0);
        }

        public int Message { get; private set; }

    }
}
