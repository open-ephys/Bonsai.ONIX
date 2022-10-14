using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{
    using IO = DS90UB9xConfiguration.Direction;

    [ONIXDeviceID(DeviceID.DS90UB9X)]
    [Description("Provides access to raw data from a DS90UB9x deserializer.")]
    public class DS90UB9xDevice : ONIFrameReader<RawDataFrame, ushort>
    {

        protected override IObservable<RawDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            return source.Select(f => { return new RawDataFrame(f, frameOffset); });
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
        [Description("GPIO direction.")]
        public IO[] GPIODirection
        {
            get
            {
                var val = ReadRegister((uint)DS90UB9xConfiguration.Register.GPIODirection);
                return new IO [] { (val & 0x01) > 0 ? IO.Input : IO.Output,
                                   (val & 0x02) > 0 ? IO.Input : IO.Output,
                                   (val & 0x04) > 0 ? IO.Input : IO.Output,
                                   (val & 0x08) > 0 ? IO.Input : IO.Output};
            }
            set
            {
                uint val = 0;
                val = value[0] == IO.Input ? val | 1 << 0 : val;
                val = value[1] == IO.Input ? val | 1 << 1 : val;
                val = value[2] == IO.Input ? val | 1 << 2 : val;
                val = value[3] == IO.Input ? val | 1 << 3 : val;
                WriteRegister((uint)DS90UB9xConfiguration.Register.GPIODirection, val);
            }
        }

        [Category("Configuration")]
        [Description("GPIO value.")]
        public bool[] GPIOValue
        {
            get
            {
                var val = ReadRegister((uint)DS90UB9xConfiguration.Register.GPIOValue);
                return new bool[] { ((val >> 0) & 1) == 1,
                                    ((val >> 1) & 1) == 1,
                                    ((val >> 2) & 1) == 1,
                                    ((val >> 3) & 1) == 1};
            }
            set
            {
                uint val = 0;
                val = value[0] ? val | 1 << 0 : val;
                val = value[1] ? val | 1 << 1 : val;
                val = value[2] ? val | 1 << 2 : val;
                val = value[3] ? val | 1 << 3 : val;
                WriteRegister((uint)DS90UB9xConfiguration.Register.GPIOValue, val);
            }
        }

        [XmlIgnore]
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
                using (var i2c = new I2CConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    return (DS90UB9xConfiguration.Mode)(i2c.ReadByte((uint)DS90UB9xConfiguration.I2CRegister.PortMode) & 0x3);
                }
            }
            set
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    uint val = 0x4 + (uint)value; // 0x4 maintains coax mode
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.PortMode, val);
                }
            }
        }
    }
}
