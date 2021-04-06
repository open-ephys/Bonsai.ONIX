namespace Bonsai.ONIX
{
    /// <summary>
    /// Managed copy of <see cref="oni.Frame"/> with strongly-typed data array.
    /// </summary>
    /// <typeparam name="T">The data type of the Sample array</typeparam>
    public class ONIManagedFrame<T> where T : unmanaged
    {
        public ONIManagedFrame(oni.Frame frame)
        {
            Sample = frame.Data<T>();
            FrameClock = frame.Clock;
            DeviceAddress = frame.DeviceAddress;
        }

        public readonly T[] Sample;

        public ulong FrameClock { get; private set; }

        public uint DeviceAddress { get; private set; }
    }
}
