using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    [Description("Heartbeat device")]
    public class HeartbeatDevice : ONIFrameReaderDeviceBuilder<HeartbeatDataFrame>
    {

        // Control registers (see onidevices.h)
        public enum Register
        {
            NULLPARM = 0,  // No command
            CLK_DIV = 1,  // Heartbeat clock divider ratio.Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
        }

        public HeartbeatDevice() : base(oni.Device.DeviceID.HEARTBEAT) { }

        public override IObservable<HeartbeatDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndices.Contains(DeviceIndex.SelectedIndex))
                .Select(f => { return new HeartbeatDataFrame(f, DeviceIndex.SelectedIndex, 0); });
        }

        int beat_hz;
        [Range(0, 10e6)]
        public int BeatHz
        {
            get
            {
                var val = Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_DIV);
                return Controller.AcqContext.SystemClockHz / val;
            }
            set
            {
                beat_hz = value;
                Controller.WriteRegister(DeviceIndex.SelectedIndex,
                                         (int)Register.CLK_DIV,
                                         Controller.AcqContext.SystemClockHz / value);
            }
        }

    }
}