using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Acquires digital data from an Open-Ephys FMC Breakout Board.")]
    public class FMCDigitalIODevice : ONIFrameReaderAndWriter<int, BreakoutDigitalInputDataFrame, ushort>
    {
        enum Register
        {
            ENABLE = 0, // No command
            LEDMODE = 1, // 0 = All off, 1 = Power only, 2 = Power and running, 3 = normal
            LEDLVL = 2, // 0-255 overall LED brightness value.
        }

        public FMCDigitalIODevice() : base(ONIXDevices.ID.BreakoutDigitalIO) { }

        protected override IObservable<BreakoutDigitalInputDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new BreakoutDigitalInputDataFrame(f); });
        }

        protected override void Write(ONIContextTask ctx, int input)
        {
            ctx.Write(DeviceAddress.Address, (uint)input);
        }

        [Category("Acquisition")]
        [Range(0, 15)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public uint LEDBrightness
        {
            get
            {
                return ReadRegister(DeviceAddress.Address, (int)Register.LEDLVL);
            }
            set
            {
                WriteRegister(DeviceAddress.Address, (int)Register.LEDLVL, value);
            }
        }

        public enum LEDModes
        {
            OFF = 0,
            POWERONLY,
            POWERANDRUNNING,
            ON
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister(DeviceAddress.Address, (uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister(DeviceAddress.Address, (uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        [Category("Acquisition")]
        public LEDModes LEDMode
        {
            get
            {
                return (LEDModes)ReadRegister(DeviceAddress.Address, (int)Register.LEDMODE);
            }
            set
            {
                WriteRegister(DeviceAddress.Address, (uint)Register.LEDMODE, (uint)value);
            }
        }
    }
}
