namespace Bonsai.ONIX
{
    public class BreakoutDigitalInputDataFrame : DataFrame
    {
        public BreakoutDigitalInputDataFrame(RawDataFrame<ushort> frame)
            : base(frame)
        {
            Port = frame.sample[4];
            Buttons = (byte)(0x00FF & frame.sample[5]);
            Links = (byte)((0x0F00 & frame.sample[5]) >> 8);
        }

        public byte Buttons { get; private set; }

        public byte Links { get; private set; }

        public ushort Port { get; private set; }

    }
}
