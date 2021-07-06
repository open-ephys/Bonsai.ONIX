using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONIX
{
    [Description("Controls a dual channel optical (LED or Laser Diode) stimulator. A boolean input can be" +
        "used to trigger stimulation: True = Stimulation triggered, False = Stimulation untriggered.")]
    [DefaultProperty("DeviceAddress")]
    public sealed class OpticalStimulationDevice : ONISink<bool>
    {
        enum Register
        {
            NULLPARM = 0,   // No command
            MAXCURRENT,     // Max LED/LD current, (0 to 255 = 800mA to 0 mA.See fig XX of CAT4016 datasheet)
            PULSEMASK,      // Bitmask determining which of the(up to 32) channels is affected by trigger
            PULSEDUR,       // Pulse duration, microseconds
            PULSEPERIOD,    // Inter-pulse interval, microseconds
            BURSTCOUNT,     // Burst duration, number of pulses in burst
            IBI,            // Inter-burst interval, microseconds
            TRAINCOUNT,     // Pulse train duration, number of bursts in train, 0 = continuous
            TRAINDELAY,     // Pulse train delay, microseconds
            TRIGGER,        // Trigger stimulation (0 = off, 1 = deliver)
            ENABLE,         // Default 1: enables the stimulator, 0: stimulator ignores triggers(so that a common trigger can be used)
            RESTMASK,       // Bitmask determining the "off" state of the up to 32 channels
            RESET,          // None If 1, Reset all parameters to default
            MINRHEOR,       // The series resistor between the potentiometer (rheostat) and RSET bin on the CAT4016
            POTRES,         // The resistance value of the potentiometer connected in rheostat config to RSET on CAT4016
        }

        // Setup context etc
        public OpticalStimulationDevice() : base(ONIXDevices.ID.OpticalStimulator) { }

        // Fit from Fig. 10 of CAT4016 datasheet
        // x = (y/a)^(1/b)
        // a = 3.833e+05
        // b = -0.9632
        private uint CurrentToPotSettings(double current)
        {
            double R = Math.Pow(current / 3.833e+05, 1 / -0.9632);
            double s = 256 * (R - MinRheostatResistance) / PotResistance;
            if (s > 255)
            {
                return 255;
            }
            else if (s < 0)
            {
                return 0;
            }
            else
            {
                return (uint)s;
            }
        }

        // Fit from Fig. 10 of CAT4016 datasheet
        // y = a*x^b
        // a = 3.833e+05
        // b = -0.9632
        private double PotSettingToCurrent(uint setting)
        {
            double R = MinRheostatResistance + setting * PotResistance / 256;
            return Math.Round(3.833e+05 * Math.Pow(R, -0.9632), 2);
        }

        private double MinRheostatResistance
        {
            get
            {
                return ReadRegister((int)Register.MINRHEOR);
            }
        }

        private double PotResistance
        {
            get
            {
                return ReadRegister((int)Register.POTRES);
            }
        }

        private void WritePulseMask(int channel, double percent)
        {
            uint mask = 0x00000000;
            var p = 0.0;
            while (p < percent)
            {
                mask = (mask << 1) | 1;
                p += 12.5;
            }

            var val = ReadRegister((int)Register.PULSEMASK);
            val = channel == 0 ? (val & 0x0000FF00) | mask : (mask << 8) | (val & 0x000000FF);
            WriteRegister((int)Register.PULSEMASK, val);
        }

        private double ReadPulseMask(int channel)
        {
            var val = ReadRegister((int)Register.PULSEMASK);
            var mask = channel == 0 ? (val & 0x000000FF) : (val & 0x0000FF00) >> 8;

            double percent = 0.0;
            while (mask > 0)
            {
                mask >>= 1;
                percent += 12.5;
            }

            return percent;
        }

        [Category("Acquisition")]
        [Description("Maximum current per channel per pulse (mA). " +
            "This value is used by both channels. To get different amplitudes " +
            "for each channel use the Channel0Level and Channel1Level parameters.")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(0, 800)]
        public double MaxCurrent
        {
            get
            {
                var val = ReadRegister((int)Register.MAXCURRENT);
                return PotSettingToCurrent(val);
            }
            set
            {
                WriteRegister((int)Register.MAXCURRENT, CurrentToPotSettings((double)value));
            }
        }

        [Category("Acquisition")]
        [Description("If false, triggers will be ignored.")]
        public bool Enable
        {
            get
            {
                return ReadRegister((int)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((int)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        // Outputs 1-8 are tied to channel 0
        [Category("Acquisition")]
        [Description("Channel 0 percent of max current. If greater than 0, channel 0 will respond to triggers.")]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0, 100)]
        [Precision(1, 12.5)]
        public double ChannelZeroCurrent
        {
            get
            {
                return ReadPulseMask(0);
            }
            set
            {
                WritePulseMask(0, value);
            }
        }

        // Outputs 9-16 are tied to channel 1
        [Category("Acquisition")]
        [Description("Channel 1 percent of max current. If greater than 0, channel 1 will respond to triggers.")]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0, 100)]
        [Precision(1, 12.5)]
        public double ChannelOneCurrent
        {
            get
            {
                return ReadPulseMask(1);
            }
            set
            {
                WritePulseMask(1, value);
            }
        }

        [Category("Acquisition")]
        [Description("Pulse duration (msec).")]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 100.0)]
        [Precision(3, 1)]
        public double PulseDuration
        {
            get
            {
                return 0.001 * ReadRegister((int)Register.PULSEDUR);
            }
            set
            {
                var pulsePeriod = PulsePeriod;
                if (value > pulsePeriod)
                {
                    WriteRegister((int)Register.PULSEDUR, (uint)(1000 * pulsePeriod - 1));
                }
                else
                {
                    WriteRegister((int)Register.PULSEDUR, (uint)(1000 * value));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Pulse period (msec).")]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 1000.0)]
        [Precision(3, 1)]
        public double PulsePeriod
        {
            get
            {
                return 0.001 * ReadRegister((int)Register.PULSEPERIOD);
            }
            set
            {
                var pulseDuration = PulseDuration;
                if (value > pulseDuration)
                {
                    WriteRegister((int)Register.PULSEPERIOD, (uint)(1000 * value));
                }
                else
                {
                    WriteRegister((int)Register.PULSEPERIOD, (uint)(1000 * pulseDuration + 1));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Number of pulses to deliver in a burst.")]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0, int.MaxValue)]
        [Precision(0, 1)]
        public uint BurstPulseCount
        {
            get
            {
                return ReadRegister((int)Register.BURSTCOUNT);
            }
            set
            {
                WriteRegister((int)Register.BURSTCOUNT, value);
            }
        }

        [Category("Acquisition")]
        [Description("Interburst interval (msec).")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 10000.0)]
        [Precision(3, 1)]
        public double InterBurstInterval
        {
            get
            {
                return 0.001 * ReadRegister((int)Register.IBI);
            }
            set
            {
                WriteRegister((int)Register.IBI, (uint)(1000 * value));
            }
        }

        [Category("Acquisition")]
        [Description("Number of bursts to deliver in a train.")]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0, int.MaxValue)]
        [Precision(0, 1)]
        public uint TrainBurstCount
        {
            get
            {
                return ReadRegister((int)Register.TRAINCOUNT);
            }
            set
            {
                WriteRegister((int)Register.TRAINCOUNT, value);
            }
        }

        [Category("Acquisition")]
        [Description("Delay between issue of trigger and start of train (msec).")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 1000.0)]
        [Precision(3, 1)]
        public double TrainDelay
        {
            get
            {
                return 0.001 * ReadRegister((int)Register.TRAINDELAY);
            }
            set
            {
                WriteRegister((int)Register.TRAINDELAY, (uint)(1000 * value));
            }
        }

        [Category("ONI Configuration")]
        [Description("The full device hardware address consisting of a hardware slot and device table index.")]
        [Editor("Bonsai.ONIX.Design.StimulatorEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [TypeConverter(typeof(ONIDeviceAddressTypeConverter))]
        public new ONIDeviceAddress DeviceAddress
        {
            get => base.DeviceAddress;
            set => base.DeviceAddress = value;
        }

        protected override void OnNext(ONIContextTask ctx, bool triggered)
        {
            WriteRegister((int)Register.TRIGGER, (uint)(triggered ? 1 : 0));
        }

        protected override void OnFinally(ONIContextTask ctx)
        {
            WriteRegister((int)Register.TRIGGER, 0);
        }
    }
}