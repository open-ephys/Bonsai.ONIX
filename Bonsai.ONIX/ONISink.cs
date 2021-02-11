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
        public ONISink(ONIXDevices.ID dev_id) : base(dev_id) { }

        public IObservable<TSource> Process(IObservable<TSource> source)
        {
            return Observable.Using(
                cancellationToken => ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot),
                (cd, cancellationToken) =>
                {
                    var in_table = cd.Context.DeviceTable.TryGetValue(DeviceAddress.Address, out var device);
                    if (!in_table || device.ID != (int)ID)
                    {
                        throw new WorkflowException("Selected device address is invalid.");
                    }

                    return Task.FromResult(source.Do(input =>
                    {
                        Write(cd.Context, input);
                    }));
                }
            );
        }

        protected abstract void Write(ONIContextTask ctx, TSource source);
    }
}
