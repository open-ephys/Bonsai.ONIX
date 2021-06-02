using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    public class RawDevice : ONIFrameReader<RawDataFrame, ushort>
    {
        public RawDevice() : base(ONIXDevices.ID.Null) { }

        protected override IObservable<RawDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new RawDataFrame(f); });
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
