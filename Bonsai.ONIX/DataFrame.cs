namespace Bonsai.ONIX
{
    public class DataFrame
    {
        public DataFrame(oni.Frame frame, double acq_clk_hz, double data_clk_hz)
        {
            sample = frame.Data<ushort>();

            FrameClock = frame.Clock();
            DataClock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);

            FrameClockHz = acq_clk_hz;
            DataClockHz = data_clk_hz;
        }

        protected ushort[] sample;

        public double FrameClockHz { get; private set; }
        public double DataClockHz { get; private set; }
        public ulong FrameClock { get; private set; }
        public ulong DataClock { get; private set; }

    }
}
