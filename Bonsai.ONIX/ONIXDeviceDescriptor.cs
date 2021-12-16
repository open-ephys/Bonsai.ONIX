using System;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    public class ONIXDeviceDescriptor
    {
        public ONIXDeviceDescriptor(DeviceID id, ONIDeviceAddress address)
        {
            if (string.IsNullOrEmpty(address.HardwareSlot.Driver))
            {
                throw new ArgumentException("Invalid hardware driver.");
            }

            if (!address.Address.HasValue)
            {
                throw new ArgumentException("Invalid device address.");
            }

            using (var c = ONIContextManager.ReserveContext(address.HardwareSlot))
            {
                if (!(c.Context.DeviceTable[(uint)address.Address].ID == (uint)id))
                {
                    throw new ArgumentException("Address " + address.ToString() + " does not contain a device with ID " + id);
                }
            }

            ID = id;
            Address = address;
        }

        [Description("The device type/ID.")]
        public DeviceID ID { get; } = DeviceID.Null;

        [Description("The hardware location and device address within the device table.")]
        public ONIDeviceAddress Address { get; }

        public static bool IsValid(DeviceID id, ONIDeviceAddress address)
        {
            if (string.IsNullOrEmpty(address.HardwareSlot.Driver) || !address.Address.HasValue)
            {
                return false;
            }

            using (var c = ONIContextManager.ReserveContext(address.HardwareSlot))
            {
                if (!(c.Context.DeviceTable[(uint)address.Address].ID == (uint)id))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
