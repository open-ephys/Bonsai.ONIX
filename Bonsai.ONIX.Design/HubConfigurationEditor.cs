using System;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class HubConfigurationEditor : Form
    {
        // Hold reference to the controller being manipulated
        Bonsai.ONIX.ONIController CtrlRef;
        int hub_state;

        public HubConfigurationEditor(Bonsai.ONIX.ONIController controller)
        {
            InitializeComponent();

            CtrlRef = controller;
            hub_state = CtrlRef.AcqContext.GetCustomOption((int)oni.lib.ONIXOption.PORTFUNC);

            radioButtonAStandard.Checked = (hub_state & 0x0001) == 0;
            radioButtonAPassthrough.Checked = (hub_state & 0x0001) == 1;
            radioButtonBStandard.Checked = (hub_state & 0x0004) == 0;
            radioButtonBPassthrough.Checked = (hub_state & 0x0004) == 1;
        }

        private void radioButtonAStandard_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAStandard.Checked)
            {
                hub_state &= ~(1 << 0);
            }
            else
            {
                hub_state |= 1 << 0;
            }
        }

        private void radioButtonBStandard_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButtonBStandard.Checked)
            {
                hub_state &= ~(1 << 2);
            }
            else
            {
                hub_state |= 1 << 2;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CtrlRef.AcqContext.SetCustomOption((int)oni.lib.ONIXOption.PORTFUNC, hub_state);
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
