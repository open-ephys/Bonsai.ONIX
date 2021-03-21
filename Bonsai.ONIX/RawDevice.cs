using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    public class RawDevice : ONIFrameReader<RawDeviceDataFrame, ushort>
    {
        public RawDevice() : base(ONIXDevices.ID.NULL) { }

        protected override IObservable<RawDeviceDataFrame> Process(IObservable<RawDataFrame<ushort>> source)
        {
            return source.Select(f => { return new RawDeviceDataFrame(f); });
        }

        [Category("Configuration")]
        [Description("Device type to acquire raw data frames from.")]
        public ONIXDevices.ID DeviceType
        {
            get
            {
                return ID;
            }
            set
            {
                if (ID != value)
                {
                    ID = value;
                    DeviceAddress = null;
                    FrameClockHz = null;
                    Hub = null;
                }
            }
        }
    }
}
