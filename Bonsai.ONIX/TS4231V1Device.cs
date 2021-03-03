using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Triad TS4231 optical to digital converter array for V1 SteamVR base stations.")]
    public class TS4231V1Device : ONIFrameReader<TS4231V1DataFrame>
    {
        enum Register
        {
            ENABLE = 0,
            ENVMARGIN,
        }

        public TS4231V1Device() : base(ONIXDevices.ID.TS4231V1ARR) { }

        protected override IObservable<TS4231V1DataFrame> Process(IObservable<oni.Frame> source)
        {
            return source.Select(f => { return new TS4231V1DataFrame(f); });
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


    }
}
