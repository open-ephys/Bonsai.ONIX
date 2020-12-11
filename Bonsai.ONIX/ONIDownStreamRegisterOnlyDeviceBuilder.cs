using System;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONIDownStreamRegisterOnlyDeviceBuilder<TSource> : ONIDownStreamDeviceBuilder<TSource, TSource>
    {
        public ONIDownStreamRegisterOnlyDeviceBuilder(ONIXDevices.ID dev_id) : base(dev_id) { }

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
