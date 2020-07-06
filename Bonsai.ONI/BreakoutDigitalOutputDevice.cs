using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    [Description("Sends 8-bit digital data to an Open-Ephys FMC breakout board Rev. 1.3.")]
    public class BreakoutDigitalOutputDevice : ONIFrameWriterDeviceBuilder<byte>
    {
        public BreakoutDigitalOutputDevice() : base(oni.Device.DeviceID.BREAKDIG1R3) { }

        public override IObservable<byte> Process(IObservable<byte> source)
        {
            return source.Do(x => {
                Controller.SelectedController.AcqContext.Write((uint)DeviceIndex.SelectedIndex, (uint)x);
            });
        }
    }
}
