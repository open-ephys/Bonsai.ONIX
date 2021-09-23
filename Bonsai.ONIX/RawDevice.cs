using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(ONIXDevices.ID.Null)]
    public class RawDevice : ONIFrameReader<RawDataFrame, ushort>
    {
        public RawDevice() { }

        protected override IObservable<RawDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new RawDataFrame(f); });
        }

        private ONIDeviceAddress deviceAddress = new ONIDeviceAddress();

        public override ONIDeviceAddress DeviceAddress
        {
            get
            {
                return deviceAddress;
            }
            set
            {
                deviceAddress = value;
                RegisterIndex = 0;
                if (deviceAddress.Valid)
                {
                    using (var c = ONIContextManager.ReserveContext(deviceAddress.HardwareSlot))
                    {

                        ID = (ONIXDevices.ID)c.Context.DeviceTable[(uint)deviceAddress.Address].ID;
                    }
                }
            }
        } 

        [Category("Configuration")]
        [Description("Device type to acquire raw data frames from.")]
        [Externalizable(false)]
        [ReadOnly(true)]
            public ONIXDevices.ID DeviceType
            {
                get
                {
                    return ID;
                }
            }

        uint registerIndex;
        [Category("Acquisition")]
        [Description("Register index.")]
        public uint RegisterIndex
        {
            get
            {
                return registerIndex;
            }
            set
            {
                registerIndex = value;
                RegisterValue = ReadRegister(value);
            }
        }

        [Category("Acquisition")]
        [Description("Register value.")]
        public uint RegisterValue
        {
            get
            {
                return ReadRegister(registerIndex);
            }
            set
            {
                WriteRegister(registerIndex, value);
            }
        }
    }
}
