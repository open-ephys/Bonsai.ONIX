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
        public ONISink(ONIXDevices.ID deviceID) : base(deviceID) { }

        public IObservable<TSource> Process(IObservable<TSource> source)
        {
            return Observable.Using(
                cancellationToken => ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot),
                (contextDisposable, cancellationToken) =>
                {
                    var inTable = contextDisposable.Context.DeviceTable.TryGetValue((uint)DeviceAddress.Address, out var device);
                    if (!inTable || device.ID != (int)ID)
                    {
                        throw new WorkflowException("Selected device address is invalid.");
                    }

                    return Task.FromResult(source
                        .Do(input => { OnNext(contextDisposable.Context, input); })
                        .Finally(() => OnFinally(contextDisposable.Context))
                    );
                }
            );
        }

        protected abstract void OnNext(ONIContextTask ctx, TSource source);

        protected virtual void OnFinally(ONIContextTask ctx) { }
    }
}
