using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Bonsai.ONI
{
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONIRegisterOnlyDeviceBuilder : ONIFrameReaderDeviceBuilder<oni.Frame>
    {
        public ONIRegisterOnlyDeviceBuilder(oni.Device.DeviceID dev_id) : base(dev_id) { }

        public sealed override IObservable<oni.Frame> Process(IObservable<oni.Frame> source)
        {
            return source.Do(OnNext);
        }

        protected void OnNext(oni.Frame input)
        {
            // Nothing
        }
    }
}
