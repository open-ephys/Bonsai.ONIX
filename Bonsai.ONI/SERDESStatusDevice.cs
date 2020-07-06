using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.ONI
{
    [Description("SERDES status device.")]
    public class SERDESStatusDevice : ONIFrameReaderDeviceBuilder<SERDESStatusDataFrame>
    {
        public SERDESStatusDevice() : base(oni.Device.DeviceID.INFO) { }

        public override IObservable<SERDESStatusDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new SERDESStatusDataFrame(f, ClockHz); });
        }

    }
}
