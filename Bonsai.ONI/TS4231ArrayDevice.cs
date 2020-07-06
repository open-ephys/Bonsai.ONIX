using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    [Description("Triad TS4231 optical to digital converter array for V2 SteamVR base stations.")]
    public class TS4231ArrayDevice : ONIFrameReaderDeviceBuilder<TS4231DataFrame>
    {
        public TS4231ArrayDevice() : base(oni.Device.DeviceID.TS4231V2ARR) { }
        public override IObservable<TS4231DataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new TS4231DataFrame(f, ClockHz); });
        }
    }
}