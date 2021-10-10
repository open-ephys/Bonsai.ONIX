using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.DS90UB9X)]
    [Description("Acquire image data from UCLA Miniscope V4.")]
    public class MiniscopeV4Device : ONIFrameReader<MiniscopeDataFrame, ushort>
    {
        private const int ATMegaAddress = 0x10;
        private const int TPL0102Address = 0x50;
        private const int MAX14574Address = 0x77;
        private const int Rows = 608;
        private const int Columns = 608;

        private bool running = false;

        public enum FPS
        {
            FPS10Hz,
            FPS15Hz,
            FPS20Hz,
            FPS25Hz,
            FPS30Hz,
        }

        public enum SensorGain
        {
            Low = 0x00E1,
            Medium = 0x00E4,
            High = 0x0024,
        }

        protected override IObservable<MiniscopeDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            // The LED brightnes and binary on/off state are linked in their firmware and so require
            // some hackuiness to get the behavior I want.
            running = true;

            // Turn on EWL
            using (var i2c = new I2CConfiguration(DeviceAddress, MAX14574Address))
            {
                i2c.WriteByte(0x03, 0x03);
            }

            // Turn on LED
            using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
            {
                i2c.WriteByte(1, (uint)(LEDBrightness == 0 ? 0xFF : 0x00));
            }

            return source
            .SkipWhile(f => (f.Sample[5] & 0x8000) == 0)
            .Buffer(Rows)
            .Select(block => { return new MiniscopeDataFrame(block, frameOffset, Rows, Columns); })
            .Finally(() =>
                {
                    // Turn off EWL
                    using (var i2c = new I2CConfiguration(DeviceAddress, MAX14574Address))
                    {
                        i2c.WriteByte(0x03, 0x00);
                    }

                    // Turn off LED
                    using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
                    {
                        i2c.WriteByte(1, 0xFF);
                    }

                    running = false;
                });
        }

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

        private ONIDeviceAddress deviceAddress;
        public override ONIDeviceAddress DeviceAddress
        {
            get { return deviceAddress; }
            set
            {
                deviceAddress = value;
                if (!deviceAddress.Valid) return;

                // Deserializer triggering mode
                WriteRegister((uint)DS90UB9xConfiguration.Register.TriggerOffset, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.ReadSize, Columns);
                WriteRegister((uint)DS90UB9xConfiguration.Register.Trigger, (uint)DS90UB9xConfiguration.TriggerMode.HsyncEdgePositive);
                WriteRegister((uint)DS90UB9xConfiguration.Register.IncludeSyncBits, 0);
                WriteRegister((uint)DS90UB9xConfiguration.Register.PixelGate, (uint)DS90UB9xConfiguration.PixelGate.VsyncPositive);
                WriteRegister((uint)DS90UB9xConfiguration.Register.MarkMode, (uint)DS90UB9xConfiguration.MarkMode.VsyncRising);

                // Configuration I2C aliases
                using (var i2c = new I2CConfiguration(DeviceAddress, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    uint val = 0x4 + (uint)DS90UB9xConfiguration.Mode.Raw12BitLowFrequency; // 0x4 maintains coax mode
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.PortMode, val);

                    val = ATMegaAddress << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveID1, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveAlias1, val);

                    val = TPL0102Address << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveID2, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveAlias2, val);

                    val = MAX14574Address << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveID3, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveAlias3, val);
                }

                // Set up potentiometer
                using (var i2c = new I2CConfiguration(DeviceAddress, TPL0102Address))
                {
                    i2c.WriteByte(0x00, 0x72);
                    i2c.WriteByte(0x01, 0x00);
                }

                // Turn on EWL
                using (var i2c = new I2CConfiguration(DeviceAddress, MAX14574Address))
                {
                    //i2c.WriteByte(0x03, 0x03);
                    i2c.WriteByte(0x08, 0x7F);
                    i2c.WriteByte(0x09, 0x02);
                }

                // Turn on LED and Setup Python480
                using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
                {
                    WriteCameraRegister(i2c, 16, 3); // Turn on PLL
                    WriteCameraRegister(i2c, 32, 0x7007); // Turn on clock managment
                    WriteCameraRegister(i2c, 199, 666); // Defines granularity (unit = 1/PLL clock) of exposure and reset_length
                    WriteCameraRegister(i2c, 200, 3300); // Set frame rate to 30 Hz
                    WriteCameraRegister(i2c, 201, 3000); // Set Exposure
                }

                // TODO: This is a nasty hack, to give the serializer time to lock
                // Ideally, we should use the lock status register in the latest firmware
                // And some asynchronous task to avoid locking the UI. This is difficult because
                // Properties cannot be asychronous in C#.
                System.Threading.Thread.Sleep(100);
                if (ReadRegister((uint)DS90UB9xConfiguration.Register.LinkStatus) == 0)
                {
                    throw new WorkflowBuildException("Unable to to connect to Miniscope.");
                }
            }
        }

        // Although the LED brightness control is highly nonlinear, Daniel suggests this is useful
        // since it gives a lot of adjustability where people want it
        [Range(0, 100)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Excitation LED brightness (0-100%).")]
        [Category("Acquisition")]
        public double LEDBrightness
        {
            set
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
                {
                    i2c.WriteByte(0x01, (uint)((value == 0 || !running) ? 0xFF : 0x08));
                }
                using (var i2c = new I2CConfiguration(DeviceAddress, TPL0102Address))
                {
                    i2c.WriteByte(0x01, (uint)(255 * ((100 - value) / 100.0)));
                }
            }
            get
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, TPL0102Address))
                {
                    var val = i2c.ReadByte(1) ?? 0;
                    return 100 - 100.0 * val / 255.0;
                }
            }
        }

        private FPS frameRate;
        [Category("Acquisition")]
        [Description("Camera images acquired per second.")]
        public FPS FrameRate
        {
            set
            {
                uint totalShutterWidth;

                switch (value)
                {
                    case FPS.FPS10Hz:
                        totalShutterWidth = 10000;
                        break;
                    case FPS.FPS15Hz:
                        totalShutterWidth = 6667;
                        break;
                    case FPS.FPS20Hz:
                        totalShutterWidth = 5000;
                        break;
                    case FPS.FPS25Hz:
                        totalShutterWidth = 4000;
                        break;
                    case FPS.FPS30Hz:
                    default:
                        totalShutterWidth = 3300;
                        break;
                }

                using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
                {
                    WriteCameraRegister(i2c, 200, totalShutterWidth);
                }

                frameRate = value;
            }
            get
            {
                return frameRate;
            }
        }

        private SensorGain gain = SensorGain.Low;
        [Description("Camera sensor analog gain.")]
        [Category("Acquisition")]
        public SensorGain Gain
        {
            set
            {
                gain = value;
                using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
                {
                    WriteCameraRegister(i2c, 204, (uint)value);
                }
            }
            get
            {
                return gain;
            }
        }

        private static void WriteCameraRegister(I2CConfiguration i2c, uint register, uint value)
        {
            // ATMega -> Python480 passthrough protocol
            var regLow = register & 0xFF;
            var regHigh = (register >> 8) & 0xFF;
            var valLow = value & 0xFF;
            var valHigh = (value >> 8) & 0xFF;

            i2c.WriteByte(0x05, regHigh);
            i2c.WriteByte(regLow, valHigh);
            i2c.WriteByte(valLow, 0x00);
        }

        [Range(24.4, 69.7)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Liquid lens voltage (Volts RMS).")]
        [Category("Acquisition")]
        public double LiquidLensVoltage
        {
            set
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, MAX14574Address))
                {
                    i2c.WriteByte(0x08, (uint)((value - 24.4) / 0.0445) >> 2);
                    i2c.WriteByte(0x09, 0x02); // Update output
                }
            }
            get
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, MAX14574Address))
                {
                    return 0.0445 * (i2c.ReadByte(0x08) << 2 ?? 0) + 24.4;
                }
            }
        }

        private bool interLeaveLED = false;
        [Description("Only turn on excitation LED during camera exposures.")]
        [Category("Acquisition")]
        public bool InterleaveLED
        {
            set
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, ATMegaAddress))
                {
                    i2c.WriteByte(0x04, (uint)(value ? 0x00 : 0x03));
                }

                interLeaveLED = value;
            }
            get
            {
                // Trying to read was giving me issues, so I gave up
                return interLeaveLED;
            }
        }
    }
}
