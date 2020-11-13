using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONIX
{
    [Description("Acquires digital data from an Open-Ephys FMC Breakout Board.")]
    public class BreakoutDigitalInputDevice : ONIFrameReaderDeviceBuilder<BreakoutDigitalInputDataFrame>
    {
        public enum Register
        {
            BREAKDIG1R3_NULLPARM = 0, // No command
            BREAKDIG1R3_LEDMODE = 1, //0 = All off, 1 = Power and Running only, 3 = normal, else undefined
            BREAKDIG1R3_LEDLVL = 2, // 0-255 overall LED brightness value.
        }

        public BreakoutDigitalInputDevice() : base(oni.Device.DeviceID.BREAKDIG1R3) { }

        public override IObservable<BreakoutDigitalInputDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new BreakoutDigitalInputDataFrame(f, FrameClockHz, DataClockHz); });
        }

        uint led_level;
        [Category("Configuration")]
        [Range(0, 255)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public uint LEDBrightness
        {
            get
            {
                if (Controller != null)
                {
                    return Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BREAKDIG1R3_LEDLVL);
                }
                else
                {
                    return led_level;
                }
            }
            set
            {
                if (Controller != null)
                {
                    led_level = value;
                    Controller.WriteRegister(DeviceIndex.SelectedIndex,
                                             (int)Register.BREAKDIG1R3_LEDLVL,
                                             led_level);
                }
            }
        }

        public enum LEDModes
        {
            OFF = 0,
            POWERONLY,
            ON,
            UNDEFINED
        }

        LEDModes led_mode;
        [Category("Configuration")]
        public LEDModes LEDMode
        {
            get
            {
                if (Controller != null)
                {
                    return (LEDModes)Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BREAKDIG1R3_LEDMODE);
                }
                else
                {
                    return led_mode;
                }
            }
            set
            {
                if (Controller != null)
                {
                    led_mode = value;
                    Controller.WriteRegister(DeviceIndex.SelectedIndex,
                                             (uint)Register.BREAKDIG1R3_LEDMODE,
                                             (uint)led_mode);
                }
            }
        }

    }
}
