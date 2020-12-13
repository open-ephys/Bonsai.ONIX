using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class MemoryUsageDataFrame : DataFrame
    {
        public MemoryUsageDataFrame(oni.Frame frame, double acq_clk_hz, double data_clk_hz, uint total_words)
            : base(frame, acq_clk_hz, data_clk_hz)
        {
            uint words = ((uint)sample[4] << 16) | ((uint)sample[5] << 0);
            MemoryUsagePercentage = 100.0 * (double)words / (double)total_words;
            MemoryUsageBytes = words * sizeof(uint);
        }

        public ulong MemoryUsageBytes { get; private set; }
        public double MemoryUsagePercentage { get; private set; }
        
    }
}
