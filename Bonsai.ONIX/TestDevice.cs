using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("ONI Test device.")]
    public class TestDevice : ONIFrameReader<TestDataFrame, ushort>
    {
        enum Register
        {
            ENABLE = 0,
            MESSAGE,
        }

        public TestDevice() : base(ONIXDevices.ID.TEST) { }

        protected override IObservable<TestDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new TestDataFrame(f); });
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
        [Description("16-bit word to send as frame payload.")]
        public short Message
        {
            get
            {
                return (short)ReadRegister(DeviceAddress.Address, (uint)Register.MESSAGE);
            }
            set
            {
                WriteRegister(DeviceAddress.Address, (uint)Register.MESSAGE, (uint)value);
            }
        }
    }
}
