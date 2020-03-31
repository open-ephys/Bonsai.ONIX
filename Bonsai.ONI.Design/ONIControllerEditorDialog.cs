using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bonsai.ONI.Design
{
    public partial class ONIControllerEditorDialog : Form
    {
        // Hold reference to the RIFFA context
        public Bonsai.ONI.ONIController CtrlRef; // { get; set; } // ONIController being manipulated

        public ONIControllerEditorDialog(Bonsai.ONI.ONIController controller)
        {
            InitializeComponent();
            this.ControlBox = false;

            CtrlRef = controller;
            comboBoxDriver.SelectedItem = CtrlRef.Driver;
            numericUpDownPCIeIndex.Value = CtrlRef.Index;
            numericUpDownReadSize.Value = CtrlRef.BlockReadSize;
        }

        protected override void OnLoad(EventArgs e)
        {
            // Attempt to connect the controller
            attemptToConnect();

            base.OnLoad(e);
        }

        private void buttonRefreshContext_Click(object sender, EventArgs e)
        {
            attemptToConnect();
        }

        private void attemptToConnect()
        {
            listBoxDeviceTable.Items.Clear();

            if (CtrlRef.TryRefresh())
            {
                foreach (var d in CtrlRef.AcqContext.DeviceMap)
                {
                    listBoxDeviceTable.Items.Add(d.ToString());
                }

                labelConnected.Text = "✔";
                labelConnected.ForeColor = Color.ForestGreen;

                numericUpDownReadSize.Minimum = CtrlRef.AcqContext.MaxReadFrameSize;
                updateReadSize();

            } else {
                labelConnected.Text = "✘";
                labelConnected.ForeColor = Color.Red;
            }
        }

        private void updateReadSize()
        {
             CtrlRef.BlockReadSize = (int)numericUpDownReadSize.Value;
        }

        private void numericUpDownPCIeIndexChanged(object sender, EventArgs e)
        {
            CtrlRef.Index = (int)numericUpDownPCIeIndex.Value;
        }

        private void numericUpDownReadSizeChanged(object sender, EventArgs e)
        {
            updateReadSize();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            CtrlRef.Refresh();
        }
    }
}
