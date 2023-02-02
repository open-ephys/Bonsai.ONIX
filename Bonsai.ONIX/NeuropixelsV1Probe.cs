using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    using Channel = NeuropixelsV1Channel;

    public class NeuropixelsV1Probe : I2CRegisterConfiguration
    {
        // Exposed parameters
        public const int CHANNEL_COUNT = 384;
        public const int ADC_COUNT = 32;
        public const int ELECTRODE_COUNT = 960;
        public const int INTERNAL_REF_CHANNEL = 191;

        // Configuration bit array sizes
        private const int SHANK_CONFIG_BITS = 968;
        private const int BASE_CONFIG_BITS = 2448;

        // Configuration bit indices
        private const int SHANK_BIT_EXT1 = 965;
        private const int SHANK_BIT_EXT2 = 2;
        private const int SHANK_BIT_TIP1 = 484;
        private const int SHANK_BIT_TIP2 = 483;

        // Bit index of base configurations where we start talking about channel configs
        private const int PROBE_SRBASECONFIG_BIT_GAINBASE = 576;

        // Bit index of base configurations where we start talking about ADC calibrations
        // NB: ADC portions of base configurations are _never mutated_ by the NP API
        // It seems that they maybe planned for on-ASIC DSP but then deferred to the FPGA later,
        // where these parameters are actually used.
        // const int PROBE_SRBASECONFIG_BIT_ADCBASE = 2114;

        private enum Register
        {
            // Unmangaged register access handled by I2C base class

            // Managed
            ENABLE = 0x00008000,
            ADC01_00_OFF_THRESH = 0x00008001,
            CHAN001_000_LFPGAIN = 0x00008011,
            CHAN001_000_APGAIN = 0x000080D1,
            PROBE_SN_LSB = 0x00008191,
            PROBE_SN_MSB = 0x00008192
        }

        public NeuropixelsV1Probe(ONIDeviceAddress device) : base(device, DeviceID.NeuropixelsV1, 0x70)
        { }

        public static int ElectrodeToChannel(int electrode)
        {
            return electrode % CHANNEL_COUNT;
        }

        public static Channel.ElectrodeBank ElectrodeToBank(int electrode)
        {
            return (Channel.ElectrodeBank)(electrode / CHANNEL_COUNT);
        }

        public void FullReset()
        {
            // Set memory map and test configuration registers to default values
            WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.CAL_OFF);
            WriteByte((uint)RegAddr.TEST_CONFIG1, 0);
            WriteByte((uint)RegAddr.TEST_CONFIG2, 0);
            WriteByte((uint)RegAddr.TEST_CONFIG3, 0);
            WriteByte((uint)RegAddr.TEST_CONFIG4, 0);
            WriteByte((uint)RegAddr.TEST_CONFIG5, 0);
            WriteByte((uint)RegAddr.SYNC, 0);

            // Perform dig and channel reset
            WriteByte((uint)RegAddr.REC_MOD, (uint)RecMod.RESET_ALL);

            // Change operation state to Recording
            WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD);
        }

        public void DigitalReset()
        {
            WriteByte((uint)RegAddr.REC_MOD, (uint)RecMod.DIG_RESET);
        }

        public void Start()
        {
            // Release resets
            //WriteByte((uint)RegAddr.REC_MOD, (uint)RecMod.DIG_RESET);
            WriteByte((uint)RegAddr.REC_MOD, (uint)RecMod.ACTIVE);
        }

        public void WriteConfiguration(NeuropixelsV1Configuration config, bool performReadCheck = false)
        {
            if (config.Channels.ToList().GetRange(192, 192).Any(c => c.Bank == Channel.ElectrodeBank.TWO))
            {
                throw new ArgumentException("Electrode selection is out of bounds. Only bank 0 and 1 are valid for channels in range 192..383.", nameof(config));
            }

            // Turn on calibration if necessary
            if (config.Mode != NeuropixelsV1Configuration.OperationMode.RECORD)
            {
                switch (config.Mode)
                {
                    case NeuropixelsV1Configuration.OperationMode.CALIBRATE_ADCS:
                        WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.OSC_ACTIVE_AND_ADC_CAL);
                        WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_CALIBRATE);
                        break;
                    case NeuropixelsV1Configuration.OperationMode.CALIBRATE_CHANNELS:
                        WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.OSC_ACTIVE_AND_CH_CAL);
                        WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_CALIBRATE);
                        break;
                    case NeuropixelsV1Configuration.OperationMode.CALIBRATE_PIXELS:
                        WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.OSC_ACTIVE_AND_PIX_CAL);
                        WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_CALIBRATE);
                        break;
                    case NeuropixelsV1Configuration.OperationMode.DIGITAL_TEST:
                        WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_DIG_TEST);
                        break;
                }
            }

            // Shank configuration
            // NB: ASIC bug, read_check on SR_CHAIN1 ignored
            WriteShiftRegister((uint)RegAddr.SR_CHAIN1, GenerateShankBits(config), false);

            // Gain and ADC corrections
            WriteLFPGainCorrections(config);
            WriteAPGainCorrections(config);
            WriteADCCorrections(config);
            ConfigProbeSN = config.ConfigProbeSN;

            // Base configurations
            var base_configs = GenerateBaseBits(config);
            WriteShiftRegister((uint)RegAddr.SR_CHAIN2, base_configs[0], performReadCheck);
            WriteShiftRegister((uint)RegAddr.SR_CHAIN3, base_configs[1], performReadCheck);

            // Configuration has been uploaded
            config.RefreshNeeded = false;
        }


        public async Task<int> WriteConfigurationAsync(NeuropixelsV1Configuration config, IProgress<int> progress, bool performReadCheck = false)
        {
            return await Task.Run(() =>
            {
                if (config.Channels.ToList().GetRange(192, 192).Any(c => c.Bank == Channel.ElectrodeBank.TWO))
                {
                    throw new ArgumentException("Electrode selection is out of bounds. Only bank 0 and 1 are valid for channels in range 192..383.", nameof(config));
                }

                // Turn on calibration if necessary
                if (config.Mode != NeuropixelsV1Configuration.OperationMode.RECORD)
                {
                    switch (config.Mode)
                    {
                        case NeuropixelsV1Configuration.OperationMode.CALIBRATE_ADCS:
                            WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.OSC_ACTIVE_AND_ADC_CAL);
                            WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_CALIBRATE);
                            break;
                        case NeuropixelsV1Configuration.OperationMode.CALIBRATE_CHANNELS:
                            WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.OSC_ACTIVE_AND_CH_CAL);
                            WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_CALIBRATE);
                            break;
                        case NeuropixelsV1Configuration.OperationMode.CALIBRATE_PIXELS:
                            WriteByte((uint)RegAddr.CAL_MOD, (uint)CalMod.OSC_ACTIVE_AND_PIX_CAL);
                            WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_CALIBRATE);
                            break;
                        case NeuropixelsV1Configuration.OperationMode.DIGITAL_TEST:
                            WriteByte((uint)RegAddr.OP_MODE, (uint)Operation.RECORD_AND_DIG_TEST);
                            break;
                    }
                }
                progress.Report(10);

                // Shank configuration
                // NB: ASIC bug, read_check on SR_CHAIN1 ignored
                WriteShiftRegister((uint)RegAddr.SR_CHAIN1, GenerateShankBits(config), false);
                progress.Report(30);

                // Gain and ADC corrections
                WriteLFPGainCorrections(config);
                progress.Report(50);
                WriteAPGainCorrections(config);
                progress.Report(60);
                WriteADCCorrections(config);
                progress.Report(70);
                ConfigProbeSN = config.ConfigProbeSN;

                // Base configurations
                var base_configs = GenerateBaseBits(config);
                WriteShiftRegister((uint)RegAddr.SR_CHAIN2, base_configs[0], performReadCheck);
                progress.Report(80);
                WriteShiftRegister((uint)RegAddr.SR_CHAIN3, base_configs[1], performReadCheck);
                progress.Report(100);

                // Configuration has been uploaded
                config.RefreshNeeded = false;

                return 100;
            });
        }

        // Convert Channels into BitArray
        private static BitArray GenerateShankBits(NeuropixelsV1Configuration config)
        {
            // Default
            var shank_config = new BitArray(SHANK_CONFIG_BITS, false);

            // If external reference is used by any channel
            shank_config[SHANK_BIT_EXT1] = config.Channels.Where(ch => ch.Reference == Channel.Ref.EXTERNAL && ch.ElectrodeNumber % 2 == 1).Any();
            shank_config[SHANK_BIT_EXT2] = config.Channels.Where(ch => ch.Reference == Channel.Ref.EXTERNAL && ch.ElectrodeNumber % 2 == 0).Any();

            // If tip reference is used by any channel
            shank_config[SHANK_BIT_TIP1] = config.Channels.Where(ch => ch.Reference == Channel.Ref.TIP && ch.ElectrodeNumber % 2 == 1).Any();
            shank_config[SHANK_BIT_TIP2] = config.Channels.Where(ch => ch.Reference == Channel.Ref.TIP && ch.ElectrodeNumber % 2 == 0).Any();

            // If internal reference is used by any channel
            var refs = BankToIntRef.Values.ToArray();
            shank_config[refs[0]] = false;
            shank_config[refs[1]] = false;
            shank_config[refs[2]] = false;

            var b = config.Channels.Where(ch => ch.Reference == Channel.Ref.INTERNAL).Any();
            shank_config[refs[(int)config.Channels[INTERNAL_REF_CHANNEL].Bank]] = b;

            // Update active channels
            for (int i = 0; i < config.Channels.Length; i++)
            {
                // Reference bits always remain zero
                if (i == INTERNAL_REF_CHANNEL)
                {
                    continue;
                }

                var e = config.Channels[i].ElectrodeNumber;
                if (e != null)
                {
                    int bit_idx = e % 2 == 0 ?
                        485 + ((int)e / 2) : // even electrode
                        482 - ((int)e / 2);  // odd electrode
                    shank_config[bit_idx] = true;
                }
            }

            return shank_config;
        }

        // Convert Channels & ADCs into BitArray
        private static BitArray[] GenerateBaseBits(NeuropixelsV1Configuration config)
        {

            // MSB [Full, standby, LFPGain(3 downto 0), APGain(3 downto0)] LSB
            BitArray[] base_configs = { new BitArray(BASE_CONFIG_BITS, false),   // Ch 0, 2, 4, ...
                                        new BitArray(BASE_CONFIG_BITS, false) }; // Ch 1, 3, 5, ...

            // Channels section
            for (int i = 0; i < config.Channels.Length; i++)
            {
                var config_idx = i % 2;

                // References
                var ref_idx = config_idx == 0 ?
                    (382 - i) / 2 * 3 :
                    (383 - i) / 2 * 3;

                base_configs[config_idx][ref_idx + (int)config.Channels[i].Reference] = true;

                // Gains, standby, and filter
                var ch_opts_idx = PROBE_SRBASECONFIG_BIT_GAINBASE + ((i - config_idx) * 4);

                base_configs[config_idx][ch_opts_idx + 0] = Gains[config.Channels[i].APGain][0];
                base_configs[config_idx][ch_opts_idx + 1] = Gains[config.Channels[i].APGain][1];
                base_configs[config_idx][ch_opts_idx + 2] = Gains[config.Channels[i].APGain][2];

                base_configs[config_idx][ch_opts_idx + 3] = Gains[config.Channels[i].LFPGain][0];
                base_configs[config_idx][ch_opts_idx + 4] = Gains[config.Channels[i].LFPGain][1];
                base_configs[config_idx][ch_opts_idx + 5] = Gains[config.Channels[i].LFPGain][2];

                base_configs[config_idx][ch_opts_idx + 6] = config.Channels[i].Standby;
                base_configs[config_idx][ch_opts_idx + 7] = config.Channels[i].APFilter; // Correct? 

            }

            int k = 0;
            foreach (var adc in config.ADCs)
            {
                if (adc.CompP < 0 || adc.CompP > 0x1F)
                {
                    throw new ArgumentOutOfRangeException(String.Format("ADC calibration parameter CompP value of {0} is invalid.", adc.CompP));
                }

                if (adc.CompN < 0 || adc.CompN > 0x1F)
                {
                    throw new ArgumentOutOfRangeException(String.Format("ADC calibration parameter CompN value of {0} is invalid.", adc.CompN));
                }

                if (adc.Cfix < 0 || adc.Cfix > 0xF)
                {
                    throw new ArgumentOutOfRangeException(String.Format("ADC calibration parameter Cfix value of {0} is invalid.", adc.Cfix));
                }

                if (adc.Slope < 0 || adc.Slope > 0x7)
                {
                    throw new ArgumentOutOfRangeException(String.Format("ADC calibration parameter Slope value of {0} is invalid.", adc.Slope));
                }

                if (adc.Coarse < 0 || adc.Coarse > 0x3)
                {
                    throw new ArgumentOutOfRangeException(String.Format("ADC calibration parameter Coarse value of {0} is invalid.", adc.Coarse));
                }

                if (adc.Fine < 0 || adc.Fine > 0x3)
                {
                    throw new ArgumentOutOfRangeException(String.Format("ADC calibration parameter Fine value of {0} is invalid.", adc.Fine));
                }

                var config_idx = k % 2;
                int d = k++ / 2;

                int comp_off = 2406 - 42 * (d / 2) + (d % 2) * 10;
                int slope_off = comp_off + 20 + (d % 2);

                BitArray comp_p = new BitArray(new byte[] { (byte)adc.CompP });
                BitArray comp_n = new BitArray(new byte[] { (byte)adc.CompN });
                BitArray cfix = new BitArray(new byte[] { (byte)adc.Cfix });
                BitArray slope = new BitArray(new byte[] { (byte)adc.Slope });
                BitArray coarse = (new BitArray(new byte[] { (byte)adc.Coarse }));
                BitArray fine = new BitArray(new byte[] { (byte)adc.Fine });

                base_configs[config_idx][comp_off + 0] = comp_p[0];
                base_configs[config_idx][comp_off + 1] = comp_p[1];
                base_configs[config_idx][comp_off + 2] = comp_p[2];
                base_configs[config_idx][comp_off + 3] = comp_p[3];
                base_configs[config_idx][comp_off + 4] = comp_p[4];

                base_configs[config_idx][comp_off + 5] = comp_n[0];
                base_configs[config_idx][comp_off + 6] = comp_n[1];
                base_configs[config_idx][comp_off + 7] = comp_n[2];
                base_configs[config_idx][comp_off + 8] = comp_n[3];
                base_configs[config_idx][comp_off + 9] = comp_n[4];

                base_configs[config_idx][slope_off + 0] = slope[0];
                base_configs[config_idx][slope_off + 1] = slope[1];
                base_configs[config_idx][slope_off + 2] = slope[2];

                base_configs[config_idx][slope_off + 3] = fine[0];
                base_configs[config_idx][slope_off + 4] = fine[1];

                base_configs[config_idx][slope_off + 5] = coarse[0];
                base_configs[config_idx][slope_off + 6] = coarse[1];

                base_configs[config_idx][slope_off + 7] = cfix[0];
                base_configs[config_idx][slope_off + 8] = cfix[1];
                base_configs[config_idx][slope_off + 9] = cfix[2];
                base_configs[config_idx][slope_off + 10] = cfix[3];

            }

            return base_configs;
        }

        // Bits go into the shift registers MSB first
        // This creates a *bit-reversed* byte array from a bit array
        private static byte[] BitArrayToBytes(BitArray bits)
        {
            if (bits.Length == 0)
            {
                throw new ArgumentException("Shift register data is empty", nameof(bits));
            }

            var bytes = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(bytes, 0);

            for (int i = 0; i < bytes.Length; i++)
            {
                // NB: http://graphics.stanford.edu/~seander/bithacks.html
                bytes[i] = (byte)((bytes[i] * 0x0202020202ul & 0x010884422010ul) % 1023);
            }

            return bytes;
        }

        private void WriteShiftRegister(uint sr_addr, BitArray data, bool read_check = false)
        {
            var bytes = BitArrayToBytes(data);

            var count = read_check ? 2 : 1;
            while (count-- > 0)
            {
                WriteByte((uint)RegAddr.SR_LENGTH1, (uint)bytes.Length % 0x100);
                WriteByte((uint)RegAddr.SR_LENGTH2, (uint)bytes.Length / 0x100);

                foreach (var b in bytes)
                {
                    WriteByte(sr_addr, b);
                }
            }

            if (read_check && ReadByte((uint)RegAddr.STATUS) != (uint)Status.SR_OK)
            {
                throw new WorkflowException("Shift register programming check failed.");
            }
        }

        private void WriteLFPGainCorrections(NeuropixelsV1Configuration config)
        {
            for (int i = 0; i < config.Channels.Length; i += 2)
            {
                var addr = (uint)Register.CHAN001_000_LFPGAIN + (uint)i / 2;
                var gain_fixed0 = (uint)(config.Channels[i].LFPGainCorrection * (1 << 14));
                var gain_fixed1 = (uint)(config.Channels[i + 1].LFPGainCorrection * (1 << 14));
                var val = gain_fixed1 << 16 | gain_fixed0;
                WriteRegister(addr, val);
            }
        }

        private void WriteAPGainCorrections(NeuropixelsV1Configuration config)
        {
            for (int i = 0; i < config.Channels.Length; i += 2)
            {
                var addr = (uint)Register.CHAN001_000_APGAIN + (uint)i / 2;
                var gain_fixed0 = (uint)(config.Channels[i].APGainCorrection * (1 << 14));
                var gain_fixed1 = (uint)(config.Channels[i + 1].APGainCorrection * (1 << 14));
                var val = gain_fixed1 << 16 | gain_fixed0;
                WriteRegister(addr, val);
            }
        }

        private void WriteADCCorrections(NeuropixelsV1Configuration config)
        {
            for (int i = 0; i < config.ADCs.Length; i += 2)
            {
                var addr = (uint)Register.ADC01_00_OFF_THRESH + (uint)i / 2;
                var adc0 = (uint)config.ADCs[i].Offset << 10 | (uint)config.ADCs[i].Threshold;
                var adc1 = (uint)config.ADCs[i + 1].Offset << 10 | (uint)config.ADCs[i].Threshold;
                var val = adc1 << 16 | adc0;
                WriteRegister(addr, val);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public ulong? ConfigProbeSN
        {
            get
            {
                return ReadRegister((uint)Register.PROBE_SN_MSB) << 32 | ReadRegister((uint)Register.PROBE_SN_LSB);
            }
            private set
            {
                ulong val = 0;
                if (value != null)
                {
                    val = (ulong)value;
                }

                WriteRegister((uint)Register.PROBE_SN_LSB, (uint)(val & 0x00000000FFFFFFFF));
                WriteRegister((uint)Register.PROBE_SN_MSB, (uint)(val >> 32 & 0x00000000FFFFFFFF));
            }
        }

        #region Hardware types

        public enum RegAddr : uint
        {
            OP_MODE = 0X00,
            REC_MOD = 0X01,
            CAL_MOD = 0X02,
            TEST_CONFIG1 = 0X03,
            TEST_CONFIG2 = 0X04,
            TEST_CONFIG3 = 0X05,
            TEST_CONFIG4 = 0X06,
            TEST_CONFIG5 = 0X07,
            STATUS = 0X08,
            SYNC = 0X09,
            SR_CHAIN5 = 0X0A,
            SR_CHAIN4 = 0X0B,
            SR_CHAIN3 = 0X0C,
            SR_CHAIN2 = 0X0D,
            SR_CHAIN1 = 0X0E,
            SR_LENGTH2 = 0X0F,
            SR_LENGTH1 = 0X10,
            SOFT_RESET = 0X11,
        };

        // Probe operation flags
        [Flags]
        public enum Operation : uint
        {
            TEST = 1 << 3, // Enable Test mode
            DIG_TEST = 1 << 4, // Enable Digital Test mode
            CALIBRATE = 1 << 5, // Enable calibration mode
            RECORD = 1 << 6, // Enable recording mode
            POWER_DOWN = 1 << 7, // Enable power down mode

            // Useful combinations
            RECORD_AND_DIG_TEST = RECORD | DIG_TEST,
            RECORD_AND_CALIBRATE = RECORD | CALIBRATE,
        };

        // Record modification flags
        [Flags]
        public enum RecMod : uint
        {
            DIG_AND_CH_RESET = 0,
            RESET_ALL = 1 << 5, // 1 = Set analog SR chains to default values
            DIG_NRESET = 1 << 6, // 0 = Reset the MUX, ADC, and PSB counter, 1 = Disable reset
            CH_NRESET = 1 << 7, // 0 = Reset channel pseudo-registers, 1 = Disable reset

            // Useful combinations
            SR_RESET = RESET_ALL | CH_NRESET | DIG_NRESET,
            DIG_RESET = CH_NRESET, // Yes, this is actually correct
            CH_RESET = DIG_NRESET, // Yes, this is actually correct
            ACTIVE = DIG_NRESET | CH_NRESET,
        };

        // Calibration modification flags
        [Flags]
        public enum CalMod : uint
        {
            CAL_OFF = 0,
            OSC_ACTIVE = 1 << 4, // 0 = external osc inactive, 1 = activate the external calibration oscillator
            ADC_CAL = 1 << 5, // Enable ADC calibration
            CH_CAL = 1 << 6, // Enable channel gain calibration
            PIX_CAL = 1 << 7, // Enable pixel + channel gain calibration

            // Useful combinations
            OSC_ACTIVE_AND_ADC_CAL = OSC_ACTIVE | ADC_CAL,
            OSC_ACTIVE_AND_CH_CAL = OSC_ACTIVE | CH_CAL,
            OSC_ACTIVE_AND_PIX_CAL = OSC_ACTIVE | PIX_CAL,

        };

        // Status check flags
        [Flags]
        public enum Status : uint
        {
            SR_OK = 1 << 7  // Indicates the SR chain comparison is OK
        }

        public static readonly Dictionary<Channel.Gain, bool[]> Gains = new Dictionary<Channel.Gain, bool[]>
        {
            {Channel.Gain.x50,      new bool[]{false, false, false} },
            {Channel.Gain.x125,     new bool[]{true, false, false} },
            {Channel.Gain.x250,     new bool[]{false, true, false} },
            {Channel.Gain.x500,     new bool[]{true, true, false} },
            {Channel.Gain.x1000,    new bool[]{false, false, true} },
            {Channel.Gain.x1500,    new bool[]{true, false, true} },
            {Channel.Gain.x2000,    new bool[]{false, true, true} },
            {Channel.Gain.x3000,    new bool[]{true, true, true} },
        };

        public static readonly Dictionary<Channel.ElectrodeBank, int> BankToIntRef = new Dictionary<Channel.ElectrodeBank, int>
        {
            {Channel.ElectrodeBank.ZERO,     191 },
            {Channel.ElectrodeBank.ONE,      575 },
            {Channel.ElectrodeBank.TWO,      959 },
        };

        #endregion

    }
}
