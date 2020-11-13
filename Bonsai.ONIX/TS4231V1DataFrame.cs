namespace Bonsai.ONIX
{
    public class TS4231V1DataFrame : DataFrame
    {
        public TS4231V1DataFrame(oni.Frame frame, double acq_clk_hz, double data_clk_hz) 
            : base(frame, acq_clk_hz, data_clk_hz)
        {
            // Data
            Index = sample[4];
            PulseWidth = ((uint)sample[5] << 16) | ((uint)sample[6] << 0);
            PulseType = (short)sample[7];
        }

        public ushort Index { get; private set; }

        public double PulseWidth { get; private set; }

        public short PulseType { get; private set; }
    }
}
