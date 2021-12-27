using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace Bonsai.ONIX
{
    [DefaultProperty("DeviceAddress")]
    public abstract class ONIDevice
    {
        [Description("The ONIX device type/ID.")]
        internal DeviceID ID { get; set; } = DeviceID.Null;

        [Category("ONI Configuration")]
        [Description("The full device hardware address consisting of a hardware slot and device table index.")]
        [TypeConverter(typeof(ONIDeviceAddressTypeConverter))]
        [Editor("Bonsai.ONIX.Design.DocumentationLink, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        public abstract ONIDeviceAddress DeviceAddress { get; set; }

        [Category("ONI Configuration")]
        [Description("The hub that this device belongs to.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Externalizable(false)]
        public oni.Hub Hub
        {
            get
            {
                ONIXDeviceDescriptor.IsValid(ID, DeviceAddress);

                using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                {
                    return c.Context.GetHub((uint)DeviceAddress.Address);
                }
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
                // NB: Frame clock only requires a valid hardware slot
                if (!string.IsNullOrEmpty(DeviceAddress.HardwareSlot.Driver))
                {
                    using (var c = ONIContextManager.ReserveContext(DeviceAddress.HardwareSlot))
                    {
                        return c.Context.AcquisitionClockHz;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        protected ulong FrameClockOffset
        {
            get { return (ulong)Math.Round((Hub == null ? 0 : Hub.DelayNanoSeconds) * 1E-9 * (FrameClockHz ?? 0)); }
        }

        public ONIDevice()
        {
            ID = !(GetType().GetCustomAttributes(typeof(ONIXDeviceIDAttribute), true).FirstOrDefault() is ONIXDeviceIDAttribute devID) ?
                DeviceID.Null : devID.deviceID;
            //DeviceAddress = new ONIDeviceAddress();
        }

        protected uint ReadRegister(uint address, bool silent = true)
        {
            // NB: This is a redundant check but is here even throwing and catching within the
            // function body results in a huge UI performance hit.
            if (silent && !ONIXDeviceDescriptor.IsValid(ID, DeviceAddress))
            {
                Console.Error.WriteLine("Register read was attempted with an invalid device " +
                    "descriptor. Device ID: " + ID + ", Address: " + DeviceAddress.ToString() + ".");
                return 0;
            }

            try
            {
                return ReadRegister(new ONIXDeviceDescriptor(ID, DeviceAddress), address);
            }
            catch (Exception ex) when (silent && (ex is ArgumentException || ex is oni.ONIException))
            {
                System.Console.Error.WriteLine(ex.Message);
                return 0;
            }
        }

        protected void WriteRegister(uint address, uint value, bool silent = true)
        {
            // NB: This is a redundant check but is here even throwing and catching within the
            // function body results in a huge UI performance hit.
            if (silent && !ONIXDeviceDescriptor.IsValid(ID, DeviceAddress))
            {
                Console.Error.WriteLine("Register write was attempted with an invalid device " +
                    "descriptor. Device ID: " + ID + ", Address: " + DeviceAddress.ToString() + ".");
            }

            try
            {
                WriteRegister(new ONIXDeviceDescriptor(ID, DeviceAddress), address, value);
            }
            catch (Exception ex) when (silent && (ex is ArgumentException || ex is oni.ONIException))
            {
                System.Console.Error.WriteLine(ex.Message);
            }
        }

        static public uint ReadRegister(ONIXDeviceDescriptor descriptor, uint address)
        {
            using (var c = ONIContextManager.ReserveContext(descriptor.Address.HardwareSlot))
            {
                return c.Context.ReadRegister((uint)descriptor.Address.Address, address);
            }
        }

        static public void WriteRegister(ONIXDeviceDescriptor descriptor, uint address, uint value)
        {
            using (var c = ONIContextManager.ReserveContext(descriptor.Address.HardwareSlot))
            {
                c.Context.WriteRegister((uint)descriptor.Address.Address, address, value);
            }
        }
    }
}
