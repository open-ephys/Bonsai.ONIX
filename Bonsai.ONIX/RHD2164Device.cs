using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.RHD2164)]
    [Description("Acquires data from a single RHD2164 bioamplifier chip. Ephys data is acquired at 30 kHz/channel.")]
    public class RHD2164Device : ONIFrameReader<RHD2164DataFrame, ushort>
    {

        private enum Register
        {
            // Unmnanaged
            // See http://intantech.com/files/Intan_RHD2164_datasheet.pdf
            ADCCONF = 0,
            ADCBUFF,
            MUXBIAS,
            MUXLOAD,
            FORMAT,
            ZCHECK,
            ZDAC,
            ZSELECT,
            BW0,
            BW1,
            BW2,
            BW3,
            BW4,
            BW5,
            PWR0,
            PWR1,
            PWR2,
            PWR3,
            PWR4,
            PWR5,
            PWR6,
            PWR7,

            // Managed
            ENABLE = 0x00008000
        }

        protected override IObservable<RHD2164DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            var ephysDataFormat = EphysDataFormat;
            var auxDataFormat = AuxDataFormat;

            return source
                .Buffer(BlockSize)
                .Select(block => { return new RHD2164DataFrame(block, frameOffset, ephysDataFormat, auxDataFormat); });
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

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

        private RHD2164Configuration.EphysDataFormat ephysDataFormat = default;
        [Category("Configuration")]
        [Description("Ephys data format.")]
        public RHD2164Configuration.EphysDataFormat EphysDataFormat
        {
            get
            {
                if (ephysDataFormat != RHD2164Configuration.EphysDataFormat.MicroVolts)
                {
                    var reg = GetRawRegister((uint)Register.FORMAT);
                    ephysDataFormat = (RHD2164Configuration.EphysDataFormat)((reg >> 6) & 0x1);
                }

                return ephysDataFormat;
            }
            set
            {
                var reg = GetRawRegister((uint)Register.FORMAT);
                reg &= ~(1 << 6);
                reg |= (value == RHD2164Configuration.EphysDataFormat.MicroVolts ? 1 : (int)value) << 6;
                SetRawRegister((uint)Register.FORMAT, reg);
                ephysDataFormat = value;
            }
        }

        [Category("Configuration")]
        [Description("Auxiliary channel data format")]
        public RHD2164Configuration.AuxDataFormat AuxDataFormat { get; set; }

        [Category("Configuration")]
        [Description("High-pass digital filter (post-ADC offset removal).")]
        public RHD2164Configuration.DSPCutoff DSPCutoff
        {
            get
            {
                var reg = GetRawRegister((uint)Register.FORMAT);
                return ((reg >> 4) & 0x1) == 0 ?
                    RHD2164Configuration.DSPCutoff.Off : (RHD2164Configuration.DSPCutoff)(reg & 0xF);
            }
            set
            {

                var reg = GetRawRegister((uint)Register.FORMAT);

                if (value == RHD2164Configuration.DSPCutoff.Off)
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

        [Category("Configuration")]
        [Description("High-pass analog (pre-ADC) cutoff frequency.")]
        public RHD2164Configuration.AnalogLowCutoff AnalogLowCutoff
        {
            get
            {
                var regs = new int[3];
                regs[0] = GetRawRegister((uint)Register.BW4) & 0b01111111;
                var reg = GetRawRegister((uint)Register.BW5);
                regs[1] = reg & 0b00111111;
                regs[2] = (reg >> 6) & 0x1;
                return RHD2164Configuration.AnalogLowCutoffToRegisters.FirstOrDefault(x => Enumerable.SequenceEqual(x.Value, regs)).Key;
            }
            set
            {
                var regs = RHD2164Configuration.AnalogLowCutoffToRegisters[value];
                var reg0 = GetRawRegister((uint)Register.BW4);
                reg0 &= ~0b01111111;
                reg0 |= regs[0] & 0b01111111;
                SetRawRegister((uint)Register.BW4, reg0);

                var reg12 = GetRawRegister((uint)Register.BW5);
                reg12 &= ~0b01111111;
                reg12 |= (regs[2] << 6) & 0b01000000 | regs[1] & 0b00111111;
                SetRawRegister((uint)Register.BW5, reg12);
            }
        }

        [Category("Configuration")]
        [Description("Low-pass analog (pre-ADC) cutoff frequency.")]
        public RHD2164Configuration.AnalogHighCutoff AnalogHighCutoff
        {
            get
            {
                var regs = new int[4];
                regs[0] = GetRawRegister((uint)Register.BW0) & 0b00111111;
                regs[1] = GetRawRegister((uint)Register.BW1) & 0b00011111;
                regs[2] = GetRawRegister((uint)Register.BW2) & 0b00111111;
                regs[3] = GetRawRegister((uint)Register.BW3) & 0b00011111;
                return RHD2164Configuration.AnalogHighCutoffToRegisters.FirstOrDefault(x => Enumerable.SequenceEqual(x.Value, regs)).Key;
            }
            set
            {
                var regs = RHD2164Configuration.AnalogHighCutoffToRegisters[value];

                var reg = GetRawRegister((uint)Register.BW0);
                reg &= ~0b00111111;
                reg |= regs[0] & 0b00111111;
                SetRawRegister((uint)Register.BW0, reg);

                reg = GetRawRegister((uint)Register.BW1);
                reg &= ~0b00011111;
                reg |= regs[1] & 0b00011111;
                SetRawRegister((uint)Register.BW1, reg);

                reg = GetRawRegister((uint)Register.BW2);
                reg &= ~0b00111111;
                reg |= regs[2] & 0b00111111;
                SetRawRegister((uint)Register.BW2, reg);

                reg = GetRawRegister((uint)Register.BW3);
                reg &= ~0b00011111;
                reg |= regs[3] & 0b00011111;
                SetRawRegister((uint)Register.BW3, reg);
            }
        }

        private void SetRawRegister(uint address, int value)
        {
            WriteRegister(address, (uint)value);
        }

        private int GetRawRegister(uint address)
        {
            return (int)ReadRegister(address);
        }

        #region RawRegisters
        //[Category("Configuration")]
        //[Description("Register 0: ADC Configuration and Amplifier Fast Settle. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte ADCConfig
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.ADCCONF);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.ADCCONF, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 1: Supply Sensor and ADC Buffer Bias Current. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte ADCBias
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.ADCBUFF);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.ADCBUFF, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 2: MUX Bias Current. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte MuxBias
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.MUXBIAS);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.MUXBIAS, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 3: MUX Load, Temperature Sensor, and Auxiliary Digital Output. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte MuxLoad
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.MUXLOAD);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.MUXLOAD, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 4: ADC Output Format and DSP Offset Removal. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte Format
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.FORMAT);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.FORMAT, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 5: Impedance Check Control. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte ZCheck
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.ZCHECK);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.ZCHECK, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 6: Impedance Check DAC. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte ZDac
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.ZDAC);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.ZDAC, value);
        //    }
        //}

        //[Category("Configuration")]
        //[Description("Register 7: Impedance Check Amplifier Select. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte ZSelect
        //{
        //    get
        //    {
        //        return GetRawRegister((uint)Register.ZSELECT);
        //    }
        //    set
        //    {
        //        SetRawRegister((uint)Register.ZSELECT, value);
        //    }
        //}

        //byte[] bw_reg;
        //[Category("Configuration")]
        //[Description("Registers 8-13: On-Chip Amplifier Bandwidth Select. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte[] Bandwidth
        //{
        //    get
        //    {
        //        bw_reg = new byte[6];
        //        var start = (uint)Register.BW0;
        //        for (uint i = 0; i <= Register.BW5 - Register.BW0; i++)
        //        {
        //            bw_reg[i] = GetRawRegister(start + i);
        //        }

        //        return bw_reg;
        //    }
        //    set
        //    {
        //        if (bw_reg == null)
        //        {
        //            bw_reg = Bandwidth;
        //        }

        //        var start = (uint)Register.BW0;
        //        for (uint i = 0; i <= Register.BW5 - Register.BW0; i++)
        //        {
        //            if (bw_reg[i] != value[i])
        //            {
        //                SetRawRegister(start + i, value[i]);
        //            }
        //        }
        //    }
        //}

        //byte[] pwr_reg;
        //[Category("Configuration")]
        //[Description("Registers 14-17: Individual Amplifier Power. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        //public byte[] Power
        //{
        //    get
        //    {
        //        pwr_reg = new byte[8];
        //        var start = (uint)Register.PWR0;
        //        for (uint i = 0; i <= Register.PWR7 - Register.PWR0; i++)
        //        {
        //            pwr_reg[i] = GetRawRegister(start + i);
        //        }

        //        return pwr_reg;
        //    }
        //    set
        //    {
        //        if (pwr_reg == null)
        //        {
        //            pwr_reg = Power;
        //        }

        //        var start = (uint)Register.PWR0;
        //        for (uint i = 0; i <= Register.PWR7 - Register.PWR0; i++)
        //        {
        //            if (pwr_reg[i] != value[i])
        //            {
        //                SetRawRegister(start + i, value[i]);
        //            }
        //        }
        //    }
        //}
        #endregion

    }
}
