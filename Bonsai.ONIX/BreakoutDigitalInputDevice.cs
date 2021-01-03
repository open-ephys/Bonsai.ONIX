using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Acquires digital data from an Open-Ephys FMC Breakout Board.")]
    public class BreakoutDigitalInputDevice : ONIFrameReaderDeviceBuilder<BreakoutDigitalInputDataFrame>
    {
        enum Register
        {
            BREAKDIG1R3_NULLPARM = 0, // No command
            BREAKDIG1R3_LEDMODE = 1, // 0 = All off, 1 = Power and Running only, 3 = normal, else undefined
            BREAKDIG1R3_LEDLVL = 2, // 0-255 overall LED brightness value.
        }

        public BreakoutDigitalInputDevice() : base(ONIXDevices.ID.BREAKDIG1R3) { }

        public override IObservable<BreakoutDigitalInputDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source.Select(f => { return new BreakoutDigitalInputDataFrame(f); });
        }


        [Category("Configuration")]
        [Range(0, 255)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public uint LEDBrightness
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BREAKDIG1R3_LEDLVL);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.BREAKDIG1R3_LEDLVL, value);
            }
        }

        public enum LEDModes
        {
            OFF = 0,
            POWERONLY,
            ON,
            UNDEFINED
        }

        [Category("Configuration")]
        public LEDModes LEDMode
        {
            get
            {
                return (LEDModes)ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BREAKDIG1R3_LEDMODE);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.BREAKDIG1R3_LEDMODE, (uint)value);
            }
        }
    }
}
