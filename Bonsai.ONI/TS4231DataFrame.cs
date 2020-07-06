using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONI
{
    public class TS4231DataFrame
    {
        public TS4231DataFrame(oni.Frame frame, double hardware_clock_hz)
        {
            // NB: Data contents:
            var sample = frame.Data<ushort>();

            Clock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);
            Time = Clock / hardware_clock_hz;
            // Data
            ReceiverIndex = sample[4];
            PulseWidth = sample[5] / hardware_clock_hz;
            BeamWord = ((uint)sample[6] << 16) | ((uint)sample[7] << 0);
            //BeamWord = 0xFF_FF_80_00 & ((uint)sample[6] << 16) | ((uint)sample[7] << 0);
            //Offset = (0x7F_FF_C0_00 & ((uint)sample[7] << 16) | ((uint)sample[8] << 0)) / 6e6;
            PolyID = (ushort)(0x00_3F & sample[8]);
        }

        public ulong Clock { get; private set; }

        public double Time { get; set; }

        public double PulseWidth { get; set; }

        public ushort ReceiverIndex { get; set; }

        public uint BeamWord { get; set; }

        //public double Offset { get; set; }

        public ushort PolyID { get; set; }
    }
}
