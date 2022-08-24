using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("ONI Test device.")]
    [ONIXDeviceID(DeviceID.Test)]
    public class TestDevice : ONIFrameReader<TestDataFrame, ushort>
    {
        private enum Register
        {
            ENABLE = 0,
            MESSAGE,
        }

        public TestDevice() { }

        protected override IObservable<TestDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            return source.Select(f => { return new TestDataFrame(f, frameOffset); });
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

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

        [Category("Acquisition")]
        [Description("16-bit word to send as frame payload.")]
        public short Message
        {
            get
            {
                return (short)ReadRegister((uint)Register.MESSAGE);
            }
            set
            {
                WriteRegister((uint)Register.MESSAGE, (uint)value);
            }
        }
    }
}
