using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONI
{
    /// <summary>
    /// Provides Bonsai-friendly version of an AD7617DataBlock
    /// </summary>
    public class BreakoutDigitalInputDataFrame
    {
        public BreakoutDigitalInputDataFrame(oni.Frame frame, double hardware_clock_hz)
        {
            // NB: Data contents: [uint64_t remote_clock, uint16_t code]
            var sample = frame.Data<ushort>();

            Clock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);
            Time = Clock / hardware_clock_hz;
            Port = sample[4];

            //Buttons = ((ushort)0x00FF & sample[5]);
            Buttons = (ushort)(0x00FF & sample[5]);
            Links = (ushort)((0x0F00 & sample[5]) >> 8);
        }

        public ulong Clock { get; private set; }

        public double Time { get; private set; }

        public ushort Buttons { get; private set; }

        public ushort Links { get; private set; }

        public ushort Port { get; private set; }

    }
}
