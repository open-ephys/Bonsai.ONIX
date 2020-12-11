using System;

namespace Bonsai.ONIX
{
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONIFrameWriterDeviceBuilder<TSource> : ONIDownStreamDeviceBuilder<TSource, TSource>
    {
        public ONIFrameWriterDeviceBuilder(ONIXDevices.ID dev_id) : base(dev_id) { }

        public override abstract IObservable<TSource> Process(IObservable<TSource> source);
    }
}
