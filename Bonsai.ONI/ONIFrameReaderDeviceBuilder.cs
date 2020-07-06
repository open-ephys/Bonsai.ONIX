using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Bonsai.ONI
{
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    public abstract class ONIFrameReaderDeviceBuilder<TResult> : SingleArgumentExpressionBuilder
    {

        // Selected Index within device table.
        DeviceIndexSelection idx;
        [Category("ONI Config.")]
        [Editor("Bonsai.ONI.Design.DeviceIndexCollectionEditor, Bonsai.ONI.Design", typeof(UITypeEditor))]
        [Description("The device index (address) within the device tree.")]
        public DeviceIndexSelection DeviceIndex {
            get { return idx; }
            set { 
                idx = value;
                if (Controller != null)
                {
                    // TODO: ClockHz = oni_controller.AcqContext.DeviceMap[idx.SelectedIndex].clock_hz;
                }
            } 
        }

        [Category("ONI Config.")]
        [Description("The hardware controller supplying data to this node.")]
        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public ONIController Controller { get; private set; }

        // TODO: every device needs to have its own clock hz!
        [Category("ONI Config.")]
        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public int ClockHz { get; private set; } = 250_000_000;

        public readonly oni.Device.DeviceID ID = oni.Device.DeviceID.NULL;

        public ONIFrameReaderDeviceBuilder(oni.Device.DeviceID dev_id)
        {
            DeviceIndex = new DeviceIndexSelection();
            ID = dev_id;
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var source = arguments.First();

            // Find upstream ONIController
            var visitor = new ControllerFinder();
            visitor.Visit(source);
            if (visitor.Controllers.Count != 1)
            {
                // Never should contain more than 1 because must be attached to a frame source directly
                throw new Bonsai.WorkflowBuildException("ONI FrameReader Device is attached to multiple frame sources");
            }

            Controller = visitor.Controllers[0];

            // Find valid device indices
            var devices = ONIHelpers.FindMachingDevices(Controller.AcqContext, ID);
            if (devices.Count == 0) throw new oni.ONIException(oni.lib.Error.DEVID);
            DeviceIndex.Indices = devices.Keys.ToArray();

            // Create combinator
            var thisType = GetType();
            var method = thisType.GetMethod(nameof(Process));
            var instance = Expression.Constant(this);
            return Expression.Call(instance, method, new[] { source });
        }

        public abstract IObservable<TResult> Process(IObservable<oni.Frame> source);
    }
}
