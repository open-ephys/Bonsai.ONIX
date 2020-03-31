namespace Bonsai.ONI
{
    public class HeartbeatDataFrame
    {
        public HeartbeatDataFrame(oni.Frame frame, int device_index, double hardware_clock_hz)
        {
            // NB: Data contents: [uint64_t remote_clock, uint16_t code]
            var sample = frame.Data<ushort>(device_index);

            Clock = ((ulong)sample[0] << 48) | ((ulong)sample[1] << 32) | ((ulong)sample[2] << 16) | ((ulong)sample[3] << 0);
            Time = Clock / hardware_clock_hz;
        }

        public ulong Clock { get; private set; }

        public double Time { get; private set; }
    }
}
