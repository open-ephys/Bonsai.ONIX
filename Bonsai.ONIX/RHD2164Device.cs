using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    // TODO: More abstract control over chip's registers?

    [Description("Acquires data from a single RHD2164 bioamplifier chip.")]
    public class RHD2164Device : ONIFrameReaderDeviceBuilder<RHDDataFrame>
    {
        // see http://intantech.com/files/Intan_RHD2164_datasheet.pdf
        enum Register
        {
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
        }

        public RHD2164Device() : base(ONIXDevices.ID.RHD2164) { }

        public override IObservable<RHDDataFrame> Process(IObservable<oni.Frame> source)
        {
            var data_block = new RHDDataBlock(64, BlockSize);

            return source
                .Where(f =>
                {
                    return data_block.FillFromFrame(f);
                })
                .Select(f =>
                {
                    var sample = new RHDDataFrame(data_block);
                    data_block = new RHDDataBlock(64, BlockSize);
                    return sample;
                });
        }

        [Category("Acquisition")]
        [Range(10, 10000)]
        [Description("The size of data blocks, in samples, that are propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 250;

        [Category("Configuration")]
        [Description("Register 0: ADC Configuration and Amplifier Fast Settle. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte ADCConfig
        {
            get
            {
                return GetRawRegister((uint)Register.ADCCONF);
            }
            set
            {
                SetRawRegister((uint)Register.ADCCONF, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 1: Supply Sensor and ADC Buffer Bias Current. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte ADCBias
        {
            get
            {
                return GetRawRegister((uint)Register.ADCBUFF);
            }
            set
            {
                SetRawRegister((uint)Register.ADCBUFF, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 2: MUX Bias Current. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte MuxBias
        {
            get
            {
                return GetRawRegister((uint)Register.MUXBIAS);
            }
            set
            {
                SetRawRegister((uint)Register.MUXBIAS, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 3: MUX Load, Temperature Sensor, and Auxiliary Digital Output. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte MuxLoad
        {
            get
            {
                return GetRawRegister((uint)Register.MUXLOAD);
            }
            set
            {
                SetRawRegister((uint)Register.MUXLOAD, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 4: ADC Output Format and DSP Offset Removal. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte Format
        {
            get
            {
                return GetRawRegister((uint)Register.FORMAT);
            }
            set
            {
                SetRawRegister((uint)Register.FORMAT, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 5: Impedance Check Control. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte ZCheck
        {
            get
            {
                return GetRawRegister((uint)Register.ZCHECK);
            }
            set
            {
                SetRawRegister((uint)Register.ZCHECK, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 6: Impedance Check DAC. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte ZDac
        {
            get
            {
                return GetRawRegister((uint)Register.ZDAC);
            }
            set
            {
                SetRawRegister((uint)Register.ZDAC, value);
            }
        }

        [Category("Configuration")]
        [Description("Register 7: Impedance Check Amplifier Select. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte ZSelect
        {
            get
            {
                return GetRawRegister((uint)Register.ZSELECT);
            }
            set
            {
                SetRawRegister((uint)Register.ZSELECT, value);
            }
        }

        byte[] bw_reg;
        [Category("Configuration")]
        [Description("Registers 8-13: On-Chip Amplifier Bandwidth Select. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte[] Bandwidth
        {
            get
            {
                bw_reg = new byte[6];
                var start = (uint)Register.BW0;
                for (uint i = 0; i <= Register.BW5 - Register.BW0; i++)
                {
                    bw_reg[i] = GetRawRegister(start + i);
                }

                return bw_reg;
            }
            set
            {
                if (bw_reg == null)
                {
                    bw_reg = Bandwidth;
                }

                var start = (uint)Register.BW0;
                for (uint i = 0; i <= Register.BW5 - Register.BW0; i++)
                {
                    if (bw_reg[i] != value[i])
                    {
                        SetRawRegister(start + i, value[i]);
                    }
                }
            }
        }

        byte[] pwr_reg;
        [Category("Configuration")]
        [Description("Registers 14-17: Individual Amplifier Power. See http://intantech.com/files/Intan_RHD2164_datasheet.pdf.")]
        public byte[] Power
        {
            get
            {
                pwr_reg = new byte[8];
                var start = (uint)Register.PWR0;
                for (uint i = 0; i <= Register.PWR7 - Register.PWR0; i++)
                {
                    pwr_reg[i] = GetRawRegister(start + i);
                }

                return pwr_reg;
            }
            set
            {
                if (pwr_reg == null)
                {
                    pwr_reg = Power;
                }

                var start = (uint)Register.PWR0;
                for (uint i = 0; i <= Register.PWR7 - Register.PWR0; i++)
                {
                    if (pwr_reg[i] != value[i])
                    {
                        SetRawRegister(start + i, value[i]);
                    }
                }
            }
        }

        void SetRawRegister(uint reg_addr, uint val)
        {
            WriteRegister(DeviceIndex.SelectedIndex, reg_addr, val);
        }

        byte GetRawRegister(uint reg_addr)
        {
            return (byte)ReadRegister(DeviceIndex.SelectedIndex, reg_addr);
        }
    }
}
