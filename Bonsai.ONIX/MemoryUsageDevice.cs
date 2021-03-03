using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Memory usage monitoring device")]
    public class MemoryUsageDevice : ONIFrameReader<MemoryUsageDataFrame>
    {
        enum Register
        {
            ENABLE = 0,
            CLK_DIV = 1,  // Update clock divider ratio.Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
            TOTAL_MEM = 3, //Total memory in 32bit words
        }

        public MemoryUsageDevice() : base(ONIXDevices.ID.MEMUSAGE) { }

        protected override IObservable<MemoryUsageDataFrame> Process(IObservable<oni.Frame> source)
        {
            var total_words = MemorySize;
            return source.Select(f => { return new MemoryUsageDataFrame(f, total_words); });
        }

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool Enable
        {
            get
            {
                return ReadRegister(DeviceAddress.Address, (uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister(DeviceAddress.Address, (uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        uint update_hz;
        [Category("Configuration")]
        [Description("Rate of memory usage measurements.")]
        [Range(0, 10e6)]
        public uint UpdateHz
        {
            get
            {
                var val = ReadRegister(DeviceAddress.Address, (int)Register.CLK_DIV);
                update_hz = ReadRegister(DeviceAddress.Address, (int)Register.CLK_HZ) / val;
                return update_hz;
            }
            set
            {
                update_hz = value;
                WriteRegister(DeviceAddress.Address,
                                            (int)Register.CLK_DIV,
                                            ReadRegister(DeviceAddress.Address, (int)Register.CLK_HZ) / update_hz);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [Category("Configuration")]
        [Description("Hardware buffer size in 32-bit words.")]
        public uint MemorySize
        {
            get
            {
                return ReadRegister(DeviceAddress.Address, (int)Register.TOTAL_MEM);
            }
        }
    }
}
