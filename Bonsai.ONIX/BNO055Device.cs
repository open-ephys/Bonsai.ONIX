using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("BNO055 inertial measurement unit.")]
    public class BNO055Device : ONIFrameReaderDeviceBuilder<BNO055DataFrame>
    {
        public BNO055Device() : base(ONIXDevices.ID.BNO055) { }

        public override IObservable<BNO055DataFrame> Process(IObservable<oni.Frame> source)
        {
            return source.Select(f => { return new BNO055DataFrame(f); });
        }
    }
}
