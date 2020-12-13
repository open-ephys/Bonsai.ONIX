using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Memory usage device")]
    public class MemoryUsageDevice : ONIFrameReaderDeviceBuilder<MemoryUsageDataFrame>
    {
        public enum Register
        {
            NULLPARM = 0,  // No command
            CLK_DIV = 1,  // Update clock divider ratio.Default results in 10 Hz heartbeat. Values less than CLK_HZ / 10e6 Hz will result in 1kHz.
            CLK_HZ = 2, // The frequency parameter, CLK_HZ, used in the calculation of CLK_DIV
        }

        public MemoryUsageDevice() : base(ONIXDevices.ID.MEMUSAGE) { }

        public override IObservable<MemoryUsageDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new MemoryUsageDataFrame(f, FrameClockHz, DataClockHz); });
        }

        uint update_hz;
        [Category("Configuration")]
        [Description("Rate at which memory usage updates.")]
        [Range(0, 10e6)]
        public uint BeatHz
        {
            get
            {
                if (Controller != null)
                {
                    var val = Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_DIV);
                    update_hz = Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_HZ) / val;
                    return update_hz;
                }
                else
                {
                    return update_hz;
                }
            }
            set
            {
                if (Controller != null)
                {
                    update_hz = value;
                    Controller.WriteRegister(DeviceIndex.SelectedIndex,
                                             (int)Register.CLK_DIV,
                                             Controller.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.CLK_HZ) / update_hz);
                }
            }
        }
    }
}
