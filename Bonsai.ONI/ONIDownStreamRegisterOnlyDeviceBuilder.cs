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
    public abstract class ONIDownStreamRegisterOnlyDeviceBuilder<TSource> : ONIDownStreamDeviceBuilder<TSource, TSource>
    {
        public ONIDownStreamRegisterOnlyDeviceBuilder(oni.Device.DeviceID dev_id) : base(dev_id) { }

        public sealed override IObservable<TSource> Process(IObservable<TSource> source)
        {
            return source.Do(OnNext);
        }

        protected virtual void OnNext(TSource input)
        {
            // Nothing
        }
    }
}
