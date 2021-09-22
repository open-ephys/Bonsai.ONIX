using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    public class UCLAMiniscopeV3Device : ONIFrameReader<UCLAMiniscopeV3DataFrame, ushort>
    {

        [Description("Acquire data from UCLA Miniscope V3.")]
        public UCLAMiniscopeV3Device() : base(ONIXDevices.ID.DS90UB9X) { }

        protected override IObservable<UCLAMiniscopeV3DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source
                .SkipWhile(f => (f.Sample[5] & 0x8000) == 0)
                .Buffer(UCLAMiniscopeV3DataFrame.NumRows)
                .Select(block => { return new UCLAMiniscopeV3DataFrame(block); });
        }

        private ONIDeviceAddress deviceAddress;
        [ONIXDeviceID(ONIXDevices.ID.DS90UB9X)]
        public override ONIDeviceAddress DeviceAddress
        {
            get { return deviceAddress; }
            set { deviceAddress = value; OnDeviceAddressUpdate(); }
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        private const int LEDDriverAddress = 0x4C;
        private const int CameraSensorAddress = 0x5C;

        protected override void OnDeviceAddressUpdate()
        {
            // Deserializer triggering mode
            if (!DeviceAddress.Valid) return;
            WriteRegister((uint)DS90UB9xConfiguration.Register.TRIGGER_OFFSET, 0);
            WriteRegister((uint)DS90UB9xConfiguration.Register.READSZ, 752);
            WriteRegister((uint)DS90UB9xConfiguration.Register.TRIGGER, (uint)DS90UB9xConfiguration.TriggerMode.HSYNC_EDGE_POSITIVE);
            WriteRegister((uint)DS90UB9xConfiguration.Register.SYNC_BITS, 0);
            WriteRegister((uint)DS90UB9xConfiguration.Register.PIXEL_GATE, (uint)DS90UB9xConfiguration.PixelGate.VSYNC_POSITIVE);
            WriteRegister((uint)DS90UB9xConfiguration.Register.MARK_MODE, (uint)DS90UB9xConfiguration.MarkMode.VSYNC_RISING);

            // Configuration I2C aliases
            using (var i2c = new I2CConfiguration(DeviceAddress, DS90UB9xConfiguration.DeserializerDefaultAddress))
            {
                uint val = 0x4 + (uint)DS90UB9xConfiguration.DeserializerModes.RAW12BITLF; // 0x4 maintains coax mode
                i2c.WriteByte((uint)DS90UB9xConfiguration.DeserializerRegister.PORT_MODE, val);

                //This is a very nasty hack, to give the serializer time to lock
                //Ideally we should use the lock status register in the latest firmware
                //And some asynchronous task to avoid locking the UI
                System.Threading.Thread.Sleep(100);
                val = CameraSensorAddress << 1;
                i2c.WriteByte(0x5E, val); // 0x5E: SLAVE_ID1. This address should go to DS90UB9xConfiguration.DeserializerRegister
                i2c.WriteByte(0x66, val); // 0x66 SLAVE_ALIAS1. Ditto

                val = LEDDriverAddress << 1;
                i2c.WriteByte(0x5F, val);  // 0x5E: SLAVE_ID2. This address should go to DS90UB9xConfiguration.DeserializerRegister
                i2c.WriteByte(0x67, val);  // 0x5E: SLAVE_ALIAS1. Ditto
            }

            // Camera sensor configuration
            using (var i2c = new I2CConfiguration(DeviceAddress, CameraSensorAddress))
            {
                // Turn off automatic gain and exposure control
                WriteCameraRegister(i2c, (uint)SensorAddress.AECAndAGC, 0);
            }
        }

        double dacValue = 0;
        [Range(0, 100)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("LED brightness (0-100 %).")]
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
        [Description("Camera sensor analog gain (0-100 %).")]
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
        [Description("Frames per second.")]
        public FrameRate FPS
        {
            set
            {
                uint hBlank = 0;
                uint vBlank = 0;
                uint totalShutterWidth = 0;

                switch (value)
                {
                    case FrameRate.FPS5Hz:
                        hBlank = 993;
                        vBlank = 2500;
                        totalShutterWidth = 2970;
                        break;
                    case FrameRate.FPS10Hz:
                        hBlank = 750;
                        vBlank = 1250;
                        totalShutterWidth = 1720;
                        break;
                    case FrameRate.FPS15Hz:
                        hBlank = 657;
                        vBlank = 750;
                        totalShutterWidth = 1220;
                        break;
                    case FrameRate.FPS30Hz:
                        hBlank = 94;
                        vBlank = 545;
                        totalShutterWidth = 1000;
                        break;
                    case FrameRate.FPS60Hz:
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
                            return FrameRate.FPS5Hz;
                        case 750:
                            return FrameRate.FPS10Hz;
                        case 657:
                            return FrameRate.FPS15Hz;
                        case 94:
                            return FrameRate.FPS30Hz;
                        case 93:
                            return FrameRate.FPS60Hz;
                        default:
                            throw new WorkflowRuntimeException("Error retreiving frame rate from camera.");
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

        public enum FrameRate
        {
            FPS5Hz,
            FPS10Hz,
            FPS15Hz,
            FPS30Hz,
            FPS60Hz
        }

        static int ReadCameraRegister(I2CConfiguration i2c, uint reg)
        {
            return (int)(i2c.ReadByte(reg) << 8 | i2c.ReadByte(0xF0));
        }
        static void WriteCameraRegister(I2CConfiguration i2c, uint reg, uint value)
        {
            uint byte0 = value & 0xFF;
            uint byte1 = (value >> 8) & 0xFF;
            i2c.WriteByte(reg, byte1);
            i2c.WriteByte(0xF0, byte0);
        }

        static void WriteLEDDriver(I2CConfiguration i2c, uint value)
        {
            uint byte0 = (0xF0 & value) >> 4;
            uint byte1 = (0x0F & value) << 4;
            i2c.WriteByte(byte0, byte1);
        }
    }
}
