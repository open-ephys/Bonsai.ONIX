using System;
using System.Linq;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class MultiBoardSyncDialog : Form
    {
        readonly ONIContextConfiguration Configuration;
        public MultiBoardSyncDialog(Bonsai.ONIX.ONIContextConfiguration configuration)
        {
            InitializeComponent();
            Configuration = configuration;
            using (var c = Bonsai.ONIX.ONIContextManager.ReserveContext(Configuration.Slot))
            {
                int addr = c.Context.HardwareAddress;
                int sync_channel = addr & 0x000000FF;
                int sync_mode = (addr & 0x00FF0000) >> 16;
                groupMode.Controls.Cast<RadioButton>().Where(b => b.TabIndex == sync_mode).ToList().ForEach(b => b.Checked = true);
                groupChannel.Controls.Cast<RadioButton>().Where(b => b.TabIndex == sync_channel).ToList().ForEach(b => b.Checked = true);
            }
        }

        private void radioButonMode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            if (pressed.Checked)
            {
                if (pressed == radioButonStandalone)
                {
                    groupChannel.Enabled = false;
                }
                else
                {
                    groupChannel.Enabled = true;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int sync_mode = groupMode.Controls.Cast<RadioButton>().Where(b => b.Checked).Select<RadioButton, int>(b => b.TabIndex).DefaultIfEmpty(0).First();
            int sync_channel = 0;
            if (sync_mode != 0)
            {
                sync_channel = groupChannel.Controls.Cast<RadioButton>().Where(b => b.Checked).Select<RadioButton, int>(b => b.TabIndex).DefaultIfEmpty(0).First();
            }
            int addr = sync_channel + (sync_mode << 16);
            using (var c = Bonsai.ONIX.ONIContextManager.ReserveContext(Configuration.Slot))
            {
                c.Context.HardwareAddress = addr;
            }
            Close();
        }
    }
}
