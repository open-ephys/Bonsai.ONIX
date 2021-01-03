using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Source]
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public abstract class ONIFrameReaderDeviceBuilder<TResult> : ONIDeviceExpressionBuilder
    {
        static readonly Range<int> argumentRange = Range.Create(lowerBound: 0, upperBound: 0);

        public override Range<int> ArgumentRange
        {
            get { return argumentRange; }
        }

        public ONIFrameReaderDeviceBuilder(ONIXDevices.ID dev_id) : base(dev_id) { }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            using (var c = ONIContextManager.ReserveContext(HardwareSlot))
            {
                // Find valid device indices
                var devices = c.Context.DeviceTable
                    .Where(dev => dev.Value.id == (uint)ID)
                    .ToDictionary(x => x.Value.idx, x => x.Value);

                if (devices.Count == 0)
                {
                    FrameClockHz = null;
                    DataClockHz = null;
                    throw new Bonsai.WorkflowBuildException("Device was not found in device table.");
                }
                else
                {
                    DeviceIndex.Indices = devices.Keys.ToArray();

                    // Update device-specific clocks
                    FrameClockHz = c.Context.AcquisitionClockHz;
                    DataClockHz = c.Context.HubDataClock((uint)DeviceIndex.SelectedIndex & 0x0000FF00);

                    // Create combinator
                    var thisType = GetType();
                    var method = thisType.GetMethod(nameof(Generate));
                    var instance = Expression.Constant(this);
                    return Expression.Call(instance, method);
                }
            }
        }

        public IObservable<TResult> Generate()
        {
            var source = Observable.Create<oni.Frame>(async observer =>
                {
                    var context = await ONIContextManager.ReserveContextAsync(HardwareSlot);

                    EventHandler<FrameReceivedEventArgs> frame_received = (sender, e) =>
                    {
                        if (e.Frame.DeviceIndex() == DeviceIndex.SelectedIndex)
                        {
                            observer.OnNext(e.Frame);
                        }
                    };

                    context.Context.FrameReceived += frame_received;
                    return Disposable.Create(() =>
                    {
                        context.Context.FrameReceived -= frame_received;
                        context.Dispose();
                    });
                });

            return Process(source);
        }

        public abstract IObservable<TResult> Process(IObservable<oni.Frame> source);
    }
}
