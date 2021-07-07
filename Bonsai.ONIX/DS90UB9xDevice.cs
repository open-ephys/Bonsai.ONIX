using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    public class DS90UB9xDevice : ONIFrameReader<RawDataFrame, ushort>
    {
        public DS90UB9xDevice() : base(ONIXDevices.ID.DS90UB9X) { }

        protected override IObservable<RawDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new RawDataFrame(f); });
        }


        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        [Category("Configuration")]
        [Description("Frame width in samples (pixels)")]
        public uint DataSize
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.READSZ);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.READSZ, value);
            }
        }

        [Category("Configuration")]
        [Description("Frame start trigger mode")]
        public DS90UB9xConfiguration.TriggerMode TriggerMode
        {
            get
            {
                return (DS90UB9xConfiguration.TriggerMode)ReadRegister((uint)DS90UB9xConfiguration.Register.TRIGGER);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.TRIGGER, (uint)value);
            }
        }

        [Category("Configuration")]
        [Description("Trigger Offset. Start frame N samples after trigger.")]
        public uint TriggerOffset
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.TRIGGER_OFFSET);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.TRIGGER_OFFSET, value);
            }
        }

        [Category("Configuration")]
        [Description("Pixel gate. Ignore pixels when gate not active.")]
        public DS90UB9xConfiguration.PixelGate PixelGate
        {
            get
            {
                return (DS90UB9xConfiguration.PixelGate)ReadRegister((uint)DS90UB9xConfiguration.Register.PIXEL_GATE);
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.PIXEL_GATE, (uint)value);
            }
        }

        [Category("Configuration")]
        [Description("Invlude synchronozation bits in samples.")]
        public bool SyncBits
        {
            get
            {
                return ReadRegister((uint)DS90UB9xConfiguration.Register.SYNC_BITS) > 0;
            }
            set
            {
                WriteRegister((uint)DS90UB9xConfiguration.Register.SYNC_BITS, value ? (uint)1 : 0);
            }
        }
    }
}
