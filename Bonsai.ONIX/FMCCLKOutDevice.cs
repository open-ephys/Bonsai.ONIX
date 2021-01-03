using System;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("Controls the high performance output clock that is synchronized to the system clock on the Open Ephys FMC Host.")]
    public class FMCCLKOutDevice : ONIRegisterOnlyDeviceBuilder<bool>
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

        public FMCCLKOutDevice() : base(ONIXDevices.ID.FMCCLKOUT1R3) { }

        [Description("Enable.")]
        public bool ClockEnabled
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.ENABLE) != 0;
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.ENABLE, (uint)(value ? 1 : 0));
            }
        }

        uint BaseClockHz
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BASE_CLOCK_HZ);
            }
        }

        double frequency_hz = 1e6;
        [Range(0, 100e6)]
        [Description("Output Clock frequency (Hz).")]
        public double Frequency
        {
            get
            {
                var h = ReadRegister(DeviceIndex.SelectedIndex, (uint)Register.HIGH_CYCLES);
                var l = ReadRegister(DeviceIndex.SelectedIndex, (uint)Register.LOW_CYCLES);
                frequency_hz = GetFreq(h, l);
                return frequency_hz;
            }
            set
            {
                frequency_hz = value;
                var hl = GetHL(frequency_hz, duty);
                WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.HIGH_CYCLES, hl.Item1);
                WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.LOW_CYCLES, hl.Item2);
            }
        }

        double duty = 50.0;
        [Description("Duty Cycle (%).")]
        [Range(1, 99)]
        public double DutyCycle
        {
            get
            {
                var h = ReadRegister(DeviceIndex.SelectedIndex, (uint)Register.HIGH_CYCLES);
                var l = ReadRegister(DeviceIndex.SelectedIndex, (uint)Register.LOW_CYCLES);
                h = h == 0 ? 1 : h; // the firmware does this as well
                l = l == 0 ? 1 : l; // the firmware does this as well
                duty = GetDuty(h, l);
                return duty;
            }
            set
            {
                duty = value;
                var hl = GetHL(frequency_hz, duty);
                WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.HIGH_CYCLES, hl.Item1);
                WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.LOW_CYCLES, hl.Item2);
            }
        }

        double delay = 0;
        [Range(0, int.MaxValue)]
        [Description("Delay from start of acquisition to start the clock (sec)")]
        public double Delay
        {
            get
            {
                var d = ReadRegister(DeviceIndex.SelectedIndex, (uint)Register.DELAY_CYCLES);
                delay = d / BaseClockHz;
                return delay;
            }
            set
            {
                delay = value;
                var d = delay * BaseClockHz;
                WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.DELAY_CYCLES, (uint)d);
            }
        }

        [Description("Should the clock start synchronously with acquisition (true) or free run whenever enabled (false) ?")]
        public bool SyncToRun
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.GATE_WITH_RUNNING) == 1;
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.GATE_WITH_RUNNING, (uint)(value ? 1 : 0));
            }
        }

        public override void DoIt(bool enabled)
        {
            ClockEnabled = enabled;
        }

    }
}
