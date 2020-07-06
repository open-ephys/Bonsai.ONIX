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
            numericUpDownWriteAlloc.Value = CtrlRef.WritePreAllocSize;
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
            dataGridViewDeviceTable.Rows.Clear();

            if (CtrlRef.TryRefresh())
            {
                int k = 0;
                foreach (var d in CtrlRef.AcqContext.DeviceTable.Values) {

                    var ri = dataGridViewDeviceTable.Rows.Add(k++, 
                        $@"0x{(byte)(d.idx >> 8):X2}", $@"0x{(byte)(d.idx >> 0):X2}", 
                        d.id, d.read_size, d.write_size, d.Description());
                    dataGridViewDeviceTable.Rows[ri].HeaderCell.Value = ri.ToString();
                }

                labelConnected.Text = "✔";
                labelConnected.ForeColor = Color.ForestGreen;

                numericUpDownReadSize.Minimum = CtrlRef.AcqContext.MaxReadFrameSize;
                updateReadSize();
                updateWriteSize();

            } else {
                labelConnected.Text = "✘";
                labelConnected.ForeColor = Color.Red;
            }
        }

        private void updateReadSize()
        {
             CtrlRef.BlockReadSize = (int)numericUpDownReadSize.Value;
        }

        private void updateWriteSize()
        {
            CtrlRef.WritePreAllocSize = (int)numericUpDownWriteAlloc.Value;
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

        private void dataGridViewDeviceTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((DataGridView)sender).SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void numericUpDownWriteAlloc_ValueChanged(object sender, EventArgs e)
        {
            updateWriteSize();
        }
    }
}
