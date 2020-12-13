using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class MemoryUsageDataFrame : DataFrame
    {
        public MemoryUsageDataFrame(oni.Frame frame, double acq_clk_hz, double data_clk_hz)
            : base(frame, acq_clk_hz, data_clk_hz)
        {
            MemoryUsageBytes = ((ulong)sample[4] << 48) | ((ulong)sample[5] << 32) | ((ulong)sample[6] << 16) | ((ulong)sample[7] << 0);
        }

        public ulong MemoryUsageBytes { get; private set; }
    }
}
