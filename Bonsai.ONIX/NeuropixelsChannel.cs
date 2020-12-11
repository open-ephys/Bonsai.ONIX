namespace Bonsai.ONIX
{
    public class NeuropixelsChannel
    {
        public enum Gain
        {
            x50 = 50,
            x125 = 125,
            x250 = 250,
            x500 = 500,
            x1000 = 1000,
            x1500 = 1500,
            x2000 = 2000,
            x3000 = 3000,
        }

        public enum ElectrodeBank : uint
        {
            ZERO,
            ONE,
            TWO,
            DISCONNECTED
        }

        public enum Ref : uint
        {
            EXTERNAL = 0,
            TIP = 1,
            INTERNAL = 2
        }

        public ElectrodeBank Bank { get; set; } = ElectrodeBank.ZERO;
        public Gain APGain { get; set; } = Gain.x1000;
        public Gain LFPGain { get; set; } = Gain.x50;
        public bool Standby { get; set; } = false;
        public bool APFilter { get; set; } = false;
        public Ref Reference { get; set; } = Ref.EXTERNAL;
        public int Shank { get; set; } = 0;

    }
}
