using System.ComponentModel;


namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.RHS2116Trigger)]
    [Description("Trigger circuit to sychronize application of stimulation patterns across multiple " +
        "RHS2116 chips and headstages (when using inter-headstage sync cable). Takes a double indicating " +
        "delay, in microseconds, that should be applied before stimulus is delivered.")]
    [DefaultProperty("DeviceAddress")]
    public class RHS2116TriggerDevice : ONISink<double>
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
        protected override void OnNext(ONIContextTask ctx, double delayMicroSec)
        {
            var delaySamples = (int)(delayMicroSec / RHS2116Device.SamplePeriodMicroSeconds);
            WriteRegister((int)Register.TRIGGER, (uint)(delaySamples << 12 | 0x1));
        }

        protected override void OnFinally(ONIContextTask ctx)
        {
            WriteRegister((int)Register.TRIGGER, 0);
        }
    }
}
