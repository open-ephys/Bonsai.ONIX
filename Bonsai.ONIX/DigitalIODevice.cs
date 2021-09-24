using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.BreakoutDigitalIO)]
    [Description("Acquires digital data from, and sends digital data to, an ONIX " +
        "Breakout Board and controls the indication LED state. The least significant bits of " +
        "the integer input are used to determine the output port state.")]
    public class DigitalIODevice : ONIFrameReaderAndWriter<int, BreakoutDigitalInputDataFrame, ushort>
    {
        private enum Register
        {
            ENABLE = 0, // No command
            LEDMODE = 1, // 0 = All off, 1 = Power only, 2 = Power and running, 3 = normal
            LEDLVL = 2, // 0-255 overall LED brightness value.
        }

        protected override IObservable<BreakoutDigitalInputDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new BreakoutDigitalInputDataFrame(f); });
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        protected override void Write(ONIContextTask ctx, int input)
        {
            ctx.Write((uint)DeviceAddress.Address, (uint)input);
        }

        [Category("Acquisition")]
        [Range(0, 100)]
        [Precision(0, 1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Adjust the breakout board's indication LED brightness.")]
        public double LEDBrightness
        {
            get
            {
                var level = ReadRegister((int)Register.LEDLVL);
                return 100d * (level / 15.0d);
            }
            set
            {
                var level = (uint)Math.Ceiling(15 * value / 100);
                WriteRegister((int)Register.LEDLVL, level);
            }
        }

        public enum LEDModes
        {
            Off = 0,
            PowerOnly,
            PowerAndRunning,
            On
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }
    }
}
