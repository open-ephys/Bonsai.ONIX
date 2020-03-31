using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    class HiddenStateVisitor : ExpressionVisitor
    {
        public ONIController Controller { get; private set; }

        protected override Expression VisitExtension(Expression node)
        {
            var hiddenState = node as HiddenStateExpression;
            if (hiddenState != null)
            {
                Controller = hiddenState.Tag;
                // code to do with hidden state
                //Console.WriteLine(hiddenState.Tag.Driver);
            }

            return base.VisitExtension(node);
        }
    }

    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    public abstract class ONIFrameReaderDeviceBuilder<TResult> : SingleArgumentExpressionBuilder
    {
        // Private controller reference
        protected ONIController Controller;

        // Selected Index within device table.
        [Editor("Bonsai.ONI.Design.DeviceIndexCollectionEditor, Bonsai.ONI.Design", typeof(UITypeEditor))]
        [Description("The device index (address) within the device tree.")]
        public DeviceIndexSelection DeviceIndex { get; set; }

        public readonly oni.Device.DeviceID ID = oni.Device.DeviceID.NULL;

        // TODO: every device needs to have its own clock hz!

        public ONIFrameReaderDeviceBuilder(oni.Device.DeviceID dev_id)
        {
            DeviceIndex = new DeviceIndexSelection();
            ID = dev_id;
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var source = arguments.First();

            var visitor = new HiddenStateVisitor();
            visitor.Visit(source); // This is where you must do your recursive search!
            Controller = visitor.Controller;

            while (Controller == null && source.CanReduce)
            {
                source = source.ReduceAndCheck(); // This returns the proxy Expression, not the hidden state
                visitor.Visit(source);
                Controller = visitor.Controller;
            }

            // Find valid device indicies
            var devices = ONIHelpers.FindMachingDevices(Controller.AcqContext, ID);
            if (devices.Count == 0) throw new oni.ONIException(oni.lib.Error.DEVID);
            DeviceIndex.Indices = devices.Keys.ToArray();

            var thisType = GetType();
            var method = thisType.GetMethod(nameof(Process));
            var instance = Expression.Constant(this);
            return Expression.Call(instance, method, new[] { source });
        }

        public abstract IObservable<TResult> Process(IObservable<oni.Frame> source);
    }
}
