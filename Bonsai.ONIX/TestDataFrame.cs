namespace Bonsai.ONIX
{
    public class TestDataFrame
    {
        public TestDataFrame(oni.Frame frame)
        {
            // NB: Data contents: [uint64_t remote_clock, ...]
            var sample = frame.Data<ushort>();

            FrameClock = frame.Clock();
            DataClock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);
            Message = ((int)sample[4] << 16) | ((int)sample[5] << 0);
        }

        public ulong FrameClock { get; private set; }

        public ulong DataClock { get; private set; }

        public int Message { get; private set; }

    }
}
