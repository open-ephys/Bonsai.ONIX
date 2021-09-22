using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("BNO055 inertial measurement unit.")]
    public class BNO055Device : ONIFrameReader<BNO055DataFrame, ushort>
    {
        enum Register
        {
            ENABLE = 0,
            MESSAGE,
        }

        public BNO055Device() : base(ONIXDevices.ID.BNO055) { }

        protected override IObservable<BNO055DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new BNO055DataFrame(f); });
        }

        [ONIXDeviceID(ONIXDevices.ID.BNO055)]
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
