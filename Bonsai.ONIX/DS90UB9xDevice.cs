using System;
using System.Collections;
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
        [Description("The bits that will be checked for magic word.")]
        public bool[] MagicMask
        {
            get
            {
                var val = ReadRegister((uint)DS90UB9xConfiguration.Register.MagicMask);
                return new BitArray(new int[] { (int)val }).Cast<bool>().ToArray();
            }
            set
            {
                var array = new int[1];
                new BitArray(value).CopyTo(array, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicMask, (uint)array[0]);
            }
        }

        [Category("Configuration")]
        [Description("If MagicMask is non-zero, the magic word that must appear on the stream to start frame.")]
        public uint MagicWord
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.Magic);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.Magic, value);
            }
        }

        [Category("Configuration")]
        [Description("Max number of samples to wait from trigger to mask detection before cancelling and going " +
            "back to trigger detection. 0 means wait indefinitely.")]
        public uint MagicWait
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.MagicWait);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.MagicWait, value);
            }
        }

        [Category("Configuration")]
        [Description("Bit 0: '0' = Normal parallel mode. '1' = Serial mode, Bit 1: '1' = Include `index` field " +
            "in normal mode, '0'= Do not include it in normal mode. Bit 2: Number of serial streams '0' = 1 stream, " +
            "'1' = 2 streams., Bit 3: reserved. Bits 7-4: Number of bits per word - 1 (i.e.: '0x0' = 1bit, '0xF' = 16bits. " +
            "Bits 9-8 number of lines per stream '00' = 1, '01' = 2. '10' = 4, '11' = 8. Bit 10: data order in " +
            "serial mode '0' = MSB first, '1' = LSB first .")]
        public bool[] DataMode
        {
            get
            {
                var val = ReadRegister((uint)DS90UB9xConfiguration.Register.DataMode);
                return new BitArray(new int[] { (int)val }).Cast<bool>().ToArray();
            }
            set
            {
                var array = new int[1];
                new BitArray(value).CopyTo(array, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.DataMode, (uint)array[0]);
            }
        }

        [Category("Configuration")]
        [Description("TODO")]
        public int[] DataLines0
        {
            get
            {
                var val = ReadRegister((uint)DS90UB9xConfiguration.Register.DataLines0);
                var lineMap = new int[8];

                for (var i = 0; i < lineMap.Length; i++)
                {
                    lineMap[i] = ((int)val >> i * 4) & 0xF;
                }

                return lineMap;
            }
            set
            {
                int val = 0;
                for (var i = 0; i < value.Length; i++)
                {
                    val |= value[i] << i * 4;
                }

                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines0, (uint)val);
            }
        }

        [Category("Configuration")]
        [Description("TODO")]
        public int[] DataLines1
        {
            get
            {
                var val = ReadRegister((uint)DS90UB9xConfiguration.Register.DataLines1);
                var lineMap = new int[8];

                for (var i = 0; i < lineMap.Length; i++)
                {
                    lineMap[i] = ((int)val >> i * 4) & 0xF;
                }

                return lineMap;
            }
            set
            {
                int val = 0;
                for (var i = 0; i < value.Length; i++)
                {
                    val |= value[i] << i * 4;
                }

                WriteRegister((uint)DS90UB9xConfiguration.Register.DataLines1, (uint)val);
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
                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    return (DS90UB9xConfiguration.Mode)(i2c.ReadByte((uint)DS90UB9xConfiguration.DesI2CRegister.PortMode) & 0x3);
                }
            }
            set
            {
                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    uint val = 0x4 + (uint)value; // 0x4 maintains coax mode
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.PortMode, val);
                }
            }
        }
    }
}
