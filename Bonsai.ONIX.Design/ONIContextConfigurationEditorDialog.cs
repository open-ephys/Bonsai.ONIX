using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            TypeDescriptor.GetProperties(typeof(ONIDevice))[nameof(ONIDevice.DeviceAddress)].SetReadOnlyAttribute(true);
        }

        protected override void OnLoad(EventArgs e)
        {
            AttemptToConnect();
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            TypeDescriptor.GetProperties(typeof(ONIDevice))[nameof(ONIDevice.DeviceAddress)].SetReadOnlyAttribute(false);
            base.OnClosed(e);
        }

        private void UpdateDeviceTable(uint selectedHub)
        {
            using (var c = ONIContextManager.ReserveContext(Configuration.Slot))
            {
                var context = c.Context;

                dataGridViewDeviceTable.Rows.Clear();

                for (int i = 0; i < context.DeviceTable.Count; i++)
                {

                    var dev = context.DeviceTable.Values.ElementAt(i);
                    var idx = context.DeviceTable.Keys.ElementAt(i);
                    var hub = (idx & 0x0000FF00) >> 8;

                    if (dev.ID != (int)ONIXDevices.ID.Null && hub == selectedHub)
                    {
                        var ri = dataGridViewDeviceTable.Rows.Add(
                            idx,
                            $@" 0x{(byte)(idx >> 8):X2}.0x{(byte)(idx >> 0):X2}",
                            (ONIXDevices.ID)dev.ID,
                            dev.Version,
                            dev.ReadSize,
                            dev.WriteSize,
                            dev.Description);

                        dataGridViewDeviceTable.Rows[ri].HeaderCell.Value = ri.ToString();
                    }
                }
            }
        }

        private void AttemptToConnect()
        {
            dataGridViewDeviceTable.Rows.Clear();

            tabControlHubs.SelectedTab = tabControlHubs.TabPages[0];
            tabControlHubs.SelectedTab.Text = "";
            while (tabControlHubs.TabPages.Count > 1)
            {
                tabControlHubs.TabPages.RemoveAt(tabControlHubs.TabPages.Count - 1);
            }

            try
            {
                using (var c = ONIContextManager.ReserveContext(Configuration.Slot))
                {
                    var context = c.Context;
                    var hubs = new List<oni.Hub>();

                    foreach (var d in context.DeviceTable.Values)
                    {
                        if (d.ID != (int)ONIXDevices.ID.Null)
                        {
                            try
                            {
                                hubs.Add(context.GetHub(d.Address));
                            }
                            catch (oni.ONIException)
                            {
                                Console.WriteLine($@"Failure to obtain hub information for device at address {d.Address}.");
                            }
                        }
                    }

                    hubs = hubs.GroupBy(h => h.Address).Select(g => g.First()).ToList();

                    if (hubs.Count > 0)
                    {
                        tabControlHubs.TabPages[0].Text = $@"Hub 0x{hubs.ElementAt(0).Address:X2}: " + hubs[0].Description;
                        tabControlHubs.TabPages[0].Tag = hubs[0];

                        foreach (var h in hubs.GetRange(1, hubs.Count - 1))
                        {
                            var text = $@"Hub 0x{h.Address:X2}: " + h.Description;
                            tabControlHubs.TabPages.Add(h.Address.ToString(), text);
                            tabControlHubs.TabPages[h.Address.ToString()].Tag = h;
                        }
                    }

                    UpdateDeviceTable(hubs[0].Address);

                    toolStripSplitButton.Text = Properties.Resources.ONIConnectionSuccess;
                    toolStripSplitButton.ForeColor = Color.Black;

                    if (Configuration.ReadSize < context.MaxReadFrameSize)
                    {
                        Configuration.ReadSize = (int)context.MaxReadFrameSize;
                    }
                    numericUpDownReadSize.Minimum = context.MaxReadFrameSize;
                }
            }
            catch (Exception err)
            {
                if (err is oni.ONIException || err is InvalidProgramException || err is TimeoutException)
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
            f.ShowDialog();
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
                propertyGridSelecteDevice.SelectedObject = null;
                return;
            }

            var row = dataGridViewDeviceTable.SelectedRows[0];
            var deviceIndex = (uint)row.Cells[0].Value;

            using (var c = ONIContextManager.ReserveContext(Configuration.Slot))
            {

                var context = c.Context;
                if (context.DeviceTable.TryGetValue(deviceIndex, out oni.Device dev))
                {
                    var device = ONIDeviceFactory.Make((ONIXDevices.ID)dev.ID);
                    if (device != null)
                    {
                        // Hacky "back door" into ONIDeviceIndexTypeConverter's functionality
                        device.DeviceAddress = new ONIDeviceAddress { HardwareSlot = Configuration.Slot, Address = deviceIndex };
                        propertyGridSelecteDevice.SelectedObject = device;
                    }
                    else
                    {
                        propertyGridSelecteDevice.SelectedObject = null;
                    }
                }
                else
                {
                    propertyGridSelecteDevice.SelectedObject = null;
                }
            }
        }

        private void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            AttemptToConnect();
        }

        private void tabControlHubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewDeviceTable.Parent = tabControlHubs.SelectedTab;
            UpdateDeviceTable(((oni.Hub)tabControlHubs.SelectedTab.Tag).Address);
        }

        private void buttonBlockMin_Click(object sender, EventArgs e)
        {
            using (var c = ONIContextManager.ReserveContext(Configuration.Slot))
            {
                var context = c.Context;
                Configuration.ReadSize = (int)context.MaxReadFrameSize;
                numericUpDownReadSize.Value = Configuration.ReadSize;
            }
        }

        private void boardSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new MultiBoardSyncDialog(Configuration);
            f.ShowDialog();
        }

        private void linkLabelDocumentation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://open-ephys.github.io/onix-docs/Software%20Guide/Bonsai/ONIContext.html");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open documentation link.");
            }
        }

        private void dataGridViewDeviceTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDeviceTable.Columns[e.ColumnIndex].Name == "DeviceID")
            {
                var uri = ONIDeviceDataSheetURIFactory.Make((ONIXDevices.ID)dataGridViewDeviceTable[e.ColumnIndex, e.RowIndex].Value);

                if (uri == null)
                {
                    MessageBox.Show("Unable to open documentation link.");
                    return;
                }

                try
                {
                    System.Diagnostics.Process.Start(uri.ToString());
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to open documentation link.");
                }
            }
        }
    }
}
