namespace Bonsai.ONIX
{
    public class HARPSyncInputDataFrame : U16DataFrame
    {
        public HARPSyncInputDataFrame(ONIManagedFrame<ushort> frame, ulong frameOffset)
            : base(frame, frameOffset)
        {
            HARPTime = ((uint)frame.Sample[4] << 16) | ((uint)frame.Sample[5] << 0);
        }

        public uint HARPTime { get; private set; }
    }
}
