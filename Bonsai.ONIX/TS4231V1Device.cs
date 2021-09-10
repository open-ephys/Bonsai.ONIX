using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Triad TS4231 optical to digital converter array for V1 SteamVR base stations.")]
    public class TS4231V1Device : ONIFrameReader<TS4231V1DataFrame, ushort>
    {
        enum Register
        {
            ENABLE = 0,
            ENVMARGIN,
        }

        public TS4231V1Device() : base(ONIXDevices.ID.TS4231V1Array) { }

        protected override IObservable<TS4231V1DataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new TS4231V1DataFrame(f); });
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
    }
}
