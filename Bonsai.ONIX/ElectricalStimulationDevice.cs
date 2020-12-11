using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONIX
{
    [Description("Controls a single microstimulator circuit.")]
    public sealed class ElectricalStimulationDevice : ONIRegisterOnlyDeviceBuilder
    {
        // Control registers (see onidevices.h)
        public enum Register
        {
            NULLPARM = 0,  // No command
            BIPHASIC = 1,  // Biphasic pulse (0 = monophasic, 1 = biphasic; NB: currently ignored)
            CURRENT1 = 2,  // Phase 1 current, (0 to 255 = -1.5 mA to +1.5mA)
            CURRENT2 = 3,  // Phase 2 voltage, (0 to 255 = -1.5 mA to +1.5mA)
            PULSEDUR1 = 4,  // Phase 1 duration, 10 microsecond steps
            IPI = 5,  // Inter-phase interval, 10 microsecond steps
            PULSEDUR2 = 6,  // Phase 2 duration, 10 microsecond steps
            PULSEPERIOD = 7,  // Inter-pulse interval, 10 microsecond steps
            BURSTCOUNT = 8,  // Burst duration, number of pulses in burst
            IBI = 9,  // Inter-burst interval, microseconds
            TRAINCOUNT = 10, // Pulse train duration, number of bursts in train
            TRAINDELAY = 11, // Pulse train delay, microseconds
            TRIGGER = 12, // Trigger stimulation (1 = deliver)
            POWERON = 13, // Control estim sub-circuit power (0 = off, 1 = on)
            ENABLE = 14, // Control null switch (0 = stim output shorted to ground, 1 = stim output attached to electrode during pulses)
            RESTCURR = 15, // Resting current between pulse phases, (0 to 255 = -1.5 mA to +1.5mA)
            RESET = 16, // Reset all parameters to default
            REZ = 17, // Internal DAC resolution in bits
        }

        // Setup context etc
        public ElectricalStimulationDevice() : base(ONIXDevices.ID.ESTIM) { }

        private uint currentK(double currentuA)
        {
            double k = 3000 / (Math.Pow(2, DACResolution ?? double.NaN) - 1);
            return (uint)((currentuA + 1500) / k);
        }

        private double inv_currentK(uint current_k)
        {
            double k = 3000 / (Math.Pow(2, DACResolution ?? double.NaN) - 1);
            return current_k * k - 1500;
        }

        [System.Xml.Serialization.XmlIgnore]
        [Category("Configuration")]
        [Description("Resolution of stimulation waveform generation DAC in bits.")]
        public uint? DACResolution
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.REZ);
            }
        }

        [Category("Acquisition")]
        [Description("Phase 1 pulse current (uA).")]
        [Range(-1500, 1500)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public double? Phase1CurrentuA
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CURRENT1);
                if (val != null) return inv_currentK((uint)val);
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.CURRENT1, currentK((double)value));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Phase 2 pulse current (uA).")]
        [Range(-1500, 1500)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public double? Phase2CurrentuA
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CURRENT2);
                if (val != null) return inv_currentK((uint)val);
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.CURRENT2, currentK((double)value));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Resting current between pulse phases(uA).")]
        [Range(-1500, 1500)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public double? RestingCurrentuA
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.RESTCURR);
                if (val != null) return inv_currentK((uint)val);
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.RESTCURR, currentK((double)value));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Phase 1 pulse duration (usec).")]
        [Range(0, int.MaxValue)]
        public uint? PulsePhase1DurationuSec
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEDUR1);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEDUR1, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Inter-pulse phase duration (usec).")]
        [Range(0, int.MaxValue)]
        public uint? InterPulsePhaseDurationuSec
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.IPI);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.IPI, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Phase 2 pulse duration (usec).")]
        [Range(0, int.MaxValue)]
        public uint? PulsePhase2DurationuSec
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEDUR2);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEDUR2, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Pulse period (usec).")]
        [Range(0, int.MaxValue)]
        public uint? PulsePerioduSec
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEPERIOD);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.PULSEPERIOD, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Number of pulses to deliver in a burst.")]
        [Range(0, int.MaxValue)]
        public uint? BurstPulseCount
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.BURSTCOUNT);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.BURSTCOUNT, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Interburst interval (usec).")]
        [Range(0, int.MaxValue)]
        public uint? InterBurstIntervaluSec
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.IBI);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.IBI, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Number of bursts to deliver in a train.")]
        [Range(0, int.MaxValue)]
        public uint? TrainBurstCount
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINCOUNT);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINCOUNT, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Delay between issue of trigger and start of train (usec).")]
        [Range(0, int.MaxValue)]
        public uint? TrainDelayuSec
        {
            get
            {
                return Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINDELAY);
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRAINDELAY, (uint)value);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Stimulation sub-circuit power (True = On, False = Off).")]
        public bool? PowerOn
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.POWERON);
                if (val != null) return val != 0;
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.POWERON, (uint)((bool)value ? 1 : 0));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Should the stimulator respect trigger signals?")]
        public bool? Enable
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.ENABLE);
                if (val != null) return val != 0;
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.ENABLE, (uint)((bool)value ? 1 : 0));
                }
            }
        }

        [Category("Acquisition")]
        [Description("Triggered (True = Stimulation triggered, False = Stimulation untriggered,).")]
        public bool? Triggered
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TRIGGER);
                if (val != null) return val != 0;
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.TRIGGER, (uint)((bool)value ? 1 : 0));
                }
            }
        }
    }
}