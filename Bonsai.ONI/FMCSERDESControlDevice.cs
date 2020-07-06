using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.ONI
{
    [Description("Controls the dual SERDES link voltage on the Open Ephys FMC Host. THIS NODE CAN DAMAGE YOUR HEADSTAGE: BE CAREFUL!")]
    public class FMCSERDESControlDevice : ONIRegisterOnlyDeviceBuilder
    {
        const double VLIM = 6.3;

        // Control registers (see oedevices.h)
        // NB: registers for this device are all write only.
        public enum Register
        {
            LINKAVOLTAGE = 0,
            LINKBVOLTAGE = 1,
            SAVELINKA = 2,
            SAVELINKB = 3,
            DESAPND = 4,
            DESBPND = 5,
        }

        public FMCSERDESControlDevice() : base(oni.Device.DeviceID.FMCVCTRL) { }

        bool link_a_enabled = false;
        [Description("Link A main power enabled or disabled.")]
        public bool LinkAPowerEnabled
        {
            get
            {
                return link_a_enabled;
            }
            set
            {
                link_a_enabled = value;
                LinkAVoltage = link_a_v;
            }
        }

        bool link_b_enabled = false;
        [Description("Link B main power enabled or disabled.")]
        public bool LinkBPowerEnabled
        {
            get
            {
                return link_b_enabled;
            }
            set
            {
                link_b_enabled = value;
                LinkBVoltage = link_b_v;
            }
        }

        //[Description("Save link A voltage to internal EEPROM.")]
        //public bool SaveLinkAVoltage { get; set; } = false;
        //        if (SaveLinkAVoltage) ctx.Environment.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.SAVELINKA, 0);

        //[Description("Save link B voltage to internal EEPROM.")]
        //public bool SaveLinkBVoltage { get; set; } = false;
        //        if (SaveLinkBVoltage) ctx.Environment.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.SAVELINKB, 0);

        bool des_a_enabled = true;
        [Description("Deserializer A power enabled or disabled.")]
        public bool DesAPowerEnabled {
            get
            {
                return des_a_enabled;
            }
            set
            {
                if (Controller != null)
                {
                    des_a_enabled = value;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.DESAPND, des_a_enabled ? (uint)1 : 0);
                }
            }
        }

        bool des_b_enabled = true;
        [Description("Deserializer B power enabled or disabled.")]
        public bool DesBPowerEnabled {
            get
            {
                return des_b_enabled;
            }
            set
            {
                if (Controller != null)
                {
                    des_b_enabled = value;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.DESBPND, des_b_enabled ? (uint)1 : 0);
                }
            }
        }

        bool extended_v_enabled = false;
        string extended_v_enabled_str = "";
        [Description("Type \"BE CAREFUL\" here to enable the extended link voltage range.")]
        public string EnableExtendedVoltageRange {
            get { return extended_v_enabled_str; }
            set {
                extended_v_enabled_str = value;
                if (extended_v_enabled_str == "BE CAREFUL")
                    extended_v_enabled = true;
                else
                    extended_v_enabled = false;
            }
        }

        double link_a_v = 5.0;
        [Range(3.3, 11.0)]
        [Precision(1, 0.1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Link A voltage.")]
        public double LinkAVoltage {
            get
            {
                return link_a_v;
            }
            set {
                if (Controller != null)
                {
                    link_a_v = !extended_v_enabled & value > VLIM ? VLIM : value;
                    link_a_v = link_a_enabled ? link_a_v : 0.0;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LINKAVOLTAGE, (uint)(link_a_v * 10));
                }
            }
        }

        double link_b_v = 5.0;
        [Range(3.3, 11.0)]
        [Precision(1, 0.1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Link B voltage.")]
        public double LinkBVoltage
        {
            get
            {
                return link_b_v;
            }
            set
            {
                if (Controller != null)
                {
                    link_b_v = !extended_v_enabled & value > VLIM ? VLIM : value;
                    link_b_v = link_b_enabled ? link_b_v : 0.0;
                    Controller.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.LINKBVOLTAGE, (uint)(link_b_v * 10));
                }
            }
        }
    }
}
