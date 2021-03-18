using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
// TODO get rid of this

namespace Bonsai.ONIX
{
    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public abstract class ONIFrameReader<TResult> : ONIDevice
    {
        public ONIFrameReader(ONIXDevices.ID dev_id) : base(dev_id) { }

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

            return Process(source);
        }

        protected abstract IObservable<TResult> Process(IObservable<oni.Frame> source);
    }
}
