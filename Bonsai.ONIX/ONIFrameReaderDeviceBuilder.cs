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
    [Combinator(MethodName = "Process")]
    [WorkflowElementCategory(ElementCategory.Combinator)]
    public abstract class ONIFrameReaderDeviceBuilder<TResult> : SingleArgumentExpressionBuilder
    {
        // Selected Index within device table.
        DeviceIndexSelection idx;
        [Category("ONI Config.")]
        [Editor("Bonsai.ONIX.Design.DeviceIndexCollectionEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Description("The fully specified device index within the device table.")]
        public DeviceIndexSelection DeviceIndex {
            get { return idx; }
            set { 
                idx = value;
                UpdateClocks();
            } 
        }

        private void UpdateClocks()
        {
            if (Controller != null)
            {
                FrameClockHz = Controller.AcqContext.AcquisitionClockHz;
                try
                {
                    DataClockHz = Controller.HubDataClock((uint)DeviceIndex.SelectedIndex & 0x0000FF00);
                } catch
                {
                    DataClockHz = 0;
                }
            }
        }

        [Category("ONI Config.")]
        [Description("The hardware controller supplying data to this node.")]
        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public ONIController Controller { get; private set; }

        [Category("ONI Config.")]
        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public double FrameClockHz { get; private set; }

        [Category("ONI Config.")]
        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public double DataClockHz { get; private set; }

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
                throw new Bonsai.WorkflowBuildException("ONI FrameReader Device is attached to multiple ONI controllers.");
            }

            Controller = visitor.Controllers[0];

            // Find valid device indices
            var devices = ONIHelpers.FindMachingDevices(Controller.AcqContext, ID);
            if (devices.Count == 0) throw new oni.ONIException(oni.lib.Error.DEVID);
            DeviceIndex.Indices = devices.Keys.ToArray();

            // Update internal parameters
            UpdateClocks();

            // Create combinator
            var thisType = GetType();
            var method = thisType.GetMethod(nameof(Process));
            var instance = Expression.Constant(this);
            return Expression.Call(instance, method, new[] { source });
        }

        public abstract IObservable<TResult> Process(IObservable<oni.Frame> source);
    }
}
