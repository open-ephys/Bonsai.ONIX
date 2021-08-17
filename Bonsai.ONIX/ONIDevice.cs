using System.ComponentModel;

namespace Bonsai.ONIX
{
    public abstract class ONIDevice
    {
        [Description("The device type/ID.")]
        internal ONIXDevices.ID ID { get; set; } = ONIXDevices.ID.Null;

        [Category("ONI Configuration")]
        [Description("The full device hardware address consisting of a hardware slot and device table index.")]
        [TypeConverter(typeof(ONIDeviceAddressTypeConverter))]
        public virtual ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        [Category("ONI Configuration")]
        [Description("The hub that this device belongs to.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Externalizable(true)]
        public oni.Hub Hub { get; set; }

        [Category("ONI Configuration")]
        [Description("The rate of the host frame clock counter (Hz).")]
        [Externalizable(true)]
        [ReadOnly(true)]
        public double? FrameClockHz { get; set; }

        public ONIDevice(ONIXDevices.ID deviceID)
        {
            ID = deviceID;
        }

        // NB: Write/ReadRegister are used extensively in node property settings.
        // This means they cannot throw or there will be a variety of consequences
        // such as errors loading XML descriptions of workflows and terrible performance
        // if a valid device is not yet selected. As an intermediate solution,
        // I'm just dumping errors to the Console and returning 0 or doing nothing.
        protected uint ReadRegister(uint address)
        {
            if (DeviceAddress == null || !DeviceAddress.Valid)
            {
                System.Console.Error.WriteLine("Device is not valid.");
                return 0;
            }

            try
            {
                using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                {
                    return c.Context.ReadRegister((uint)DeviceAddress.Address, address);
                }
            }
            catch (oni.ONIException ex)
            {
                System.Console.Error.WriteLine(ex.Message);
                return 0;
            }
        }

        protected void WriteRegister(uint address, uint value)
        {
            if (DeviceAddress == null || !DeviceAddress.Valid)
            {
                System.Console.Error.WriteLine("Device is not valid.");
                return;
            }

            try
            {
                using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                {
                    c.Context.WriteRegister((uint)DeviceAddress.Address, address, value);
                }
            }
            catch (oni.ONIException ex)
            {
                System.Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}
