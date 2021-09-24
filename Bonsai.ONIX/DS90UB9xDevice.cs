using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.DS90UB9X)]
    [Description("Provides access to raw data from a DS90UB9x deserializer.")]
    public class DS90UB9xDevice : ONIFrameReader<RawDataFrame, ushort>
    {

        protected override IObservable<RawDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new RawDataFrame(f); });
        }
        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.Enable) > 0;
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.Enable, value ? (uint)1 : 0);
            }
        }

        [Category("Configuration")]
        [Description("Frame width in samples (pixels).")]
        public uint DataSize
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.ReadSize);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.ReadSize, value);
            }
        }

        [Category("Configuration")]
        [Description("Frame start trigger mode.")]
        public DS90UB9xConfiguration.TriggerMode TriggerMode
        {
            get
            {
                return (DS90UB9xConfiguration.TriggerMode)ReadRegister((uint)DS90UB9xConfiguration.Register.Trigger);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.Trigger, (uint)value);
            }
        }

        [Category("Configuration")]
        [Description("Trigger Offset. Start frame N samples after trigger.")]
        public uint TriggerOffset
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.TriggerOffset);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.TriggerOffset, value);
            }
        }

        [Category("Configuration")]
        [Description("Pixel gate. Ignore pixels when gate not active.")]
        public DS90UB9xConfiguration.PixelGate PixelGate
        {
            get
            {
                return (DS90UB9xConfiguration.PixelGate)ReadRegister((uint)DS90UB9xConfiguration.Register.PixelGate);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.PixelGate, (uint)value);
            }
        }

        [Category("Configuration")]
        [Description("Include synchronozation bits in samples.")]
        public bool SyncBits
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.IncludeSyncBits) > 0;
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.IncludeSyncBits, value ? (uint)1 : 0);
            }
        }

        [Category("Configuration")]
        [Description("Marks the first received frame accordinf to a sync line edge.\nUseful to mark first line in a picture.")]
        public DS90UB9xConfiguration.MarkMode MarkMode
        {
            get
            {
                return (DS90UB9xConfiguration.MarkMode)ReadRegister((uint)DS90UB9xConfiguration.Register.MarkMode);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.MarkMode, (uint)value);
            }
        }

        [Category("Configuration")]
        [Description("Link lock status")]
        public bool LockState
        {
            get
            {
                uint state = ReadRegister((uint)DS90UB9xConfiguration.Register.LinkStatus);
                return (state & 0x01) != 0;
            }
            private set { }
        }

        [Category("Configuration")]
        [Description("Set the deserializer mode.")]
        public DS90UB9xConfiguration.Mode DeserializerMode
        {
            get
            {
                if (!DeviceAddress.Valid) return DS90UB9xConfiguration.Mode.Raw12BitHighFrequency;
                using (var i2c = new I2CConfiguration(DeviceAddress, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    return (DS90UB9xConfiguration.Mode)(i2c.ReadByte((uint)DS90UB9xConfiguration.I2CRegister.PortMode) & 0x3);
                }
            }
            set
            {
                if (!DeviceAddress.Valid) return;
                using (var i2c = new I2CConfiguration(DeviceAddress, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    uint val = 0x4 + (uint)value; // 0x4 maintains coax mode
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.PortMode, val);
                }
            }
        }
    }
}
