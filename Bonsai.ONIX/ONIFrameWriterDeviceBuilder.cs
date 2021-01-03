using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONIFrameWriterDeviceBuilder<TSource> : ONIDeviceExpressionBuilder
    {

        static readonly Range<int> argumentRange = Range.Create(lowerBound: 1, upperBound: 1);

        public override Range<int> ArgumentRange
        {
            get { return argumentRange; }
        }

        public ONIFrameWriterDeviceBuilder(ONIXDevices.ID dev_id) : base(dev_id) { }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var source = arguments.First();

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

                    // Create Sink
                    var thisType = GetType();
                    var method = thisType.GetMethod(nameof(Process));
                    var instance = Expression.Constant(this);
                    return Expression.Call(instance, method, new[] { source });
                }
            }
        }

        public IObservable<TSource> Process(IObservable<TSource> source)
        {
            return Observable.Using(
                cancellationToken => ONIContextManager.ReserveContextAsync(HardwareSlot),
                (ctx, cancellationToken) =>
                {
                    return Task.FromResult(source.Do(input =>
                    {
                        Write(ctx.Context, input);
                    }));
                }
            );
        }

        public abstract void Write(ONIContextTask ctx, TSource source);

    }
}
