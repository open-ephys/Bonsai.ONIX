using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public abstract class ONIFrameReader<TResult, TData> : ONIDevice where TData : unmanaged
    {
        public ONIFrameReader(ONIXDevices.ID deviceId) : base(deviceId) { }

        public IObservable<TResult> Generate()
        {
            var source = Observable.Create<ONIManagedFrame<TData>>(async observer =>
                {
                    var cd = await ONIContextManager.ReserveOpenContextAsync(DeviceAddress.HardwareSlot);

                    var inTable = cd.Context.DeviceTable.TryGetValue(DeviceAddress.Address, out var device);
                    if (!inTable || device.ID != (int)ID)
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

            return Process(source);
        }

        protected abstract IObservable<TResult> Process(IObservable<ONIManagedFrame<TData>> source);
    }
}
