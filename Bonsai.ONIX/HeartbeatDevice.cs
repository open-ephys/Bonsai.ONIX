using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    using HeartbeatDataFrame = U16DataFrame;

    [Description("Heartbeat device")]
    [ONIXDeviceID(ONIXDevices.ID.Heartbeat)]
    public class HeartbeatDevice : ONIFrameReader<HeartbeatDataFrame, ushort>
    {
        enum Register
        {
            ENABLE = 0,  // Enable the heartbeat
            CLK_DIV = 1,  // Heartbeat clock divider ratio. Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
        }

        public HeartbeatDevice() { }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        protected override IObservable<HeartbeatDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new HeartbeatDataFrame(f); });
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
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

        uint beat_hz = 1;
        [Category("Configuration")]
        [Description("Rate at which beats are produced.")]
        [Range(1, 10e6)]
        public uint BeatHz
        {
            get
            {
                var val = ReadRegister((int)Register.CLK_DIV);
                if (val != 0) { beat_hz = ReadRegister((int)Register.CLK_HZ) / val; }
                return beat_hz;
            }
            set
            {
                beat_hz = value;
                if (beat_hz != 0)
                {
                    WriteRegister((int)Register.CLK_DIV, ReadRegister((int)Register.CLK_HZ) / beat_hz);
                }
            }
        }
    }
}