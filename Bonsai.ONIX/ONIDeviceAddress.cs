using System.ComponentModel;

namespace Bonsai.ONIX
{
    public class ONIDeviceAddress
    {

        [Description("ONI hardware translation driver and hardware index tuple supporting this device.")]
        public ONIHardwareSlot HardwareSlot { get; set; } = new ONIHardwareSlot();

        [Description("The address within the device table.")]
        public uint? Address { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(HardwareSlot.Driver)
                ? ""
                : string.Format("{0}: {1}", HardwareSlot.ToString(), Address);
        }
    }
}
