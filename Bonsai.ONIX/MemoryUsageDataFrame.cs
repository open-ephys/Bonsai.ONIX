namespace Bonsai.ONIX
{
    public class MemoryUsageDataFrame : U16DataFrame
    {
        public MemoryUsageDataFrame(ONIManagedFrame<ushort> frame, ulong frameOffset, uint total_words)
            : base(frame, frameOffset)
        {
            uint words = ((uint)frame.Sample[4] << 16) | ((uint)frame.Sample[5] << 0);
            MemoryUsagePercentage = 100.0 * words / total_words;
            MemoryUsageBytes = words * sizeof(uint);
        }

        public ulong MemoryUsageBytes { get; private set; }
        public double MemoryUsagePercentage { get; private set; }

    }
}
