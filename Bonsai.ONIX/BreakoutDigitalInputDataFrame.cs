namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides Bonsai-friendly version of an AD7617DataBlock
    /// </summary>
    public class BreakoutDigitalInputDataFrame : DataFrame
    {
        public BreakoutDigitalInputDataFrame(oni.Frame frame)
            : base(frame)
        {
            Port = sample[4];
            Buttons = (ushort)(0x00FF & sample[5]);
            Links = (ushort)((0x0F00 & sample[5]) >> 8);
        }

        public ushort Buttons { get; private set; }

        public ushort Links { get; private set; }

        public ushort Port { get; private set; }

    }
}
