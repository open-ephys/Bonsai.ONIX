using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONIX
{
    [Description("Controls a SERDES link to a remote headstage on the Open Ephys FMC Host. THIS NODE CAN DAMAGE YOUR HEADSTAGE: BE CAREFUL!")]
    public class FMCHeadstageControlDevice : ONIFrameReaderDeviceBuilder<FMCHeadstageControlFrame>
    {
        const double VLIM = 6.3;

        // Control registers (see oedevices.h)
        // NB: registers for this device are all write only.
        public enum Register
        {
            GPOSTATE = 0,
            DESERIALIZERPOWER = 1,
            LINKVOLTAGE = 2,
            SAVELINKVOLTAGE = 3,
        }

        public FMCHeadstageControlDevice() : base(oni.Device.DeviceID.FMCLINKCTRL) { }

        public override IObservable<FMCHeadstageControlFrame> Process(IObservable<oni.Frame> source)
        {
            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Select(f => { return new FMCHeadstageControlFrame(f, FrameClockHz, DataClockHz); });
        }

        //[Description("Save link A voltage to internal EEPROM.")]
        //public bool SaveLinkAVoltage { get; set; } = false;
        //        if (SaveLinkAVoltage) ctx.Environment.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.SAVELINKA, 0);

        [Category("Acquisition")]
        [Description("Headstage deserializer power enabled or disabled.")]
        public bool? DeserializerPowerEnabled
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.DESERIALIZERPOWER);
                if (val != null) return val > 0;
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.DESERIALIZERPOWER, (bool)value ? (uint)1 : 0);
                }
            }
        }

        [Category("Acquisition")]
        [Description("Type \"BE CAREFUL\" here to enable the extended link voltage range.")]
        public string EnableExtendedVoltageRange { get; set; } 

        bool link_enabled = false;
        [Category("Acquisition")]
        [Description("Headstage power enabled or disabled.")]
        public bool LinkPowerEnabled
        {
            get
            {
                return link_enabled;
            }
            set
            {
                link_enabled = value;
                LinkVoltage = link_v;
            }
        }

        // TODO: reading voltage does not work with current firmware
        double link_v = 5.0;
        [Category("Acquisition")]
        [Range(3.3, 10.0)]
        [Precision(1, 0.1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Headstage link voltage.")]
        public double LinkVoltage {
            get
            {
                return link_v;
            }
            set {
                if (Controller != null)
                {
                    link_v = EnableExtendedVoltageRange != "BE CAREFUL" & value > VLIM ? VLIM : value;
                    link_v = link_enabled ? link_v : 0.0;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LINKVOLTAGE, (uint)(link_v * 10));
                }
            }
        }

        uint gpo_register;

        [Category("Acquisition")]
        [Description("GPO1 state. Used to trigger certain functionality on the headstage. Check headstage documentation.")]
        public bool? GPO1
        {
            get
            {
                var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.GPOSTATE);
                if (val != null) { gpo_register = (uint)val; return (gpo_register & 0x01) > 0; };
                return null;
            }
            set
            {
                if (Controller != null && value != null)
                {
                    gpo_register = (gpo_register & ~((uint)1 << 0)) | (((bool)value ? (uint)1 : (uint)0) << 0);
                    Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.GPOSTATE, gpo_register);
                }
            }
        }


        //[Category("Acquisition")]
        //[Description("GPO Line state. Used to trigger certain functionality on the headstage. Check headstage documentation.")]
        //public bool[] GPOLines
        //{
        //    get
        //    {
        //        var val = Controller?.ReadRegister(DeviceIndex.SelectedIndex, (int)Register.GPOSTATE);
        //        if (val != null) return new bool[] { (val & 0x01) > 0, (val & 0x02) > 0, (val & 0x04) > 0 };
        //        return null;
        //    }
        //    set
        //    {
        //        if (Controller != null && value != null)
        //        {
        //            var val = (value[0] ? 0x01 : 0x00) | (value[1] ? 0x02 : 0x00) | (value[2] ? 0x04 : 0x00);
        //            Controller.WriteRegister(DeviceIndex.SelectedIndex, (int)Register.GPOSTATE, (uint)val);
        //        }
        //    }
        //}
    }
}
