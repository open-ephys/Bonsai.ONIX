namespace Bonsai.ONIX
{
    public class DataFrame
    {
        public DataFrame(oni.Frame frame)
        {
            sample = frame.DataU16();

            FrameClock = frame.Clock();
            DataClock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);
        }

        protected ushort[] sample;

        public ulong FrameClock { get; private set; }
        public ulong DataClock { get; private set; }

    }
}
