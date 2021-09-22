using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Memory usage monitoring device")]
    public class MemoryUsageDevice : ONIFrameReader<MemoryUsageDataFrame, ushort>
    {
        enum Register
        {
            ENABLE = 0,
            CLK_DIV = 1,  // Update clock divider ratio.Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
            TOTAL_MEM = 3, //Total memory in 32bit words
        }

        public MemoryUsageDevice() : base(ONIXDevices.ID.MemoryUsage) { }

        protected override IObservable<MemoryUsageDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            var total_words = MemorySize;
            return source.Select(f => { return new MemoryUsageDataFrame(f, total_words); });
        }

        [ONIXDeviceID(ONIXDevices.ID.MemoryUsage)]
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

        uint update_hz = 1;
        [Category("Configuration")]
        [Description("Rate of memory usage measurements.")]
        [Range(1, 10e6)]
        public uint UpdateHz
        {
            get
            {
                var val = ReadRegister((int)Register.CLK_DIV);
                if (val != 0)
                {
                    update_hz = ReadRegister((int)Register.CLK_HZ) / val;
                }
                return update_hz;
            }
            set
            {
                update_hz = value;
                WriteRegister((int)Register.CLK_DIV, ReadRegister((int)Register.CLK_HZ) / update_hz);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [Category("Configuration")]
        [Description("Hardware buffer size in 32-bit words.")]
        public uint MemorySize
        {
            get
            {
                return ReadRegister((int)Register.TOTAL_MEM);
            }
        }
    }
}
