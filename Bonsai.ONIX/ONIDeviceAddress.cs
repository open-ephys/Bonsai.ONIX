using System.ComponentModel;

namespace Bonsai.ONIX
{
    public class ONIDeviceAddress
    {
        [Description("ONI hardware translation driver and hardware index tuple supporting this device.")]
        public ONIHardwareSlot HardwareSlot { get; set; } = new ONIHardwareSlot();

        [Description("The fully specified device address within the device table.")]
        public uint Address { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(HardwareSlot.Driver))
            {
                return "";
            }
            else
            {
                return string.Format("({0},{1}): {2}", HardwareSlot.Driver, HardwareSlot.Index, Address);
            }
        }
    }
}
