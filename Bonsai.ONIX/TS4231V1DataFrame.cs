namespace Bonsai.ONIX
{
    public class TS4231V1DataFrame : DataFrame
    {
        public TS4231V1DataFrame(RawDataFrame<ushort> frame)
            : base(frame)
        {
            // Data
            Index = frame.sample[4];
            PulseWidth = ((uint)frame.sample[5] << 16) | ((uint)frame.sample[6] << 0);
            PulseType = (short)frame.sample[7];
        }

        public ushort Index { get; private set; }

        public double PulseWidth { get; private set; }

        public short PulseType { get; private set; }
    }
}
