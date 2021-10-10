using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.BNO055)]
    [Description("BNO055 inertial measurement unit.")]
    public class BNO055Device : ONIFrameReader<BNO055DataFrame, ushort>
    {
        private enum Register
        {
            ENABLE = 0,
            MESSAGE,
        }

        protected override IObservable<BNO055DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            return source.Select(f => { return new BNO055DataFrame(f, frameOffset); });
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        [Category("Configuration")]
        [Description("Enable the input data stream.")]
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
