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
    //[Combinator(MethodName = "Process")]
    //[WorkflowElementCategory(ElementCategory.Sink)]
    public abstract class ONIDownStreamDeviceBuilder<TSource, TResult> : SingleArgumentExpressionBuilder
    {
        // Controller references
        protected ControllerSelection Controllers;

        [Category("ONI Config.")]
        [Editor("Bonsai.ONI.Design.ControllerCollectionEditor, Bonsai.ONI.Design", typeof(UITypeEditor))]
        [Description("The hardware controller this node will write frames to.")]
        public ControllerSelection Controller {
            get { return Controllers; }
            set { 
                Controllers = value;
                if (value.SelectedController != null)
                {
                    var devices = ONIHelpers.FindMachingDevices(value.SelectedController.AcqContext, ID);
                    if (devices.Count == 0) throw new oni.ONIException(oni.lib.Error.DEVID);
                    DeviceIndex.Indices = devices.Keys.ToArray();
                }
            }
        }

        // Selected Index within device table.
        [Category("ONI Config.")]
        [Editor("Bonsai.ONI.Design.DeviceIndexCollectionEditor, Bonsai.ONI.Design", typeof(UITypeEditor))]
        [Description("The device index (address) within the device tree.")]
        public DeviceIndexSelection DeviceIndex { get; set; }

        public readonly oni.Device.DeviceID ID = oni.Device.DeviceID.NULL;

        public ONIDownStreamDeviceBuilder(oni.Device.DeviceID dev_id)
        {
            Controller = new ControllerSelection();
            DeviceIndex = new DeviceIndexSelection();
            ID = dev_id;
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var source = arguments.First();

            // Find upstream ONIControllers
            var visitor = new ControllerFinder();
            visitor.Visit(source);
            if (visitor.Controllers.Count == 0)
            {
                throw new Bonsai.WorkflowBuildException("ONI FrameWriter could not associate with upstream hardware controller.");
            }

            Controller.Controllers = visitor.Controllers;

            if (Controller.SelectedController != null) {
                var devices = ONIHelpers.FindMachingDevices(Controller.SelectedController.AcqContext, ID);
                if (devices.Count == 0) throw new oni.ONIException(oni.lib.Error.DEVID);
                DeviceIndex.Indices = devices.Keys.ToArray();
            }

            // Create Sink
            var thisType = GetType();
            var method = thisType.GetMethod(nameof(Process));
            var instance = Expression.Constant(this);
            return Expression.Call(instance, method, new[] { source });
        }

        public abstract IObservable<TResult> Process(IObservable<TSource> source);
    }
}
