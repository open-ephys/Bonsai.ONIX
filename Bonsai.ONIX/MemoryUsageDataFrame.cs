namespace Bonsai.ONIX
{
    public class MemoryUsageDataFrame : DataFrame
    {
        public MemoryUsageDataFrame(RawDataFrame<ushort> frame, uint total_words)
            : base(frame)
        {
            uint words = ((uint)frame.sample[4] << 16) | ((uint)frame.sample[5] << 0);
            MemoryUsagePercentage = 100.0 * words / total_words;
            MemoryUsageBytes = words * sizeof(uint);
        }

        public ulong MemoryUsageBytes { get; private set; }
        public double MemoryUsagePercentage { get; private set; }

    }
}
