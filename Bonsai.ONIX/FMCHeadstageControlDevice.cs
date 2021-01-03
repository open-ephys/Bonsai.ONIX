using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Controls a SERDES link to a remote headstage on the Open Ephys FMC Host. THIS NODE CAN DAMAGE YOUR HEADSTAGE: BE CAREFUL!")]
    public class FMCHeadstageControlDevice : ONIFrameReaderDeviceBuilder<FMCHeadstageControlFrame>
    {
        const double VLIM = 6.5;

        // NB: registers for this device are all write only.
        enum Register
        {
            GPOSTATE = 0,
            DESERIALIZERPOWER = 1,
            LINKVOLTAGE = 2,
            SAVELINKVOLTAGE = 3,
        }

        public FMCHeadstageControlDevice() : base(ONIXDevices.ID.FMCLINKCTRL) { }

        public override IObservable<FMCHeadstageControlFrame> Process(IObservable<oni.Frame> source)
        {
            return source.Select(f => { return new FMCHeadstageControlFrame(f); });
        }

        //[Description("Save link A voltage to internal EEPROM.")]
        //public bool SaveLinkAVoltage { get; set; } = false;
        //        if (SaveLinkAVoltage) ctx.Environment.AcqContext.WriteRegister((uint)DeviceIndex.SelectedIndex, (int)Register.SAVELINKA, 0);

        [Category("Acquisition")]
        [Description("Headstage deserializer power enabled or disabled.")]
        public bool DeserializerPowerEnabled
        {
            get
            {
                return ReadRegister(DeviceIndex.SelectedIndex, (int)Register.DESERIALIZERPOWER) > 0;
            }
            set
            {
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.DESERIALIZERPOWER, (uint)(value ? 1 : 0));
            }
        }

        [Category("Acquisition")]
        [Description("Type \"BE CAREFUL\" here to enable the extended link voltage range.")]
        public string EnableExtendedVoltageRange { get; set; }

        bool link_enabled = true;
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
        double link_v = 5.5;
        [Category("Acquisition")]
        [Range(3.3, 10.0)]
        [Precision(1, 0.1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        [Description("Headstage link voltage.")]
        public double LinkVoltage
        {
            get
            {
                return link_v;
            }
            set
            {

                link_v = EnableExtendedVoltageRange != "BE CAREFUL" & value > VLIM ? VLIM : value;
                link_v = link_enabled ? link_v : 0.0;

                //// TODO: HACK HACK HACK
                //// The idea here is that on headstages that use the ds9033/9034 SERDES pair, there is some POR issue 
                //// in our circuit that is preventing a lock. If we quickly power cycle, the caps on the HS hold charge where
                //// it needs to be long enough to get the POR sequence right.
                //WriteRegister(DeviceIndex.SelectedIndex, (int)Register.LINKVOLTAGE, (uint)(link_v * 10));
                //Thread.Sleep(100);
                //WriteRegister(DeviceIndex.SelectedIndex, (int)Register.LINKVOLTAGE, 0);
                //Thread.Sleep(500);
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.LINKVOLTAGE, (uint)(link_v * 10));
            }
        }

        uint gpo_register;

        [Category("Acquisition")]
        [Description("GPO1 state. Used to trigger certain functionality on the headstage. Check headstage documentation.")]
        public bool? GPO1
        {
            get
            {
                var val = ReadRegister(DeviceIndex.SelectedIndex, (int)Register.GPOSTATE);
                gpo_register = val;
                return (gpo_register & 0x01) > 0;
            }
            set
            {

                gpo_register = (gpo_register & ~((uint)1 << 0)) | (((bool)value ? 1 : (uint)0) << 0);
                WriteRegister(DeviceIndex.SelectedIndex, (int)Register.GPOSTATE, gpo_register);
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
