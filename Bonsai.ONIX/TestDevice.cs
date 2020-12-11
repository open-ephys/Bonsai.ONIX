using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("ONI Test device.")]
    public class TestDevice : ONIFrameReaderDeviceBuilder<TestDataFrame>
    {
        public enum Register
        {
            NULLPARAM = 0,
            MESSAGE,
        }

        public TestDevice() : base(ONIXDevices.ID.TEST) { }

        public override IObservable<TestDataFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new TestDataFrame(f); });
        }

        [Category("Acquisition")]
        [Description("16-bit word to send as frame payload.")]
        public short? Message
        {
            get
            {
                return (short?)Controller?.ReadRegister(DeviceIndex.SelectedIndex, (uint)Register.MESSAGE);
            }
            set
            {
                if (Controller != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (uint)Register.MESSAGE, (uint)value);
                }
            }
        }
    }
}
