namespace Bonsai.ONIX
{
    /// <summary>
    /// Base class for most data frames using <see cref="ushort"/> as
    /// the underlying data type.
    /// </summary>
    public class U16DataFrame
    {
        public U16DataFrame(ONIManagedFrame<ushort> frame)
        {
            DataClock = ((ulong)frame.Sample[0] << 48) |
                        ((ulong)frame.Sample[1] << 32) |
                        ((ulong)frame.Sample[2] << 16) |
                        ((ulong)frame.Sample[3] << 0);
            FrameClock = frame.FrameClock;
        }

        /// <summary>
        /// The sample clock, create locally alongside the source device.
        /// </summary>
        public ulong DataClock { get; private set; }

        /// <summary>
        /// The frame clock. Created by the host when receiving the sample from the device.
        /// </summary>
        public ulong FrameClock { get; private set; }

    }
}
