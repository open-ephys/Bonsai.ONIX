using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class ONIControllerEditorDialog : Form
    {
        // Hold reference to the controller being manipulated
        public Bonsai.ONIX.ONIController CtrlRef; 

        public ONIControllerEditorDialog(Bonsai.ONIX.ONIController controller)
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

                for (int i = 0; i < CtrlRef.AcqContext.DeviceTable.Count; i++) {

                    var d = CtrlRef.AcqContext.DeviceTable.Values.ElementAt(i);

                    if (d.id != (int)oni.Device.DeviceID.NULL)
                    {

                        var ri = dataGridViewDeviceTable.Rows.Add(
                            d.idx.ToString() + $@" (0x{(byte)(d.idx >> 8):X2}.0x{(byte)(d.idx >> 0):X2})",
                            d.id, 
                            d.version,
                            d.read_size, 
                            d.write_size, 
                            d.Description());

                        dataGridViewDeviceTable.Rows[ri].HeaderCell.Value = ri.ToString();
                    }
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

        private void comboBoxDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            CtrlRef.Driver = comboBoxDriver.SelectedItem.ToString();
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

        private void hubsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new HubConfigurationEditor(CtrlRef);
            f.Show();
        }


    }
}
