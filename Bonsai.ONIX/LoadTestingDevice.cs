using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Variable load testing device and latency tester.")]
    public class LoadTestingDevice : ONIFrameReaderAndWriter<ulong, LoadTestingDataFrame, ushort>
    {
        private uint[] writeArray;

        enum Register
        {
            ENABLE = 0,
            CLK_DIV = 1,  // Heartbeat clock divider ratio. Default results in 10 Hz heartbeat.
                          // Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
            DT0H16_WORDS = 3, // Number of repetitions of 16-bit unsigned integer 42 sent with each frame. 
                              // Note: max here depends of CLK_HZ and CLK_DIV. There needs to be enough clock
                              // cycles to push the data at the requested CLK_HZ. Specifically,
                              // CLK_HZ / CLK_DIV >= TX16_WORDS + 9. Going above this will result in 
                              // decreased bandwidth as samples will be skipped.
            HTOD32_WORDS = 4  // Number of 32-bit words in a write-frame. All write frame data is ignored except
                              // the first 64-bits, which are looped back into the device to host data frame for   
                              // testing loop latency. This value must be at least 2.
        }

        public LoadTestingDevice() : base(ONIXDevices.ID.LoadTest) { }

        protected override IObservable<LoadTestingDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new LoadTestingDataFrame(f); });
        }

        [ONIXDeviceID(ONIXDevices.ID.LoadTest)]
        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        protected override void Write(ONIContextTask ctx, ulong input)
        {
            writeArray[0] = (uint)(input & uint.MaxValue);
            writeArray[1] = (uint)(input >> 32);
            ctx.Write((uint)DeviceAddress.Address, writeArray);
        }

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
        [Description("Number of repetitions of the 16-bit unsigned integer 42 sent with each read-frame.")]
        [Range(0, 10e6)]
        public uint ReceivedWords
        {
            get
            {
                return ReadRegister((int)Register.DT0H16_WORDS);
            }
            set
            {
                var max_size = ValidSize();
                var bounded = value > max_size ? max_size : value;
                WriteRegister((int)Register.DT0H16_WORDS, bounded);
            }
        }

        [Category("Configuration")]
        [Description("Number of repetitions of the 32-bit integer 42 sent with each write frame.")]
        [Range(0, 10e6)]
        public uint TransmittedWords
        {
            get
            {
                return ReadRegister((int)Register.HTOD32_WORDS);
            }
            set
            {
                writeArray = Enumerable.Repeat((uint)42, (int)(value + 2)).ToArray();
                WriteRegister((int)Register.HTOD32_WORDS, value);
            }
        }

        [Category("Acquisition")]
        [Description("Rate at frames are produced.")]
        public uint FrameHz
        {
            get
            {
                var val = ReadRegister((int)Register.CLK_DIV);
                return ReadRegister((int)Register.CLK_HZ) / val;
            }
            set
            {
                WriteRegister((int)Register.CLK_DIV, ReadRegister((int)Register.CLK_HZ) / value);
                var max_size = ValidSize();
                if (ReceivedWords > max_size)
                {
                    ReceivedWords = max_size;
                }
            }
        }

        // Assumes 8-byte timer
        uint ValidSize()
        {
            var clk_div = ReadRegister((int)Register.CLK_DIV);
            return clk_div - 4 - 10; // -10 is overhead hack
        }

    }
}