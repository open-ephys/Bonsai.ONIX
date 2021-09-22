using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Acquires data from the twelve 14-bit analog inputs on the Open Ephys Host board. " +
        "Optionally, sends data to the 16-bit analog outputs on the Open Ephys Host bpard, if those " +
        "channels are selected to be outputs. The output range is fixed to +/-10V. Usually these signals" +
        "are accessed via the ONIX breakout board.")]
    public class AnalogIODevice : ONIFrameReaderAndWriter<Arr, AnalogInputDataFrame, short>
    {
        enum Register
        {
            ENABLE = 0,
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

        private readonly float[] scale;

        public AnalogIODevice() : base(ONIXDevices.ID.BreakoutAnalogIO)
        {
            scale = Enumerable.Repeat((float)0.000305, AnalogInputDataFrame.NumberOfChannels).ToArray();
        }

        protected override IObservable<AnalogInputDataFrame> Process(IObservable<ONIManagedFrame<short>> source)
        {
            return Observable
                .Return(scale.Copy())
                .CombineLatest(source.Buffer(BlockSize), (s, block) => { return new AnalogInputDataFrame(block, s, DataType); });
        }

        [ONIXDeviceID(ONIXDevices.ID.BreakoutAnalogIO)]
        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        // TODO: The order of data in the matrix is reverse of the channel index.
        // m[11] => channel 0, etc.
        protected override void Write(ONIContextTask ctx, Arr input)
        {
            var inputMatrix = input.GetMat();

            // Check dims
            var size = inputMatrix.Rows * inputMatrix.Cols;
            if (size % AnalogInputDataFrame.NumberOfChannels != 0)
            {
                throw new IndexOutOfRangeException("Source must contain a multiple of 12 elements.");
            }

            if (DataType == AnalogDataType.S16 && inputMatrix.Depth == Depth.S16)
            {
                var data = new Mat(inputMatrix.Size, Depth.U16, 1);
                CV.ConvertScale(inputMatrix, data, 1, 32768);
                ctx.Write((uint)DeviceAddress.Address, data.Data, 2 * size);
            }
            else if (DataType == AnalogDataType.Volts && (inputMatrix.Depth == Depth.F32 || inputMatrix.Depth == Depth.F64))
            {
                var data = new Mat(inputMatrix.Size, Depth.U16, 1);
                CV.ConvertScale(inputMatrix, data, 3276.75, 32768);
                ctx.Write((uint)DeviceAddress.Address, data.Data, 2 * size);
            }
            else
            {
                throw new Bonsai.WorkflowRuntimeException("Source element depth must Depth.S16 when " +
                    "DataType is S16 and either Depth.F32 or Depth.F64 when Datatype is Volts.");
            }
        }

        [Category("Configuration")]
        [Range(1, 1e5)]
        [Description("The size of data blocks, in samples, that are propagated as events in the observable sequence.")]
        public int BlockSize { get; set; } = 100;

        public enum AnalogDataType
        {
            S16,
            Volts
        }

        [Category("Acquisition")]
        [Range(1, 1e5)]
        [Description("The format of the analog data consumed and produced by this node.\n" +
            " - S16: raw 16-bit signed integer conversion results or DAC values.\n" +
            " - Volts: 32-bit floating-point voltages.")]
        public AnalogDataType DataType { get; set; } = AnalogDataType.S16;

        [Category("Configuration")]
        [Description("Enable the input data stream.")]
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
        [Description("The input voltage range of channel 0.")]
        public VoltageRange InputRange00
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
        public VoltageRange InputRange01
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
        public VoltageRange InputRange02
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
        public VoltageRange InputRange03
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
        public VoltageRange InputRange04
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
        public VoltageRange InputRange05
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
        public VoltageRange InputRange06
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
        public VoltageRange InputRange07
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
        public VoltageRange InputRange08
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
        public VoltageRange InputRange09
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
        public VoltageRange InputRange10
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
        public VoltageRange InputRange11
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

        [Category("Acquisition")]
        [Description("The direction of channel 0.")]
        public InputOutput Direction00
        {
            get
            {
                return GetIO(0);
            }
            set
            {
                SetIO(0, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 1.")]
        public InputOutput Direction01
        {
            get
            {
                return GetIO(1);
            }
            set
            {
                SetIO(1, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 2.")]
        public InputOutput Direction02
        {
            get
            {
                return GetIO(2);
            }
            set
            {
                SetIO(2, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 3.")]
        public InputOutput Direction03
        {
            get
            {
                return GetIO(3);
            }
            set
            {
                SetIO(3, value);
            }
        }


        [Category("Acquisition")]
        [Description("The direction of channel 4.")]
        public InputOutput Direction04
        {
            get
            {
                return GetIO(4);
            }
            set
            {
                SetIO(4, value);
            }
        }


        [Category("Acquisition")]
        [Description("The direction of channel 5.")]
        public InputOutput Direction05
        {
            get
            {
                return GetIO(5);
            }
            set
            {
                SetIO(5, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 6.")]
        public InputOutput Direction06
        {
            get
            {
                return GetIO(6);
            }
            set
            {
                SetIO(6, value);
            }
        }
        [Category("Acquisition")]
        [Description("The direction of channel 7.")]
        public InputOutput Direction07
        {
            get
            {
                return GetIO(7);
            }
            set
            {
                SetIO(7, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 8.")]
        public InputOutput Direction08
        {
            get
            {
                return GetIO(8);
            }
            set
            {
                SetIO(8, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 9.")]
        public InputOutput Direction09
        {
            get
            {
                return GetIO(9);
            }
            set
            {
                SetIO(9, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 10.")]
        public InputOutput Direction10
        {
            get
            {
                return GetIO(10);
            }
            set
            {
                SetIO(10, value);
            }
        }

        [Category("Acquisition")]
        [Description("The direction of channel 11.")]
        public InputOutput Direction11
        {
            get
            {
                return GetIO(11);
            }
            set
            {
                SetIO(11, value);
            }
        }

        public enum InputOutput
        {
            Input = 0,
            Output = 1
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


        void SetVoltageRange(Register channel, VoltageRange range)
        {
            WriteRegister((uint)channel, (uint)range);

            switch (range)
            {
                case VoltageRange.TwoPointFiveVolts:
                    scale[(uint)channel - 2] = (float)7.6250e-05;
                    break;
                case VoltageRange.FiveVolts:
                    scale[(uint)channel - 2] = (float)1.5250e-04;
                    break;
                case VoltageRange.TenVolts:
                    scale[(uint)channel - 2] = (float)0.000305;
                    break;
            }
        }

        VoltageRange GetVoltageRange(Register channel)
        {
            var range = (VoltageRange)ReadRegister((uint)channel);

            switch (range)
            {
                case VoltageRange.TwoPointFiveVolts:
                    scale[(uint)channel - 2] = (float)7.6250e-05;
                    break;
                case VoltageRange.FiveVolts:
                    scale[(uint)channel - 2] = (float)1.5250e-04;
                    break;
                case VoltageRange.TenVolts:
                    scale[(uint)channel - 2] = (float)0.000305;
                    break;
            }

            return range;
        }

        uint io_reg = 0;

        void SetIO(int channel, InputOutput io)
        {
            io_reg = (io_reg & ~((uint)1 << channel)) | ((uint)(io) << channel);
            WriteRegister((uint)Register.CHDIR, io_reg);
        }

        InputOutput GetIO(int channel)
        {
            var io_reg = ReadRegister((int)Register.CHDIR);
            return (InputOutput)((io_reg >> channel) & 1);
        }
    }
}
