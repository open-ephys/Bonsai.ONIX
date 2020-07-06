using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONI
{
    public class LightHouseDataFrame
    {
        public LightHouseDataFrame(oni.Frame frame, double hardware_clock_hz)
        {
            // NB: Data contents: [uint64_t remote_clock, uint16_t width, int16_t type]
            var sample = frame.Data<ushort>();

            Clock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);
            Time = Clock / hardware_clock_hz;

            // Data
            PulseWidth = sample[4] / hardware_clock_hz;
            PulseType = (short)sample[5];
        }

        public ulong Clock { get; private set; }

        public double Time { get; set; }

        public double PulseWidth { get; set; }

        public short PulseType { get; set; }
    }
}
