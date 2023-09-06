using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml.Serialization;
using TinyCsvParser;

namespace Bonsai.ONIX.Design
{
    public partial class NeuropixelsV1EditorDialog : Form
    {
        public NeuropixelsV1Configuration Config;
        private readonly NeuropixelsV1Drawing probeDrawing;
        private List<int> selectedElectrodes = new List<int>();
        private bool closeContextMenu = true;

        public NeuropixelsV1EditorDialog(NeuropixelsV1Configuration config)
        {
            InitializeComponent();

            // Create a deep copy of the configuration to work with internally that won't 
            // commit changes to config until user clicks "OK"
            Config = ObjectExtensions.Copy(config);

            // Display probe and config SNs
            CheckStatus();

            // Recall if we need to perform a read check
            performSRReadCheckToolStripMenuItem.Checked = Config.PerformReadCheck;

            // Need to manually add the SelectedIndexChange event handler or assigning the data source will trigger it
            toolStripComboBoxOperationMode.ComboBox.BindingContext = this.BindingContext;
            toolStripComboBoxOperationMode.ComboBox.DataSource = Enum.GetValues(typeof(NeuropixelsV1Configuration.OperationMode));
            toolStripComboBoxOperationMode.ComboBox.SelectedItem = Config.Mode;
            toolStripComboBoxOperationMode.ComboBox.SelectedIndexChanged += new EventHandler(toolStripComboBoxOperationMode_SelectedIndexChanged);

            var combo_col = new DataGridViewComboBoxColumn
            {
                HeaderText = "Bank",
                DataPropertyName = "Bank",
                ValueType = typeof(NeuropixelsV1Channel.ElectrodeBank),
                DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.ElectrodeBank)),
            };
            dataGridView_Channels.Columns.Add(combo_col);

            combo_col = new DataGridViewComboBoxColumn
            {
                HeaderText = "AP Gain",
                DataPropertyName = "APGain",
                ValueType = typeof(NeuropixelsV1Channel.Gain),
                DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.Gain))
            };
            dataGridView_Channels.Columns.Add(combo_col);

            combo_col = new DataGridViewComboBoxColumn
            {
                HeaderText = "LFP Gain",
                DataPropertyName = "LFPGain",
                ValueType = typeof(NeuropixelsV1Channel.Gain),
                DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.Gain))
            };
            dataGridView_Channels.Columns.Add(combo_col);

            combo_col = new DataGridViewComboBoxColumn
            {
                HeaderText = "Reference",
                DataPropertyName = "Reference",
                ValueType = typeof(NeuropixelsV1Channel.Ref),
                DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.Ref))
            };
            dataGridView_Channels.Columns.Add(combo_col);

            // Immediate update
            dataGridView_Channels.CellEndEdit += dataGridView_Channels_CellEndEdit;

            // Bind the data grids
            dataGridView_Channels.DataSource = Config.Channels;
            dataGridView_ADCs.DataSource = Config.ADCs;

            // Drawing panel options
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                panelProbeDrawing, new object[] { true });

            panelProbeDrawing.MouseWheel += panelProbeDrawing_MouseWheel;
            probeDrawing = new NeuropixelsV1Drawing(panelProbeDrawing);

            panelProbeDrawing.MouseMove += panelProbeDrawing_MouseMove;
            panelProbeDrawing.MouseDown += panelProbeDrawing_MouseDown;
            panelProbeDrawing.MouseUp += panelProbeDrawing_MouseUp;
            panelProbeDrawing.PreviewKeyDown += panelProbeDrawing_PreviewKeyDown;

            apGainToolStripComboBox.ComboBox.BindingContext = BindingContext;
            apGainToolStripComboBox.ComboBox.DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.Gain));

            lfpGainToolStripComboBox.ComboBox.BindingContext = BindingContext;
            lfpGainToolStripComboBox.ComboBox.DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.Gain));

            referenceToolStripComboBox.ComboBox.BindingContext = BindingContext;
            referenceToolStripComboBox.ComboBox.DataSource = Enum.GetValues(typeof(NeuropixelsV1Channel.Ref));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView gridView = sender as DataGridView;

            if (null != gridView)
            {
                foreach (DataGridViewRow r in gridView.Rows)
                {
                    gridView.Rows[r.Index].HeaderCell.Value = r.Index.ToString();
                }
            }
            gridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new SaveFileDialog
            {
                Filter = "JSON file|*.json|XML file|*.xml",
                Title = "Export neuropixels configuration",
                FileName = Config.ProbePartNo + "_sn-" + Config.FlexProbeSN.ToString()
            };
            var result = fd.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (result == DialogResult.OK && fd.FileName != "")
            {
                switch (fd.FilterIndex)
                {
                    case 1:
                        var opts = new JsonSerializerOptions
                        {
                            WriteIndented = true
                        };
                        var json = JsonSerializer.Serialize(Config, opts);
                        File.WriteAllText(fd.FileName, json);
                        break;

                    case 2:
                        var text_writer = new StreamWriter(fd.FileName);
                        var ser = new XmlSerializer(typeof(NeuropixelsV1Configuration));
                        ser.Serialize(text_writer, Config);
                        text_writer.Close();
                        break;
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog
            {
                Filter = "JSON file|*.json|XML file|*.xml",
                Title = "Import neuropixels configuration"
            };
            var result = fd.ShowDialog();

            if (result == DialogResult.OK && fd.FileName != "")
            {
                var config_tmp = new NeuropixelsV1Configuration();

                switch (fd.FilterIndex)
                {
                    case 1:
                        var json = File.ReadAllText(fd.FileName);
                        config_tmp = JsonSerializer.Deserialize<NeuropixelsV1Configuration>(json);
                        break;

                    case 2:
                        var xml = new XmlSerializer(typeof(NeuropixelsV1Configuration));
                        var xml_file = new FileStream(fd.FileName, FileMode.Open);
                        config_tmp = (NeuropixelsV1Configuration)xml.Deserialize(xml_file);
                        break;
                }

                if (config_tmp.ConfigProbeSN != Config.FlexProbeSN)
                {
                    var txt = String.Format("Warning, the configuration and probe serial numbers do not match. " +
                        "Do you wish to import the channel configuration from probe {0}?. " +
                        "The current ADC calibration data will be preserved", config_tmp.ConfigProbeSN);
                    result = MessageBox.Show(txt,
                        "Serial number mismatch",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Config.Channels = config_tmp.Channels;
                    }
                }
                else
                {
                    Config = config_tmp;

                    // Bind the data grids
                    dataGridView_Channels.DataSource = Config.Channels;
                    dataGridView_ADCs.DataSource = Config.ADCs;

                    // You need to upload these values
                    Config.RefreshNeeded = true;
                }

                CheckStatus();
            }
        }

        private void loadCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog { ShowNewFolderButton = false };

            if (Config.CalibrationFolderPath != null && Directory.Exists(Config.CalibrationFolderPath))
            {
                fd.RootFolder = Environment.SpecialFolder.Desktop;
                fd.SelectedPath = Config.CalibrationFolderPath;
            }

            fd.Description = String.Format("Select a folder containing the following calibration files:\n" +
                "* {0}_ADCCalibration.csv\n" +
                "* {0}_GainCalValues.csv", Config.FlexProbeSN);

            if (fd.ShowDialog() == DialogResult.OK)
            {
                var adc_cal_fn = String.Format("{0}_ADCCalibration.csv", Config.FlexProbeSN);
                var gain_cal_fn = String.Format("{0}_GainCalValues.csv", Config.FlexProbeSN);

                var adc_path = Directory.GetFiles(fd.SelectedPath, "*" + adc_cal_fn);
                var gain_path = Directory.GetFiles(fd.SelectedPath, "*" + gain_cal_fn);

                if (adc_path.Length != 0 && gain_path.Length != 0)
                {
                    // Check serial numbers inside each file
                    System.IO.StreamReader file = new System.IO.StreamReader(adc_path[0]);
                    Config.ConfigProbeSN = UInt64.Parse(file.ReadLine());
                    file.Close();
                    CheckStatus();

                    file = new System.IO.StreamReader(gain_path[0]);
                    Config.ConfigProbeSN = UInt64.Parse(file.ReadLine());
                    file.Close();
                    CheckStatus();

                    // Parse ADC calibrations
                    CsvParserOptions csv_parser_opts = new CsvParserOptions(true, ',');
                    var adc_csv_mapper = new CsvNeuropixelsADCMapping();
                    var adc_csv_parser = new CsvParser<NeuropixelsV1ADC>(csv_parser_opts, adc_csv_mapper);

                    // Parse ADC calibration parameters
                    var adcs = adc_csv_parser
                        .ReadFromFile(adc_path[0], Encoding.ASCII)
                        .ToList();

                    // Confirm parse
                    if (!adcs.All(x => x.IsValid))
                    {
                        ShowError("Parse Error", adcs.First(x => !x.IsValid).Error.Value);
                        return;
                    }

                    // Check length
                    if (adcs.Count() != Config.ADCs.Length)
                    {
                        ShowError("Parse Error",
                            String.Format("Too many rows in ADC calibration file. There are {0} when " +
                            "there should be {1}.", adcs.Count(), Config.ADCs.Length));
                        return;
                    }

                    // Parse Gains
                    var electrde_csv_mapper = new CsvNeuropixelsElectrodeMapping();
                    var electrde_csv_parser = new CsvParser<NeuropixelsV1GainCorrection>(csv_parser_opts, electrde_csv_mapper);

                    // Parse ADC calibration parameters
                    var electrodes = electrde_csv_parser
                        .ReadFromFile(gain_path[0], Encoding.ASCII)
                        .ToList();

                    // Confirm parse
                    if (!electrodes.All(x => x.IsValid))
                    {
                        ShowError("Parse Error", electrodes.First(x => !x.IsValid).Error.Value);
                        return;
                    }

                    // Check length
                    if (electrodes.Count() != NeuropixelsV1Probe.ELECTRODE_COUNT)
                    {
                        ShowError("Parse Error",
                            String.Format("Incorrect number of gain correction values in calibration file. There are {0} when " +
                            "there should be {1}.", electrodes.Count(), NeuropixelsV1Probe.ELECTRODE_COUNT));
                        return;
                    }

                    // Assign
                    Config.ADCs = adcs.Select(x => x.Result).ToArray();

                    foreach (var c in Config.Channels)
                    {
                        c.UpdateGainCorrections(electrodes.Select(x => x.Result).ToArray());
                    }

                    // Update gridviews
                    dataGridView_ADCs.DataSource = Config.ADCs;

                    // Save for next time
                    Config.CalibrationFolderPath = fd.SelectedPath;

                    // You need to upload these values
                    Config.RefreshNeeded = true;

                }
                else
                {
                    ShowError("Missing calibration files", String.Format("One or both of the calibration" +
                        " files matching the current probe serial number, {0} and {1}, were not found in the " +
                        "specified directory.", adc_cal_fn, gain_cal_fn));
                    return;
                }
            }
        }

        private void CheckStatus()
        {
            toolStripStatusLabel.Text = "";
            toolStripStatusLabel_ProbeSN.Text = "Probe SN: " + Config.FlexProbeSN.ToString();
            toolStripStatusLabel_ConfigSN.Text = "Config SN: " + Config.ConfigProbeSN.ToString();

            if (Config.FlexProbeSN == null || Config.ConfigProbeSN == null || Config.FlexProbeSN != Config.ConfigProbeSN)
            {
                toolStripStatusLabel.Image = Properties.Resources.StatusWarningImage;
                toolStripStatusLabel.Text = "Serial number mismatch";
            }
            else if (Config.RefreshNeeded)
            {
                toolStripStatusLabel.Image = Properties.Resources.StatusWarningImage;
                toolStripStatusLabel.Text = "Upload required";
            }
            else
            {
                toolStripStatusLabel.Image = Properties.Resources.StatusReadyImage;
                toolStripStatusLabel.Text = "";
            }
        }

        private void ShowError(string title, string msg)
        {
            MessageBox.Show(msg,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void dataGridView_Channels_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                e.ContextMenuStrip = contextMenuStrip_ChannelsGrid;
                e.ContextMenuStrip.Tag = new Tuple<int, int>(e.RowIndex, e.ColumnIndex);
            }
        }

        private void applyToColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip_ChannelsGrid.Tag is Tuple<int, int> pos)
            {
                var value = dataGridView_Channels.Rows[pos.Item1].Cells[pos.Item2].Value;

                for (int i = 0; i < dataGridView_Channels.Rows.Count; i++)
                {
                    dataGridView_Channels.Rows[i].Cells[pos.Item2].Value = value;
                }
            }
        }

        private void toolStripComboBoxOperationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var box = sender as ComboBox;
            Enum.TryParse(box.SelectedValue.ToString(), out NeuropixelsV1Configuration.OperationMode mode);
            Config.Mode = mode;
        }

        private void dataGridView_Channels_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView_Channels.BindingContext[dataGridView_Channels.DataSource].EndCurrentEdit();
            Config.RefreshNeeded = true;
            CheckStatus();
        }

        private async void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            using (var probe = new NeuropixelsV1Probe(Config.DeviceAddress))
            {
                probe.FullReset();

                toolStripProgressBar_UploadPogress.Value = 0;
                var progressIndicator = new Progress<int>(ReportProgress);
                int uploads = await probe.WriteConfigurationAsync(Config, progressIndicator, Config.PerformReadCheck);
                CheckStatus();
            }
        }

        private void ReportProgress(int value)
        {
            toolStripProgressBar_UploadPogress.Value = value;
        }

        private void performSRReadCheckToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            Config.PerformReadCheck = performSRReadCheckToolStripMenuItem.Checked;
        }

        // TODO: Massive mess. Will need to make less brittle if we want it to work for other probes.

        private void panelProbeDrawing_Paint(object sender, PaintEventArgs e)
        {
            selectedElectrodes = probeDrawing.DrawProbe(Config, e.Graphics, panelProbeDrawing);
        }

        private void panelProbeDrawing_Scroll(object sender, ScrollEventArgs e)
        {
            panelProbeDrawing.Invalidate();
        }

        private void panelProbeDrawing_MouseHover(object sender, EventArgs e)
        {
            panelProbeDrawing.Focus();
        }

        private void panelProbeDrawing_MouseWheel(object sender, MouseEventArgs e)
        {
            probeDrawing.UpdateZoom(e);
            panelProbeDrawing.Invalidate();
        }

        private void panelProbeDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            probeDrawing.UpdateMouseLocation(e);
            if (e.Button == MouseButtons.Left)
            {
                probeDrawing.ClearSelections();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                panelProbeDrawing.Cursor = Cursors.NoMove2D;
            }
        }

        private void panelProbeDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                probeDrawing.Drag(e, false);
            }
            else if (e.Button == MouseButtons.Left)
            {
                probeDrawing.Drag(e, true);
            }
            else
            {
                probeDrawing.UpdateMouseLocation(e, true);
            }

            panelProbeDrawing.Invalidate();
        }

        private void panelProbeDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            panelProbeDrawing.Cursor = Cursors.Default;
            probeDrawing.MouseUp();
        }

        private void panelProbeDrawing_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                probeDrawing.ClearSelections();
            }
        }

        private void electrodeEnableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var elec in selectedElectrodes)
            {
                Config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(elec)].Bank =
                    NeuropixelsV1Probe.ElectrodeToBank(elec);
            }

            closeContextMenu = false;
            panelProbeDrawing.Invalidate();
        }

        private void aPFilterToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            foreach (var elec in selectedElectrodes)
            {
                Config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(elec)].APFilter =
                    aPFilterToolStripMenuItem.Checked;
            }

            closeContextMenu = false;
        }

        private void standbyToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            foreach (var elec in selectedElectrodes)
            {
                Config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(elec)].Standby =
                    standbyToolStripMenuItem.Checked;
            }

            closeContextMenu = false;
        }

        private void lfpGainToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var elec in selectedElectrodes)
            {
                Config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(elec)].LFPGain = (NeuropixelsV1Channel.Gain)lfpGainToolStripComboBox.SelectedItem;
            }

            closeContextMenu = false;
        }

        private void apGainToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var elec in selectedElectrodes)
            {
                Config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(elec)].APGain = (NeuropixelsV1Channel.Gain)apGainToolStripComboBox.SelectedItem;
            }

            closeContextMenu = false;
        }

        private void referenceToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var elec in selectedElectrodes)
            {
                Config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(elec)].Reference = (NeuropixelsV1Channel.Ref)referenceToolStripComboBox.SelectedItem;
            }

            closeContextMenu = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeContextMenu = true;
        }

        private void contextMenuStrip_Probe_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            e.Cancel = !closeContextMenu;
            closeContextMenu = true;
        }

        private void linkLabelDocumentation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://open-ephys.github.io/onix-docs/Software%20Guide/Bonsai.ONIX/Nodes/NeuropixelsV1Device.html");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open documentation link.");
            }
        }


    }
}
