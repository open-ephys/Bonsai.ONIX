using System;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONIRegisterOnlyDeviceBuilder : ONIFrameReaderDeviceBuilder<oni.Frame>
    {
        public ONIRegisterOnlyDeviceBuilder(ONIXDevices.ID dev_id) : base(dev_id) { }

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
