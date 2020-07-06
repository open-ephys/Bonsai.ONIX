using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.ONI
{
    [Description("Acquires data from a single TS4231 light to digital converter chip.")]
    public class LightHouseDevice : ONIFrameReaderDeviceBuilder<LightHouseDataFrame>
    {
        public LightHouseDevice() : base(oni.Device.DeviceID.TS4231) { }

        public override IObservable<LightHouseDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new LightHouseDataFrame(f, ClockHz); });
        }
    }
}
