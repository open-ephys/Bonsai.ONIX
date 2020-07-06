using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONI
{
    [Description("Controls the high performance output clock that is synchronized to the system clock on the Open Ephys FMC Host.")]
    public class FMCCLKOutDevice : ONIRegisterOnlyDeviceBuilder
    {
        // Control registers (see oedevices.h)
        public enum Register
        {
            NULLPARAM = 0,
            ENABLE = 1,
            HIGH_CYCLES = 2,
            LOW_CYCLES = 3,
            DELAY_CYCLES = 4,
            GATE_WITH_RUNNING = 5,
        }

        Tuple<uint, uint> get_hl(double frequency, double duty)
        {
            var l = (ClockHz / frequency) * (1 - duty / 100);
            var h = (ClockHz / frequency) - l;
            return new Tuple<uint, uint>((uint)h, (uint)l);
        }

        double get_freq(double h, double l)
        {
            return ClockHz / (h + l);
        }

        double get_duty(double h, double l)
        {
            return 100.0 * h / (h + l);
        }

        public FMCCLKOutDevice() : base(oni.Device.DeviceID.FMCCLKOUT1R3) { }

        bool clock_enabled = false;
        [Description("Enable.")]
        public bool ClockEnabled
        {
            get
            {
                if (Controller != null)
                {
                    clock_enabled = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.ENABLE) != 0;
                    return clock_enabled;
                }
                else
                {
                    return clock_enabled;
                }
            }
            set
            {
                if (Controller != null)
                {
                    clock_enabled = value;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.ENABLE, (uint)(clock_enabled ? 1 : 0));
                }
            }
        }

        double frequency_hz = 1e6;
        [Range(0, 100e6)]
        [Description("Clock frequency (Hz).")]
        public double Frequency
        {
            get
            {
                if (Controller != null)
                {
                    var h = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.HIGH_CYCLES);
                    var l = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LOW_CYCLES);
                    frequency_hz = get_freq(h, l);
                    return frequency_hz;
                }
                else
                {
                    return frequency_hz;
                }
            }
            set
            {
                if (Controller != null)
                {
                    frequency_hz = value;
                    var hl = get_hl(frequency_hz, duty);
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.HIGH_CYCLES, hl.Item1);
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LOW_CYCLES, hl.Item2);
                }
            }
        }

        double duty = 50.0;
        [Description("Duty Cycle (%).")]
        [Range(1, 99)]
        public double DutyCycle
        {
            get
            {
                if (Controller != null)
                {
                    var h = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.HIGH_CYCLES);
                    var l = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LOW_CYCLES);
                    h = h == 0 ? 1 : h; // the firmware does this as well
                    l = l == 0 ? 1 : l; // the firmware does this as well
                    duty = get_duty(h, l);
                    return duty;
                }
                else
                {
                    return duty;
                }
            }
            set
            {
                if (Controller != null)
                {
                    duty = value;
                    var hl = get_hl(frequency_hz, duty);
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.HIGH_CYCLES, hl.Item1);
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LOW_CYCLES, hl.Item2);
                }
            }
        }

        double delay = 0;
        [Range(0, int.MaxValue)]
        [Description("Delay from start of acquisition to start the clock (sec)")]
        public double Delay
        {
            get
            {
                if (Controller != null)
                {
                    var d = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.DELAY_CYCLES);
                    delay = d / ClockHz;
                    return delay;
                }
                else
                {
                    return delay;
                }
            }
            set
            {
                if (Controller != null)
                {
                    delay = value;
                    var d = delay * ClockHz;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.DELAY_CYCLES,(uint)d);
                }
            }
        }

        bool gate_with_run = true;
        [Description("Should the clock start synchronously with acquisition (true) or free run whenever enabled (false) ?")]
        public bool SyncToRun
        {
            get
            {
                if (Controller != null)
                {
                    gate_with_run = Controller.AcqContext.ReadRegister((uint)DeviceIndex.SelectedIndex, (int)Register.GATE_WITH_RUNNING) == 1;
                    return gate_with_run;
                }
                else
                {
                    return gate_with_run;
                }
            }
            set
            {
                if (Controller != null)
                {
                    gate_with_run = value;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.GATE_WITH_RUNNING, (uint)(gate_with_run ? 1 : 0));
                }
            }
        }

    }
}
