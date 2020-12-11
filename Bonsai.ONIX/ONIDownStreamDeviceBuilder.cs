using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    public abstract class ONIDownStreamDeviceBuilder<TSource, TResult> : SingleArgumentExpressionBuilder
    {
        // Controller references
        protected ControllerSelection Controllers;

        [Category("ONI Config.")]
        [Editor("Bonsai.ONIX.Design.ControllerCollectionEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Description("The hardware controller this node will write frames to.")]
        public ControllerSelection Controller
        {
            get { return Controllers; }
            set
            {
                Controllers = value;
                if (value.SelectedController != null)
                {
                    var devices = ONIHelpers.FindMachingDevices(value.SelectedController.AcqContext, ID);
                    if (devices.Count == 0) throw new Bonsai.WorkflowBuildException("Device was not found in device table."); ;
                    DeviceIndex.Indices = devices.Keys.ToArray();
                }
            }
        }

        // Selected Index within device table.
        [Category("ONI Config.")]
        [Editor("Bonsai.ONIX.Design.DeviceIndexCollectionEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Description("The fully specified device index within the device table.")]
        public DeviceIndexSelection DeviceIndex { get; set; }

        public readonly ONIXDevices.ID ID = ONIXDevices.ID.NULL;

        public ONIDownStreamDeviceBuilder(ONIXDevices.ID dev_id)
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
                throw new Bonsai.WorkflowBuildException("Could not associate with upstream ONI hardware controller.");
            }

            Controller.Controllers = visitor.Controllers;

            if (Controller.SelectedController != null)
            {
                var devices = ONIHelpers.FindMachingDevices(Controller.SelectedController.AcqContext, ID);
                if (devices.Count == 0) throw new Bonsai.WorkflowBuildException("Device was not found in device table.");
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
