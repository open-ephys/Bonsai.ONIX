using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Controls a digital communication link to a remote headstage on the Open Ephys FMC Host.")]
    [ONIXDeviceID(ONIXDevices.ID.FMCLinkController)]
    public class HeadstagePortControlDevice : ONIFrameReader<HeadstagePortControlFrame, ushort>
    {
        const double VLIM = 8.0;

        enum Register
        {
            ENABLE = 0,
            GPOSTATE = 1,
            DESERIALIZERPOWER = 2, // NB: Could eventually be used in a lock loss recovery routine, but currently unused.
            LINKVOLTAGE = 3,
            SAVELINKVOLTAGE = 4,
        }

        public HeadstagePortControlDevice() { }

        protected override IObservable<HeadstagePortControlFrame> Process(IObservable<ONIManagedFrame<ushort>> source)
        {
            return source.Select(f => { return new HeadstagePortControlFrame(f); });
        }

        
        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

        //[Description("Save link A voltage to internal EEPROM.")]
        //public bool SaveLinkAVoltage { get; set; } = false;

        //[Category("Acquisition")]
        //[Description("Headstage deserializer power enabled or disabled.")]
        //public bool DeserializerPowerEnabled
        //{
        //    get
        //    {
        //        return ReadRegister((int)Register.DESERIALIZERPOWER) > 0;
        //    }
        //    set
        //    {
        //        WriteRegister((int)Register.DESERIALIZERPOWER, (uint)(value ? 1 : 0));
        //    }
        //}

        [Category("Acquisition")]
        [Description("Type \"BE CAREFUL\" here to enable the extended link voltage range to 10V.")]
        public string EnableExtendedVoltageRange { get; set; }


        [Category("Acquisition")]
        [Range(3.3, 10.0)]
        [Precision(1, 0.1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Headstage link voltage. WARNING: THIS VOLTAGE CAN DAMAGE YOUR HEADSTAGE IF SET TOO HIGH! " +
            "The standard allowable voltage range is 0-8V. By using the EnableExtendedVoltageRange setting, " +
            "this can be expanded to 10V.")]
        public double LinkVoltage
        {
            get
            {
                return ReadRegister((int)Register.LINKVOLTAGE) / 10.0;
            }
            set
            {
                var link_v = EnableExtendedVoltageRange != "BE CAREFUL" & value > VLIM ? VLIM : value;
                WriteRegister((int)Register.LINKVOLTAGE, (uint)(link_v * 10));
            }
        }

        uint gpoRegister;
        [Category("Acquisition")]
        [Description("GPO1 state. Used to trigger certain functionality on the headstage. Check headstage documentation.")]
        public bool? GPO1
        {
            get
            {
                var val = ReadRegister((int)Register.GPOSTATE);
                gpoRegister = val;
                return (gpoRegister & 0x01) > 0;
            }
            set
            {

                gpoRegister = (gpoRegister & ~((uint)1 << 0)) | (((bool)value ? 1 : (uint)0) << 0);
                WriteRegister((int)Register.GPOSTATE, gpoRegister);
            }
        }

        //[Category("Acquisition")]
        //[Description("GPO Line state. Used to trigger certain functionality on the headstage. Check headstage documentation.")]
        //public bool[] GPOLines
        //{
        //    get
        //    {
        //        var val = Controller?.ReadRegister(DeviceIndex.Index, (int)Register.GPOSTATE);
        //        if (val != null) return new bool[] { (val & 0x01) > 0, (val & 0x02) > 0, (val & 0x04) > 0 };
        //        return null;
        //    }
        //    set
        //    {
        //        if (Controller != null && value != null)
        //        {
        //            var val = (value[0] ? 0x01 : 0x00) | (value[1] ? 0x02 : 0x00) | (value[2] ? 0x04 : 0x00);
        //            Controller.WriteRegister(DeviceIndex.Index, (int)Register.GPOSTATE, (uint)val);
        //        }
        //    }
        //}
    }
}
