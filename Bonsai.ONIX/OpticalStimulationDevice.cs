using System;
using System.ComponentModel;

// TODO: Linearize MAXCURRENT
namespace Bonsai.ONIX
{
    [Description("Controls a dual channel optical (LED or Laser Diode) stimulator. True = Stimulation triggered, False = Stimulation untriggered.")]
    public sealed class OpticalStimulationDevice : ONIRegisterOnlyDeviceBuilder<bool>
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
        public OpticalStimulationDevice() : base(ONIXDevices.ID.OSTIM) { }

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
            return 3.833e+05 * Math.Pow(R, -0.9632);
        }

        double MinRheostatResistance
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.MINRHEOR);
            }
        }

        double PotResistance
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.POTRES);
            }
        }

        [Category("Acquisition")]
        [Description("Max current per channel (mA).")]
        [Range(0, 800)]
        public double MaxCurrent
        {
            get
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.MAXCURRENT);
                return PotSettingToCurrent(val);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.MAXCURRENT, CurrentToPotSettings((double)value));
            }
        }

        // LED outputs 1-8 are tied to channel 0
        [Category("Acquisition")]
        [Description("Global Enable. If false, all triggers will be ignored.")]
        public bool GlobalEnable
        {
            get
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.ENABLE);
                return val > 0;
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        // LED outputs 1-8 are tied to channel 0
        [Category("Acquisition")]
        [Description("Channel 0 Enable. If true, channel 0 will respond to triggers.")]
        public bool Channel0Enable
        {
            get
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEMASK);
                val &= 0x000000FF;
                return val > 0;
            }
            set
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEMASK);
                var ub = val & 0x0000FF00;
                var lb = value ? 0x000000FF : (uint)0x00000000;
                val = ub | lb;
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEMASK, val);
            }
        }

        // LED outputs 9-16are tied to channel 1
        [Category("Acquisition")]
        [Description("Channel 1 Enable. If true, channel1 will respond to triggers.")]
        public bool Channel1Enable
        {
            get
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEMASK);
                val &= 0x0000FF00;
                return val > 0;
            }
            set
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEMASK);
                var lb = val & 0x000000FF;
                var ub = value ? 0x0000FF00 : (uint)0x00000000;
                val = ub | lb;
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEMASK, val);
            }
        }

        [Category("Acquisition")]
        [Description("Pulse duration (msec).")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 50.0)]
        public double PulseDuration
        {
            get
            {
                return 0.001 * ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEDUR);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEDUR, (uint)(1000 * value));
            }
        }

        [Category("Acquisition")]
        [Description("Pulse period (msec).")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 1000.0)]
        public double PulsePeriod
        {
            get
            {
                return 0.001 * ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEPERIOD);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEPERIOD, (uint)(1000 * value));
            }
        }

        [Category("Acquisition")]
        [Description("Number of pulses to deliver in a burst.")]
        [Range(0, int.MaxValue)]
        public uint BurstPulseCount
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BURSTCOUNT);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.BURSTCOUNT, value);
            }
        }

        [Category("Acquisition")]
        [Description("Interburst interval (msec).")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 10000.0)]
        public double InterBurstInterval
        {
            get
            {
                return 0.001 * ReadRegister(DeviceIndex.SelectedIndex, (int)Register.IBI);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.IBI, (uint)(1000 * value));
            }
        }

        [Category("Acquisition")]
        [Description("Number of bursts to deliver in a train.")]
        [Range(0, int.MaxValue)]
        public uint TrainBurstCount
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINCOUNT);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINCOUNT, value);
            }
        }

        [Category("Acquisition")]
        [Description("Delay between issue of trigger and start of train (msec).")]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Range(0.0, 1000.0)]
        public double TrainDelay
        {
            get
            {
                return 0.001 * ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINDELAY);
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINDELAY, (uint)(1000 * value));
            }
        }

        //[Category("Acquisition")]
        //[Description("Triggered (True = Stimulation triggered, False = Stimulation untriggered,).")]
        //public bool Triggered
        //{
        //    get
        //    {
        //        var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TRIGGER);
        //        return val != 0;
        //    }
        //    set
        //    {
        //        WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRIGGER, (uint)(value ? 1 : 0));
        //    }
        //}

        public override void DoIt(bool triggered)
        {
            WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRIGGER, (uint)(triggered ? 1 : 0));
        }
    }
}