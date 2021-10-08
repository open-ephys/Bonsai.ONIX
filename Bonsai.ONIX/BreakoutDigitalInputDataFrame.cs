namespace Bonsai.ONIX
{
    public class BreakoutDigitalInputDataFrame : U16DataFrame
    {
        public BreakoutDigitalInputDataFrame(ONIManagedFrame<ushort> frame, ulong frameOffset)
            : base(frame, frameOffset)
        {
            Port = (byte)(0x00FF & frame.Sample[4]);
            Buttons = (byte)(0x00FF & frame.Sample[5]);
            Links = (byte)((0x0F00 & frame.Sample[5]) >> 8);
        }

        public byte Buttons { get; private set; }

        public byte Links { get; private set; }

        public byte Port { get; private set; }

    }
}
