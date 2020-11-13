using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("BNO055 inertial measurement unit.")]
    public class BNO055Device : ONIFrameReaderDeviceBuilder<BNO055DataFrame>
    {
        //enum Register
        //{
        //      TODO
        //}
        //}

        public BNO055Device() : base(oni.Device.DeviceID.BNO055) { }

        public override IObservable<BNO055DataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new BNO055DataFrame(f, FrameClockHz, DataClockHz); });
        }
    }
}
