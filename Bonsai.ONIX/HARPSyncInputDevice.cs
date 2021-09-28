using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.HARPSyncInput)]
    [Description("Receives time input from a HARP synchronization source")]
    public class HARPSyncInputDevice : ONIFrameReader<HARPSyncInputDataFrame,ushort>
    {
        private enum Register
        {
            ENABLE = 0
        }

        protected override IObservable<HARPSyncInputDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new HARPSyncInputDataFrame(f); });
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

    }
}
