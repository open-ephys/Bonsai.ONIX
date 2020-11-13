using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    using HeartbeatDataFrame = DataFrame;

    [Description("Heartbeat device")]
    public class HeartbeatDevice : ONIFrameReaderDeviceBuilder<HeartbeatDataFrame>
    {
        public enum Register
        {
            NULLPARM = 0,  // No command
            CLK_DIV = 1,  // Heartbeat clock divider ratio.Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
        }

        public HeartbeatDevice() : base(oni.Device.DeviceID.HEARTBEAT) { }

        public override IObservable<HeartbeatDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new HeartbeatDataFrame(f, FrameClockHz, DataClockHz); });
        }

        uint beat_hz;
        [Category("Configuration")]
        [Description("Rate at which beats are produced.")]
        [Range(0, 10e6)]
        public uint BeatHz
        {
            get
            {
                //beat_hz = Controller?.AcqContext.SystemClockHz / Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_DIV) ?? beat_hz;
                //return beat_hz;

                if (Controller != null)
                {
                    var val = Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_DIV);
                    beat_hz = Controller.AcqContext.SystemClockHz / val;
                    return beat_hz;
                } else
                {
                    return beat_hz;
                }
            }
            set
            {
                if (Controller != null)
                {
                    beat_hz = value;
                    Controller.WriteRegister(DeviceIndex.SelectedIndex,
                                             (int)Register.CLK_DIV,
                                             Controller.AcqContext.SystemClockHz / beat_hz);
                }
            }
        }
    }
}