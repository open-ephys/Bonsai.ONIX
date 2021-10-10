using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
// using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.DS90UB9X)]
    [Description("Acquire image data from UCLA Miniscope V3.")]
    public class MiniscopeV3Device : ONIFrameReader<MiniscopeDataFrame, ushort>
    {

        private const int LEDDriverAddress = 0x4C;
        private const int CameraSensorAddress = 0x5C;
        private const int Rows = 480;
        private const int Columns = 752;

        public enum FPS
        {
            FPS5Hz,
            FPS10Hz,
            FPS15Hz,
            FPS20Hz,
            FPS30Hz,
            FPS60Hz
        }

        protected override IObservable<MiniscopeDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source
                .SkipWhile(f => (f.Sample[5] & 0x8000) == 0)
                .Buffer(Rows)
                .Select(block => { return new MiniscopeDataFrame(block, Rows, Columns); });
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

                    // TODO: This is a nasty hack, to give the serializer time to lock
                    // Ideally, we should use the lock status register in the latest firmware
                    // And some asynchronous task to avoid locking the UI. This is difficult because
                    // Properties cannot be asychronous in C#. See CheckLinkAsync() below.
                    System.Threading.Thread.Sleep(100);
                    if (ReadRegister((uint)DS90UB9xConfiguration.Register.LinkStatus) == 0)
                    {
                        throw new WorkflowBuildException("Unable to to connect to Miniscope.");
                    }

                    val = CameraSensorAddress << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveID1, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveAlias1, val);

                    val = LEDDriverAddress << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveID2, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.I2CRegister.SlaveAlias2, val);
                }

                // Camera sensor configuration
                using (var i2c = new I2CConfiguration(DeviceAddress, CameraSensorAddress))
                {
                    // Turn off automatic gain and exposure control
                    WriteCameraRegister(i2c, (uint)SensorAddress.AECAndAGC, 0);
                }
            }
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

        private double dacValue = 0; // NB: This DAC has no read capabilities
        [Range(0, 100)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Excitation LED brightness (0-100%).")]
        [Category("Acquisition")]
        public double LEDBrightness
        {
            set
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, LEDDriverAddress))
                {
                    dacValue = value;
                    WriteLEDDriver(i2c, (uint)(255 * (value / 100.0)));
                }
            }
            get
            {
                return dacValue;
            }
        }

        [Range(0, 100)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Camera sensor analog gain (0-100%).")]
        [Category("Acquisition")]
        public double Gain
        {
            set
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, CameraSensorAddress))
                {
                    WriteCameraRegister(i2c, (uint)SensorAddress.AnalogGain, (uint)(value * 48 / 100 + 16));
                }
            }
            get
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, CameraSensorAddress))
                {
                    var reg = ReadCameraRegister(i2c, (uint)SensorAddress.AnalogGain);
                    return (reg - 16) * 100 / 48;
                }
            }
        }

        [Category("Acquisition")]
        [Description("Camera images acquired per second.")]
        public FPS FrameRate
        {
            set
            {
                uint hBlank;
                uint vBlank;
                uint totalShutterWidth;

                switch (value)
                {
                    case FPS.FPS5Hz:
                        hBlank = 993;
                        vBlank = 2500;
                        totalShutterWidth = 2970;
                        break;
                    case FPS.FPS10Hz:
                        hBlank = 750;
                        vBlank = 1250;
                        totalShutterWidth = 1720;
                        break;
                    case FPS.FPS15Hz:
                        hBlank = 657;
                        vBlank = 750;
                        totalShutterWidth = 1220;
                        break;
                    case FPS.FPS20Hz:
                        hBlank = 870;
                        vBlank = 400;
                        totalShutterWidth = 2970;
                        break;
                    case FPS.FPS30Hz:
                    default:
                        hBlank = 94;
                        vBlank = 545;
                        totalShutterWidth = 1000;
                        break;
                    case FPS.FPS60Hz:
                        hBlank = 93;
                        vBlank = 33;
                        totalShutterWidth = 500;
                        break;
                }

                using (var i2c = new I2CConfiguration(DeviceAddress, CameraSensorAddress))
                {
                    WriteCameraRegister(i2c, (uint)SensorAddress.VerticalBlanking, vBlank);
                    WriteCameraRegister(i2c, (uint)SensorAddress.HorizontalBlanking, hBlank);
                    WriteCameraRegister(i2c, (uint)SensorAddress.TotalShutterWidth, totalShutterWidth);
                }
            }

            get
            {
                using (var i2c = new I2CConfiguration(DeviceAddress, CameraSensorAddress))
                {
                    var reg = ReadCameraRegister(i2c, (uint)SensorAddress.HorizontalBlanking);

                    switch (reg)
                    {
                        case 993:
                            return FPS.FPS5Hz;
                        case 750:
                            return FPS.FPS10Hz;
                        case 657:
                            return FPS.FPS15Hz;
                        case 94:
                            return FPS.FPS30Hz;
                        case 93:
                            return FPS.FPS60Hz;
                        default:
                            throw new WorkflowRuntimeException("Error retrieving frame rate from camera.");
                    }
                }
            }
        }

        private enum SensorAddress
        {
            HorizontalBlanking = 0x05,
            VerticalBlanking = 0x06,
            TotalShutterWidth = 0x0B,
            AnalogGain = 0x35,
            AECAndAGC = 0xAF,
        }

        private static int ReadCameraRegister(I2CConfiguration i2c, uint reg)
        {
            return (int)(i2c.ReadByte(reg) << 8 | i2c.ReadByte(0xF0));
        }

        private static void WriteCameraRegister(I2CConfiguration i2c, uint reg, uint value)
        {
            uint byte0 = value & 0xFF;
            uint byte1 = (value >> 8) & 0xFF;
            i2c.WriteByte(reg, byte1);
            i2c.WriteByte(0xF0, byte0);
        }

        private static void WriteLEDDriver(I2CConfiguration i2c, uint value)
        {
            uint byte0 = (0xF0 & value) >> 4;
            uint byte1 = (0x0F & value) << 4;
            i2c.WriteByte(byte0, byte1);
        }

        // See TODO above.
        //private async Task<bool> CheckLinkAsync()
        //{
        //    var check = Task.Run(() =>
        //    {
        //        while (ReadRegister((uint)DS90UB9xConfiguration.Register.LinkStatus) != 0x1)
        //        {
        //            System.Threading.Thread.Sleep(100);
        //        }
        //    });

        //    return await Task.WhenAny(check, Task.Delay(1000)) == check;
        //}
    }
}
