using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace Bonsai.ONIX
{
    [DefaultProperty("DeviceAddress")]
    public abstract class ONIDevice
    {
        [Description("The device type/ID.")]
        internal ONIXDevices.ID ID { get; set; } = ONIXDevices.ID.Null;

        private ONIDeviceAddress deviceAddress;
        [Category("ONI Configuration")]
        [Description("The full device hardware address consisting of a hardware slot and device table index.")]
        [TypeConverter(typeof(ONIDeviceAddressTypeConverter))]
        [Editor("Bonsai.ONIX.Design.DocumentationLink, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        public abstract ONIDeviceAddress DeviceAddress { get; set; }

        //Keep a local copy until address changes to avoid accessing the register interface a million times for already-known information
        private oni.Hub deviceHub = null;
        private ONIDeviceAddress oldAddressHub = null;
        private ONIDeviceAddress oldAddressHz = null;
        private double? contextHz = null;

        [Category("ONI Configuration")]
        [Description("The hub that this device belongs to.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Externalizable(false)]
        public oni.Hub Hub
        {
            get
            {
                if (DeviceAddress.Valid)
                {
                    if (deviceHub == null || oldAddressHub == null || oldAddressHub != DeviceAddress)
                    {
                        using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                        {
                            deviceHub = c.Context.GetHub((uint)DeviceAddress.Address);
                        }
                        oldAddressHub = DeviceAddress;
                    }
                    return deviceHub;
                }
                else return null;
            }
        }

        [Category("ONI Configuration")]
        [Description("The rate of the host frame clock counter (Hz).")]
        [Externalizable(false)]
        [ReadOnly(true)]
        public double? FrameClockHz
        {
            get
            {
                if (DeviceAddress.Valid)
                {
                    if (contextHz == null || oldAddressHz == null || oldAddressHz != DeviceAddress)
                    {
                        using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                        {
                            contextHz = c.Context.AcquisitionClockHz;
                        }
                        oldAddressHz = DeviceAddress;
                    }
                    return contextHz;
                }
                else return null;
            }
        }

        public ONIDevice()
        {
            var devID = this.GetType().GetCustomAttributes(typeof(ONIXDeviceIDAttribute), true).FirstOrDefault() as ONIXDeviceIDAttribute;
            ID = devID == null ? ONIXDevices.ID.Null : devID.deviceID;
            deviceAddress = new ONIDeviceAddress();
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

        protected virtual void OnDeviceAddressUpdate() { }
    }
}
