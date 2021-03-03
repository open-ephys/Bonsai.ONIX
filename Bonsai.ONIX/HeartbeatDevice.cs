using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    using HeartbeatDataFrame = DataFrame;

    [Description("Heartbeat device")]
    public class HeartbeatDevice : ONIFrameReader<HeartbeatDataFrame>
    {
        enum Register
        {
            ENABLE = 0,  // Enable the heartbeat
            CLK_DIV = 1,  // Heartbeat clock divider ratio. Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
        }

        public HeartbeatDevice() : base(ONIXDevices.ID.HEARTBEAT) { }

        protected override IObservable<HeartbeatDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source.Select(f => { return new HeartbeatDataFrame(f); });
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool Enable
        {
            get
            {
                return ReadRegister(DeviceAddress.Address, (uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister(DeviceAddress.Address, (uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        uint beat_hz;
        [Category("Configuration")]
        [Description("Rate at which beats are produced.")]
        [Range(0, 10e6)]
        public uint BeatHz
        {
            get
            {

                var val = ReadRegister(DeviceAddress.Address, (int)Register.CLK_DIV);
                beat_hz = ReadRegister(DeviceAddress.Address, (int)Register.CLK_HZ) / val;
                return beat_hz;
            }
            set
            {
                beat_hz = value;
                WriteRegister(DeviceAddress.Address,
                                            (int)Register.CLK_DIV,
                                            ReadRegister(DeviceAddress.Address, (int)Register.CLK_HZ) / beat_hz);
            }
        }
    }
}