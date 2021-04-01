namespace Bonsai.ONIX
{
    public class TS4231V1DataFrame : U16DataFrame
    {
        public TS4231V1DataFrame(ONIManagedFrame<ushort> frame)
            : base(frame)
        {
            // Data
            Index = frame.Sample[4];
            PulseWidth = ((uint)frame.Sample[5] << 16) | ((uint)frame.Sample[6] << 0);
            PulseType = (short)frame.Sample[7];
        }

        public ushort Index { get; private set; }

        public double PulseWidth { get; private set; }

        public short PulseType { get; private set; }
    }
}
