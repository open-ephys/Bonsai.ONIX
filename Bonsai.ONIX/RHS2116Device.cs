using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.RHS2116)]
    [Description("Acquires data from a single RHS2116 bioamplifier chip. " +
        "Ephys and DC data are acquired at 30.193236715 kHz/channel.")]
    [DefaultProperty("StimulusSequence")]
    public class RHS2116Device : ONIFrameReader<RHS2116DataFrame, ushort>
    {
        public const double SamplePeriodMicroSeconds = 1e6 / 30.1932367151e3;
        public const int StimMemorySlotsAvailable = 1024;
        private RHS2116StimulusSequence stimulusSequence = new RHS2116StimulusSequence();

        internal enum Register
        {
            // Unmnanaged
            // See http://intantech.com/files/Intan_RHS2116_datasheet.pdf
            BIAS = 0,     
            FORMAT,   
            ZCHECK,   
            DAC,      
            BW0,      
            BW1,      
            BW2,      
            BW3,      
            PWR,      
            SETTLE = 10,
            LOWAB = 12,
            STIMENA =  32,
            STIMENB,
            STEPSZ,  
            STIMBIAS,
            RECVOLT, 
            RECCUR,  
            DCPWR,   
            STIMON =  42,
            RECOV = 46,
            LIMREC = 48,
            NEG00 = 64,
            NEG01,
            NEG02,
            NEG03,
            NEG04,
            NEG05,
            NEG06,
            NEG07,
            NEG08,
            NEG09,
            NEG10,
            NEG11,
            NEG12,
            NEG13,
            NEG14,
            NEG15, 
            POS00 = 96,
            POS01,
            POS02,
            POS03,
            POS04,
            POS05,
            POS06,
            POS07,
            POS08,
            POS09,
            POS10,
            POS11,
            POS12,
            POS13,
            POS14,
            POS15,

            // Managed
            ENABLE = 0x00008000,    // 32768
            MAXDELTAS,              // 32769
            NUMDELTAS,              // 32770
            DELTAIDXTIME,           // 32771
            DELTAPOLEN,             // 32772
            SEQERROR,               // 32773
            TRIGGER,                // 32774
            FASTSETTLESAMPLES,      // 32775
            RESPECTSTIMACTIVE       // 32776
        }

        protected override IObservable<RHS2116DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            var reg = RHS2116Configuration.StimulatorStepSizeToRegisters[StimulusSequence.CurrentStepSize];
            SetRawRegister((uint) Register.STEPSZ, reg[2] << 13 & reg[1] << 7 & reg[0]);

            var format = DataFormat;

            return source
                .Buffer(BlockSize)
                .Select(block => { return new RHS2116DataFrame(block, frameOffset, format); });
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        #region Configuration 
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

        [Category("Configuration")]
        [Range(1, 10000)]
        [Description("The number of frames making up a single data block that is propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 30;

        private RHS2116Configuration.DataFormat dataFormat = default;
        [Category("Configuration")]
        [Description("Ephys and DC data format.")]
        public RHS2116Configuration.DataFormat DataFormat
        {
            get
            {
                return dataFormat;
            }
            set
            {
                var reg = GetRawRegister((uint)Register.FORMAT);
                reg &= ~(1 << 6); // clear bit 6
                reg |= (value == RHS2116Configuration.DataFormat.Volts ? 1 : (int)value) << 6;
                SetRawRegister((uint)Register.FORMAT, reg);
                dataFormat = value;
            }
        }

        [Category("Configuration")]
        [Description("High-pass digital filter (post-ADC offset removal).")]
        public RHS2116Configuration.DSPCutoff DSPCutoff
        {
            get
            {
                var reg = GetRawRegister((uint)Register.FORMAT);
                return ((reg >> 4) & 0x1) == 0 ?
                    RHS2116Configuration.DSPCutoff.Off : (RHS2116Configuration.DSPCutoff)(reg & 0xF);
            }
            set
            {

                var reg = GetRawRegister((uint)Register.FORMAT);

                if (value == RHS2116Configuration.DSPCutoff.Off)
                {
                    reg &= ~(1 << 4);
                }
                else
                {
                    reg |= 1 << 4;
                    reg &= ~0xF;
                    reg |= (int)value;
                }

                SetRawRegister((uint)Register.FORMAT, reg);
            }
        }

        #endregion

        #region Acquisition
        [Category("Acquisition")]
        [Description("Indicates if a malformed stimulus sequence was uploaded to the device.")]
        public bool SequenceError
        {
            get { return ReadRegister((uint)Register.SEQERROR) > 0; }
        }

        [Category("Acquisition")]
        [Description("Trigger the stimulation sequence. Note: this will only " +
            "trigger the sequence on this device. To synchronize stimulation across " +
            "RHS2116 chips, use RHS2116TriggerDevice.")]
        public bool Trigger
        {
            set 
            { 
                if (value)
                {
                    WriteRegister((uint)Register.TRIGGER, 1);
                } 
            }

            get { return false; }
        }

        [Category("Acquisition")]
        [Description("High-pass analog (pre-ADC) cutoff frequency.")]
        public RHS2116Configuration.AnalogLowCutoff AnalogLowCutoff
        {
            get
            {
                var regs = new int[3];
                var reg = GetRawRegister((uint)Register.BW2); 
                regs[0] = reg & 0b01111111;
                regs[1] = (reg >> 7) & 0b00111111;
                regs[2] = (reg >> 13) & 0x1;
                return RHS2116Configuration.AnalogLowCutoffToRegisters.FirstOrDefault(x => Enumerable.SequenceEqual(x.Value, regs)).Key;
            }
            set
            {
                var regs = RHS2116Configuration.AnalogLowCutoffToRegisters[value];
                var reg = regs[2] << 13 | regs[1] << 7 | regs[0];
                SetRawRegister((uint)Register.BW2, reg);
            }
        }

        [Category("Acquisition")]
        [Description("High-pass analog (pre-ADC) cutoff frequency during stimulus recovery.")]
        public RHS2116Configuration.AnalogLowCutoff AnalogLowCutoffRecovery
        {
            get
            {
                var regs = new int[3];
                var reg = GetRawRegister((uint)Register.BW3);
                regs[0] = reg & 0b01111111;
                regs[1] = (reg >> 7) & 0b00111111;
                regs[2] = (reg >> 13) & 0x1;
                return RHS2116Configuration.AnalogLowCutoffToRegisters.FirstOrDefault(x => Enumerable.SequenceEqual(x.Value, regs)).Key;
            }
            set
            {
                var regs = RHS2116Configuration.AnalogLowCutoffToRegisters[value];
                var reg = regs[2] << 13 | regs[1] << 7 | regs[0];
                SetRawRegister((uint)Register.BW3, reg);
            }
        }

        [Category("Acquisition")]
        [Description("Low-pass analog (pre-ADC) cutoff frequency.")]
        public RHS2116Configuration.AnalogHighCutoff AnalogHighCutoff
        {
            get
            {
                var regs = new int[4];
                var rh1 = GetRawRegister((uint)Register.BW0);
                var rh2 = GetRawRegister((uint)Register.BW1);

                regs[0] = rh1 & 0x3F;
                regs[1] = (rh1 & 0x7C0) >> 6;
                regs[2] = rh2 & 0x3F;
                regs[3] = (rh2 & 0x7C0) >> 6;
                return RHS2116Configuration.AnalogHighCutoffToRegisters.FirstOrDefault(x => Enumerable.SequenceEqual(x.Value, regs)).Key;
            }
            set
            {
                var regs = RHS2116Configuration.AnalogHighCutoffToRegisters[value];

                SetRawRegister((uint)Register.BW0, regs[1] << 6 | regs[0]);
                SetRawRegister((uint)Register.BW1, regs[3] << 6 | regs[2]);
                WriteRegister((uint)Register.FASTSETTLESAMPLES, 
                    RHS2116Configuration.AnalogHighCutoffToFastSettleSamples[value]);
            }
        }

        [Category("Acquisition")]
        [Description("If true, this device will apply AnalogLowCutoffRecovery " +
            "if stimulation occurs via any RHS chip the same headstage or others that are connected" +
            "using StimActive pin. If false, this device will apply AnalogLowCutoffRecovery during its" +
            "own stimuli.")]
        public bool RespectExternalActiveStim
        {
            get
            {
                return (ReadRegister((uint)Register.RESPECTSTIMACTIVE) & 0x0001) == 1;
            }
            set
            {
                WriteRegister((uint)Register.RESPECTSTIMACTIVE, (uint)(value ? 1 : 0));
            }
        }

        [Category("Acquisition")]
        [Description("RHS2116 stimulation sequencer configuration.")]
        [Editor("Bonsai.ONIX.Design.RHS2116StimulusSequenceEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        public RHS2116StimulusSequence StimulusSequence
        {
            get { return stimulusSequence; }
            set
            {
                // TODO: Is this a problem to do here?
                // Save the stimulus sequence because tossing the results, even if invalid, is very annoying for the user
                stimulusSequence = value;

                if (!stimulusSequence.Valid)
                {
                    throw new WorkflowException("The requested stimulus sequence is invalid.");
                }

                if (!stimulusSequence.FitsInHardware)
                {
                    throw new WorkflowException(string.Format("The requested stimulus is too complex. {0}/{1} memory slots are required.",
                        StimulusSequence.StimulusSlotsRequired,
                        RHS2116Device.StimMemorySlotsAvailable));
                }

                using (var regIO = new RegisterConfiguration(DeviceAddress, ID))
                {
                    // Anodic amplitudes
                    // TODO: cache last write and compare
                    var regAddr = (int)Register.POS00;
                    var a = value.ToAmplitudes(true);
                    for (int i = 0; i < a.Count(); i++)
                    {
                        regIO.WriteRegister((uint)(regAddr + i), a.ElementAt(i));
                    }

                    // Cathodic amplitudes
                    // TODO: cache last write and compare
                    regAddr = (int)Register.NEG00;
                    a = value.ToAmplitudes(false);
                    for (int i = 0; i < a.Count(); i++)
                    {
                        regIO.WriteRegister((uint)(regAddr + i), a.ElementAt(i));
                    }

                    // Create delta table and set length
                    var dt = value.DeltaTable;
                    regIO.WriteRegister((uint)Register.NUMDELTAS, (uint)dt.Count);

                    // If we want to do this efficently, we probably need a different data structure on the
                    // FPGA ram that allows columns to be out of order (e.g. linked list)
                    uint j = 0;
                    foreach (var d in dt)
                    {
                        uint idxTime = j++ << 22 | (d.Key & 0x003FFFFF);
                        regIO.WriteRegister((uint)Register.DELTAIDXTIME, idxTime);
                        regIO.WriteRegister((uint)Register.DELTAPOLEN, d.Value);
                    }
                }
            }
        }

        #endregion

        // NB: replaced with const StimMemorySlotsAvailable since this is const in firmware
        // and didn't want the overhead and hardware access at design time
        //private int MaxDeltas
        //{
        //    get { return (int)ReadRegister((uint)Register.MAXDELTAS); }
        //}

        private void SetRawRegister(uint address, int value)
        {
            WriteRegister(address, (uint)value);
        }

        private int GetRawRegister(uint address)
        {
            return (int)ReadRegister(address);
        }
    }
}
