using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Bonsai.ONI
{
    [Description("Acquires data from the twelve 16-bit analog inputs on the Open Ephys FMC Host. Used in concert with FMCAnalogOutputDevice.")]
    public class FMCAnalogInputDevice : ONIFrameReaderDeviceBuilder<AnalogInputDataFrame>
    {
        // Control registers (see oedevices.h)
        public enum Register
        {
            NULLPARM = 0,
            CHDIR = 1,
            CH00INRANGE = 2,
            CH01INRANGE = 3,
            CH02INRANGE = 4,
            CH03INRANGE = 5,
            CH04INRANGE = 6,
            CH05INRANGE = 7,
            CH06INRANGE = 8,
            CH07INRANGE = 9,
            CH08INRANGE = 10,
            CH09INRANGE = 11,
            CH10INRANGE = 12,
            CH11INRANGE = 13,
        }

        public enum VoltageRange
        {
            [Description("+/-10.0 V")]
            TenVolts = 0,
            [Description("+/-2.5 V")]
            TwoPointFiveVolts = 1,
            [Description("+/-5.0 V")]
            FiveVolts,
        }

        void SetVoltageRange(Register channel, VoltageRange? range)
        {
            if (Controller != null)
            {
                var val = range ?? VoltageRange.TenVolts;
                Controller.WriteRegister(DeviceIndex.SelectedIndex,
                                         (uint)channel,
                                         (uint)val);
            }
        }

        VoltageRange? GetVoltageRange(Register channel)
        {
            return (VoltageRange?)Controller?.ReadRegister(DeviceIndex.SelectedIndex, (uint)channel);
        }

        public FMCAnalogInputDevice() : base(oni.Device.DeviceID.FMCANALOG1R3) { }

        public override IObservable<AnalogInputDataFrame> Process(IObservable<oni.Frame> source)
        {
            var data_block = new AnalogInputDataBlock(NumChannels, BlockSize);

            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Where(f =>
                {
                    return data_block.FillFromFrame(f);
                })
                .Select(f =>
                {
                    var sample = new AnalogInputDataFrame(data_block, ClockHz);
                    data_block = new AnalogInputDataBlock(NumChannels, BlockSize);
                    return sample;
                });
        }

        [Category("Acquisition")]
        [Range(1, 10000)]
        [Description("The size of data blocks, in samples, that are propagated as events in the observable sequence.")]
        public int BlockSize { get; set; } = 250;

        [System.Xml.Serialization.XmlIgnore]
        [Category("Configuration")]
        [Description("Number of channels begin used.")]
        public int NumChannels { get; private set; } = 12;

        [Category("Configuration")]
        [Description("The input voltage range of channel 0.")]
        public VoltageRange? InputRange00
        {
            get
            {
                return GetVoltageRange(Register.CH00INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH00INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 1.")]
        public VoltageRange? InputRange01
        {
            get
            {
                return GetVoltageRange(Register.CH01INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH01INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 2.")]
        public VoltageRange? InputRange02
        {
            get
            {
                return GetVoltageRange(Register.CH02INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH02INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 3.")]
        public VoltageRange? InputRange03
        {
            get
            {
                return GetVoltageRange(Register.CH03INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH03INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 4.")]
        public VoltageRange? InputRange04
        {
            get
            {
                return GetVoltageRange(Register.CH04INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH04INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 5.")]
        public VoltageRange? InputRange05
        {
            get
            {
                return GetVoltageRange(Register.CH05INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH05INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 6.")]
        public VoltageRange? InputRange06
        {
            get
            {
                return GetVoltageRange(Register.CH06INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH06INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 7.")]
        public VoltageRange? InputRange07
        {
            get
            {
                return GetVoltageRange(Register.CH07INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH07INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 8.")]
        public VoltageRange? InputRange08
        {
            get
            {
                return GetVoltageRange(Register.CH08INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH08INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 9.")]
        public VoltageRange? InputRange09
        {
            get
            {
                return GetVoltageRange(Register.CH09INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH09INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 10.")]
        public VoltageRange? InputRange10
        {
            get
            {
                return GetVoltageRange(Register.CH10INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH10INRANGE, value);
            }
        }

        [Category("Configuration")]
        [Description("The input voltage range of channel 11.")]
        public VoltageRange? InputRange11
        {
            get
            {
                return GetVoltageRange(Register.CH11INRANGE);
            }
            set
            {
                SetVoltageRange(Register.CH11INRANGE, value);
            }
        }

    }
}
