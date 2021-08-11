using System;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("Controls the high performance output clock that is synchronized to the system " +
        "clock on the Open Ephys FMC Host. A boolean input can be used to toggle the Enable " +
        "register.")]
    public class ClockOutputDevice : ONISink<bool>
    {
        enum Register
        {
            NULLPARAM = 0,
            ENABLE = 1,
            HIGH_CYCLES = 2,
            LOW_CYCLES = 3,
            DELAY_CYCLES = 4,
            GATE_WITH_RUNNING = 5,
            BASE_CLOCK_HZ = 6
        }

        Tuple<uint, uint> GetHL(double frequency, double duty)
        {
            var l = BaseClockHz / frequency * (1 - duty / 100);
            var h = (BaseClockHz / frequency) - l;
            return new Tuple<uint, uint>((uint)h, (uint)l);
        }

        double GetFreq(double h, double l)
        {
            return BaseClockHz / (h + l);
        }

        double GetDuty(double h, double l)
        {
            return 100.0 * h / (h + l);
        }

        public ClockOutputDevice() : base(ONIXDevices.ID.FMCClockOutput) { }

        [Category("Acquisition")]
        [Description("Enable.")]
        public bool ClockEnabled
        {
            get
            {
                return ReadRegister((int)Register.ENABLE) != 0;
            }
            set
            {
                WriteRegister((int)Register.ENABLE, (uint)(value ? 1 : 0));
            }
        }

        uint BaseClockHz
        {
            get
            {
                return ReadRegister((int)Register.BASE_CLOCK_HZ);
            }
        }

        double frequency_hz = 1e6;
        [Category("Acquisition")]
        [Description("Output Clock frequency (Hz).")]
        [Range(0, 100e6)]
        public double Frequency
        {
            get
            {
                var h = ReadRegister((uint)Register.HIGH_CYCLES);
                var l = ReadRegister((uint)Register.LOW_CYCLES);
                frequency_hz = GetFreq(h, l);
                return frequency_hz;
            }
            set
            {
                frequency_hz = value;
                var hl = GetHL(frequency_hz, duty);
                WriteRegister((uint)Register.HIGH_CYCLES, hl.Item1);
                WriteRegister((uint)Register.LOW_CYCLES, hl.Item2);
            }
        }

        double duty = 50.0;
        [Category("Acquisition")]
        [Description("Duty Cycle (%).")]
        [Range(1, 99)]
        public double DutyCycle
        {
            get
            {
                var h = ReadRegister((uint)Register.HIGH_CYCLES);
                var l = ReadRegister((uint)Register.LOW_CYCLES);
                h = h == 0 ? 1 : h; // the firmware does this as well
                l = l == 0 ? 1 : l; // the firmware does this as well
                duty = GetDuty(h, l);
                return duty;
            }
            set
            {
                duty = value;
                var hl = GetHL(frequency_hz, duty);
                WriteRegister((uint)Register.HIGH_CYCLES, hl.Item1);
                WriteRegister((uint)Register.LOW_CYCLES, hl.Item2);
            }
        }

        double delay = 0;
        [Category("Configuration")]
        [Description("Delay from start of acquisition to start the clock (sec)")]
        [Range(0, int.MaxValue)]
        public double Delay
        {
            get
            {
                var d = ReadRegister((uint)Register.DELAY_CYCLES);
                delay = (double)d / BaseClockHz;
                return delay;
            }
            set
            {
                delay = value;
                var d = delay * BaseClockHz;
                WriteRegister((uint)Register.DELAY_CYCLES, (uint)d);
            }
        }

        [Category("Configuration")]
        [Description("Should the clock start synchronously with acquisition (true) or free run " +
            "whenever enabled (false)?")]
        public bool SyncToRun
        {
            get
            {
                return ReadRegister((int)Register.GATE_WITH_RUNNING) == 1;
            }
            set
            {
                WriteRegister((int)Register.GATE_WITH_RUNNING, (uint)(value ? 1 : 0));
            }
        }

        protected override void OnNext(ONIContextTask ctx, bool enabled)
        {
            ClockEnabled = enabled;
        }
    }
}
