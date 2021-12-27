using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    [Source]
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONISink<TSource> : ONIDevice
    {
        public IObservable<TSource> Process(IObservable<TSource> source)
        {
            return Observable.Using(
                cancellationToken => ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot),
                (contextDisposable, cancellationToken) =>
                {
                    return ONIXDeviceDescriptor.IsValid(ID, DeviceAddress)
                        ? Task.FromResult(source
                            .Do(input => { OnNext(contextDisposable.Context, input); })
                            .Finally(() => OnFinally(contextDisposable.Context)))
                        : throw new WorkflowException("Selected device address is invalid.");
                }
            );
        }

        protected abstract void OnNext(ONIContextTask ctx, TSource source);

        protected virtual void OnFinally(ONIContextTask ctx) { }
    }
}
