using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;


namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.RHS2116Trigger)]
    [Description("Trigger circuit to sychronize application of stimulation patterns across multiple " +
        "RHS2116 chips and headstages (when using inter-headstage sync cable).")]
    [DefaultProperty("DeviceAddress")]
    public class RHS2116TriggerDevice : ONISink<bool>
    {
        private enum Register
        {
           TRIGGERSOURCE,
           TRIGGER
        }

        public enum TriggerSourcePin
        {
            Local,
            External
        }

        [Category("Acquisition")]
        [Description("Trigger signal source.")]
        public TriggerSourcePin TriggerSource
        {
            get 
            { 
                return (TriggerSourcePin)ReadRegister((uint)Register.TRIGGERSOURCE); 
            } 
            set
            { 
                WriteRegister((uint)Register.TRIGGERSOURCE, (uint)value); 
            }
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        // TODO: think about using GPIO?
        protected override void OnNext(ONIContextTask ctx, bool triggered)
        {
            WriteRegister((int)Register.TRIGGER, (uint)(triggered ? 1 : 0));
        }

        protected override void OnFinally(ONIContextTask ctx)
        {
            WriteRegister((int)Register.TRIGGER, 0);
        }
    }
}
