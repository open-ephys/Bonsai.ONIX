namespace Bonsai.ONIX
{
    public class ONIContextConfiguration
    {
        public ONIHardwareSlot Slot { get; set; } = new ONIHardwareSlot();
        public int ReadSize { get; set; } = 2048;
        public int WriteSize { get; set; } = 2048;
        public override string ToString()
        {
            return Slot.ToString();
        }
    }
}