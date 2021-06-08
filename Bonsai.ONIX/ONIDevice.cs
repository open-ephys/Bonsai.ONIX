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
        public ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

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

        public ONIDevice(ONIXDevices.ID deviceID, ONIDeviceAddress deviceAddress)
        {
            ID = deviceID;
            DeviceAddress = deviceAddress;
        }

        // NB: Write/ReadRegister are used extensively in node property settings.
        // This means they cannot throw or there will be a variety of consequences
        // such as errors loading XML descriptions of workflows and terrible performance
        // if a valid device is not yet selected. As an intermediate solution,
        // I'm just dumping errors to the Console and returning 0 or doing nothing.
        protected uint ReadRegister(uint dev_index, uint register_address)
        {
            if (!DeviceAddress.Valid) return 0;

            try
            {
                using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                {
                    return c.Context.ReadRegister(dev_index, register_address);
                }
            }
            catch (oni.ONIException ex)
            {
                System.Console.WriteLine(ex.Message);
                return 0;
            }
        }

        protected void WriteRegister(uint dev_index, uint register_address, uint value)
        {
            if (!DeviceAddress.Valid) return;

            try
            {
                using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                {
                    c.Context.WriteRegister(dev_index, register_address, value);
                }
            }
            catch (oni.ONIException ex)
            {
                System.Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
