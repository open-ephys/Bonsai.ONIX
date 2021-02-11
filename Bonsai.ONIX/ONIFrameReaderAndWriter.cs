using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public abstract class ONIFrameReaderAndWriter<TSource, TResult> : ONIDevice
    {
        public ONIFrameReaderAndWriter(ONIXDevices.ID dev_id) : base(dev_id) { }

        public IObservable<TResult> Generate()
        {
            var source = Observable.Create<oni.Frame>(async observer =>
                {
                    var cd = await ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot);

                    var in_table = cd.Context.DeviceTable.TryGetValue(DeviceAddress.Address, out var device);
                    if (!in_table || device.ID != (int)ID)
                    {
                        throw new WorkflowException("Selected device address is invalid.");
                    }

                    EventHandler<FrameReceivedEventArgs> frame_received = (sender, e) =>
                    {
                        if (e.Frame.DeviceAddress == DeviceAddress.Address)
                        {
                            observer.OnNext(e.Frame);
                        }
                    };

                    cd.Context.FrameReceived += frame_received;
                    return Disposable.Create(() =>
                    {
                        cd.Context.FrameReceived -= frame_received;
                        cd.Dispose();
                    });
                });

            return Process(source.ObserveOn(Scheduler.Default.DisableOptimizations(typeof(ISchedulerLongRunning))));
        }

        public IObservable<TResult> Generate(IObservable<TSource> source)
        {
            return Observable.Create<TResult>(async observer =>
            {
                var cd = await ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot);

                var in_table = cd.Context.DeviceTable.TryGetValue(DeviceAddress.Address, out var device);
                if (!in_table || device.ID != (int)ID)
                {
                    throw new WorkflowException("Selected device address is invalid.");
                }

                var source_sub = source.Subscribe(input =>
                    Write(cd.Context, input),
                    observer.OnError,
                    () => { });

                return new CompositeDisposable(
                    Generate().SubscribeSafe(observer),
                    Disposable.Create(() =>
                    {
                        cd.Dispose();
                        source_sub.Dispose();
                    })
                );
            });
        }

        protected abstract IObservable<TResult> Process(IObservable<oni.Frame> source);
        protected abstract void Write(ONIContextTask ctx, TSource input);
    }
}
