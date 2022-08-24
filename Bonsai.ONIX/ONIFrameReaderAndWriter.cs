using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public abstract class ONIFrameReaderAndWriter<TSource, TResult, TData> : ONIDevice where TData : unmanaged
    {
        public ONIFrameReaderAndWriter() : base() { }

        public IObservable<TResult> Generate()
        {
            var source = Observable.Create<ONIManagedFrame<TData>>(async observer =>
                {
                    var cd = await ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot);

                    var in_table = cd.Context.DeviceTable.TryGetValue((uint)DeviceAddress.Address, out var device);
                    if (!in_table || device.ID != (int)ID)
                    {
                        throw new WorkflowException("Selected device address is invalid.");
                    }

                    EventHandler<FrameReceivedEventArgs> frame_received = (sender, e) =>
                    {
                        if (e.Frame.DeviceAddress == DeviceAddress.Address)
                        {
                            ONIManagedFrame<TData> frame = new ONIManagedFrame<TData>(e.Frame);
                            observer.OnNext(frame);
                        }
                    };

                    cd.Context.FrameReceived += frame_received;
                    return Disposable.Create(() =>
                    {
                        cd.Context.FrameReceived -= frame_received;
                        cd.Dispose();
                    });
                });

            ulong frameOffset = FrameClockOffset;
            return Process(source, frameOffset);
        }

        public IObservable<TResult> Generate(IObservable<TSource> source)
        {
            return Observable.Create<TResult>(async observer =>
            {
                var cd = await ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot);

                var inTable = cd.Context.DeviceTable.TryGetValue((uint)DeviceAddress.Address, out var device);
                if (!inTable || device.ID != (int)ID)
                {
                    throw new WorkflowException("Selected device address is invalid.");
                }

                var sourceSubscription = source.Subscribe(
                    input =>
                    {
                        try
                        {
                            Write(cd.Context, input);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                        }
                    },
                    observer.OnError,
                    () => { });

                return new CompositeDisposable(
                    Generate().SubscribeSafe(observer),
                    Disposable.Create(() =>
                    {
                        cd.Dispose();
                        sourceSubscription.Dispose();
                    })
                );
            });
        }

        protected abstract IObservable<TResult> Process(IObservable<ONIManagedFrame<TData>> source, ulong frameOffset);

        protected abstract void Write(ONIContextTask ctx, TSource input);
    }
}
