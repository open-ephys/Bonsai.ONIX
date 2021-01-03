using Bonsai.ONIX.Design.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class ONIContextConfigurationEditorDialog : Form
    {
        // Hold reference to the controller being manipulated
        public Bonsai.ONIX.ONIContextConfiguration Configuration;

        public ONIContextConfigurationEditorDialog(Bonsai.ONIX.ONIContextConfiguration configuration)
        {
            InitializeComponent();

            Configuration = configuration;

            numericUpDownReadSize.DataBindings.Add("Value", Configuration, "ReadSize", false, DataSourceUpdateMode.OnValidation);
            numericUpDownWriteAlloc.DataBindings.Add("Value", Configuration, "WriteSize", false, DataSourceUpdateMode.OnValidation);

            comboBoxDriver.SelectedItem = Configuration.Slot.Driver;
            numericUpDownPCIeIndex.Value = Configuration.Slot.Index;
        }

        protected override void OnLoad(EventArgs e)
        {
            AttemptToConnect();
            base.OnLoad(e);
        }

        private void AttemptToConnect()
        {
            dataGridViewDeviceTable.Rows.Clear();

            try
            {
                using (var c = ONIContextManager.ReserveContext(Configuration.Slot))
                {
                    var context = c.Context;


                    for (int i = 0; i < context.DeviceTable.Count; i++)
                    {

                        var d = context.DeviceTable.Values.ElementAt(i);

                        if (d.id != (int)ONIXDevices.ID.NULL)
                        {

                            var ri = dataGridViewDeviceTable.Rows.Add(
                                d.idx,
                                $@" 0x{(byte)(d.idx >> 8):X2}.0x{(byte)(d.idx >> 0):X2}",
                                ((ONIXDevices.ID)d.id).ToString(),
                                d.version,
                                d.read_size,
                                d.write_size,
                                d.Description());

                            dataGridViewDeviceTable.Rows[ri].HeaderCell.Value = ri.ToString();
                        }
                    }

                    toolStripSplitButton.Text = Resources.ONIConnectionSuccess;
                    toolStripSplitButton.ForeColor = Color.Black;

                    Configuration.ReadSize = (int)context.MaxReadFrameSize;
                    numericUpDownReadSize.Minimum = context.MaxReadFrameSize;

                }
            }
            catch (Exception err)
            {
                if (err is oni.ONIException || err is InvalidProgramException)
                {
                    toolStripSplitButton.Text = err.Message;
                    toolStripSplitButton.ForeColor = Color.Red;
                }
            }
        }

        private void comboBoxDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            Configuration.Slot.Driver = comboBoxDriver.SelectedItem.ToString();
            AttemptToConnect();
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

        private void hubsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new HubConfigurationEditor(Configuration);
            f.Show();
        }

        private void numericUpDownPCIeIndex_ValueChanged(object sender, EventArgs e)
        {
            Configuration.Slot.Index = (int)numericUpDownPCIeIndex.Value;
            AttemptToConnect();
        }

        private void dataGridViewDeviceTable_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewDeviceTable.SelectedRows.Count == 0)
            {
                propertyGrid.SelectedObject = null;
                return;
            }

            var row = dataGridViewDeviceTable.SelectedRows[0];
            var dev_idx = (uint)row.Cells[0].Value;

            using (var c = ONIContextManager.ReserveContext(Configuration.Slot))
            {

                var context = c.Context;
                if (context.DeviceTable.TryGetValue(dev_idx, out oni.device_t dev))
                {
                    var device = ONIDeviceExpressionBuilderFactory.Make((ONIXDevices.ID)dev.id);
                    if (device != null)
                    {
                        device.HardwareSlot = Configuration.Slot;
                        device.HardwareSlot = Configuration.Slot;
                        device.DeviceIndex.SelectedIndex = dev.idx;
                        propertyGrid.SelectedObject = device;
                    }
                    else
                    {
                        propertyGrid.SelectedObject = null;
                    }
                }
                else
                {
                    propertyGrid.SelectedObject = null;
                }
            }
        }

        private void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            AttemptToConnect();
        }
    }
}
