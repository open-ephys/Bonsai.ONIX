﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Memory usage monitoring device")]
    public class MemoryUsageDevice : ONIFrameReaderDeviceBuilder<MemoryUsageDataFrame>
    {
        enum Register
        {
            NULLPARM = 0,  // No command
            CLK_DIV = 1,  // Update clock divider ratio.Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
            TOTAL_MEM = 3, //Total memory in 32bit words
        }

        public MemoryUsageDevice() : base(ONIXDevices.ID.MEMUSAGE) { }

        public override IObservable<MemoryUsageDataFrame> Process(IObservable<oni.Frame> source)
        {
            var total_words = MemorySize;
            return source.Select(f => { return new MemoryUsageDataFrame(f, total_words); });
        }

        uint update_hz;
        [Category("Configuration")]
        [Description("Rate of memory usage measurements.")]
        [Range(0, 10e6)]
        public uint UpdateHz
        {
            get
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_DIV);
                update_hz = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_HZ) / val;
                return update_hz;
            }
            set
            {
                update_hz = value;
                WriteRegister(DeviceIndex.SelectedIndex,
                                            (int)Register.CLK_DIV,
                                            ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_HZ) / update_hz);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        [Category("Information")]
        [Description("Hardware buffer size in 32-bit words.")]
        public uint MemorySize
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.TOTAL_MEM);
            }
        }
    }
}