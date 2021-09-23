using System;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class HubConfigurationEditor : Form
    {
        private readonly ONIContextConfiguration Configuration;
        private int hub_state;

        public HubConfigurationEditor(Bonsai.ONIX.ONIContextConfiguration configuraiton)
        {
            InitializeComponent();

            Configuration = configuraiton;
            using (var c = Bonsai.ONIX.ONIContextManager.ReserveContext(Configuration.Slot))
            {
                hub_state = c.Context.HubState;
            }

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
            using (var c = Bonsai.ONIX.ONIContextManager.ReserveContext(Configuration.Slot))
            {
                c.Context.HubState = hub_state;
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
