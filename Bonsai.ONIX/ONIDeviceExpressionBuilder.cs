using Bonsai.Expressions;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONIX
{
    public abstract class ONIDeviceExpressionBuilder : ExpressionBuilder
    {
        public readonly ONIXDevices.ID ID = ONIXDevices.ID.NULL;

        [TypeConverter(typeof(ONIHardwareSlotTypeConverter))]
        [Category("ONI Configuration")]
        [Description("ONI driver and hardware index tuple.")]
        public ONIHardwareSlot HardwareSlot { get; set; } = new ONIHardwareSlot();

        [Category("ONI Configuration")]
        [Editor("Bonsai.ONIX.Design.DeviceIndexCollectionEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Description("The fully specified device index within the device table.")]
        public DeviceIndexSelection DeviceIndex { get; set; }

        [Category("ONI Metadata")]
        [Description("The rate of the host frame clock counter (Hz).")]
        [System.Xml.Serialization.XmlIgnore]
        public double? FrameClockHz { get; protected set; }

        [Category("ONI Metadata")]
        [Description("The rate of the hub-specific data clock counter (Hz).")]
        [System.Xml.Serialization.XmlIgnore]
        public double? DataClockHz { get; protected set; }

        public ONIDeviceExpressionBuilder(ONIXDevices.ID dev_id)
        {
            DeviceIndex = new DeviceIndexSelection();
            ID = dev_id;
        }

        protected uint ReadRegister(uint? dev_index, uint register_address)
        {
            using (var c = ONIContextManager.ReserveContext(HardwareSlot))
            {
                return c.Context.ReadRegister(dev_index, register_address);
            }
        }

        protected void WriteRegister(uint? dev_index, uint register_address, uint value)
        {
            using (var c = ONIContextManager.ReserveContext(HardwareSlot))
            {
                c.Context.WriteRegister(dev_index, register_address, value);
            }
        }
    }
}
