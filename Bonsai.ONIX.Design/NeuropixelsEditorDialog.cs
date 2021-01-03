using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml.Serialization;
using TinyCsvParser;

namespace Bonsai.ONIX.Design
{
    public partial class NeuropixelsEditorDialog : Form
    {
        public NeuropixelsConfiguration Config;

        public NeuropixelsEditorDialog(Bonsai.ONIX.NeuropixelsConfiguration config)
        {
            InitializeComponent();

            // Create a deep copy of the configuration to work with internally that won't 
            // commit changes to config until user clicks "OK"
            Config = ObjectExtensions.Copy(config);

            toolStripStatusLabel.Text = "Probe SN: " + Config.ProbeSN.ToString();

            // Need to manually add the SelectedIndexChange event handler or assigning the data source will trigger it
            comboBox_CalibrationMode.DataSource = Enum.GetValues(typeof(NeuropixelsConfiguration.OperationMode));
            comboBox_CalibrationMode.SelectedItem = Config.Mode;
            comboBox_CalibrationMode.SelectedIndexChanged += new EventHandler(comboBox_CalibrationMode_SelectedIndexChanged);

            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn
            {
                HeaderText = "Bank",
                DataPropertyName = "Bank",
                ValueType = typeof(NeuropixelsChannel.ElectrodeBank),
                DataSource = Enum.GetValues(typeof(NeuropixelsChannel.ElectrodeBank))
            };
            dataGridView_Channels.Columns.Add(col);

            col = new DataGridViewComboBoxColumn
            {
                HeaderText = "AP Gain",
                DataPropertyName = "APGain",
                ValueType = typeof(NeuropixelsChannel.Gain),
                DataSource = Enum.GetValues(typeof(NeuropixelsChannel.Gain))
            };
            dataGridView_Channels.Columns.Add(col);

            col = new DataGridViewComboBoxColumn
            {
                HeaderText = "LFP Gain",
                DataPropertyName = "LFPGain",
                ValueType = typeof(NeuropixelsChannel.Gain),
                DataSource = Enum.GetValues(typeof(NeuropixelsChannel.Gain))
            };
            dataGridView_Channels.Columns.Add(col);

            col = new DataGridViewComboBoxColumn
            {
                HeaderText = "Reference",
                DataPropertyName = "Reference",
                ValueType = typeof(NeuropixelsChannel.Ref),
                DataSource = Enum.GetValues(typeof(NeuropixelsChannel.Ref))
            };
            dataGridView_Channels.Columns.Add(col);

            // Bind the data grids
            dataGridView_Channels.DataSource = Config.Channels; // ch_source;
            dataGridView_ADCs.DataSource = Config.ADCs;
            dataGridView_Electrodes.DataSource = Config.Electrodes;

            // TODO: This does not work
            foreach (var i in Config.InternalReferenceChannels)
            {
                dataGridView_Channels.Rows[i].ReadOnly = true;
                dataGridView_Channels.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                FileName = Config.ProbeType + "_sn-" + Config.ProbeSN.ToString()
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
                        var ser = new XmlSerializer(typeof(NeuropixelsConfiguration));
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
                var config_tmp = new NeuropixelsConfiguration();

                switch (fd.FilterIndex)
                {
                    case 1:
                        var json = File.ReadAllText(fd.FileName);
                        config_tmp = JsonSerializer.Deserialize<NeuropixelsConfiguration>(json);
                        break;

                    case 2:
                        var xml = new XmlSerializer(typeof(NeuropixelsConfiguration));
                        var xml_file = new FileStream(fd.FileName, FileMode.Open);
                        config_tmp = (NeuropixelsConfiguration)xml.Deserialize(xml_file);
                        break;
                }

                if (config_tmp.ProbeSN != Config.ProbeSN)
                {
                    var txt = String.Format("Warning, probe serial numbers do not match. " +
                        "Do you wish to import the channel configuration from probe {0}?. " +
                        "The current ADC calibration data will be preserved", config_tmp.ProbeSN);
                    result = MessageBox.Show(txt,
                        "Serial number mismatch",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Config.Channels = config_tmp.Channels;
                        dataGridView_Channels.DataSource = Config.Channels;
                    }
                }
                else
                {
                    Config = config_tmp;

                    // Bind the data grids
                    dataGridView_Channels.DataSource = Config.Channels;
                    dataGridView_ADCs.DataSource = Config.ADCs;
                    dataGridView_Electrodes.DataSource = Config.Electrodes;

                }
            }
        }

        private void loadCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                var adc_cal_fn = String.Format("{0}_ADCCalibration.csv", Config.ProbeSN);
                var gain_cal_fn = String.Format("{0}_GainCalValues.csv", Config.ProbeSN);

                var adc_path = Directory.GetFiles(fd.SelectedPath, "*" + adc_cal_fn);
                var gain_path = Directory.GetFiles(fd.SelectedPath, "*" + gain_cal_fn);

                if (adc_path.Length != 0 && gain_path.Length != 0)
                {
                    // Check serial numbers inside each file
                    System.IO.StreamReader file = new System.IO.StreamReader(adc_path[0]);
                    var sn = UInt64.Parse(file.ReadLine());
                    file.Close();
                    if (Config.ProbeSN != sn)
                    {
                        ShowError("Serial number mismatch",
                            String.Format("Serial number inside ADC calibration file, {0}," +
                            "does not match the current probe serial number, {1}.", sn, Config.ProbeSN));
                    }

                    file = new System.IO.StreamReader(gain_path[0]);
                    sn = UInt64.Parse(file.ReadLine());
                    file.Close();
                    if (Config.ProbeSN != sn)
                    {
                        ShowError("Serial number mismatch",
                            String.Format("Serial number inside gain values file, {0}," +
                            "does not match the current probe serial number, {1}.", sn, Config.ProbeSN));
                    }

                    // Parse ADC calibrations
                    CsvParserOptions csv_parser_opts = new CsvParserOptions(true, ',');
                    var adc_csv_mapper = new CsvNeuropixelsADCMapping();
                    var adc_csv_parser = new CsvParser<NeuropixelsADC>(csv_parser_opts, adc_csv_mapper);

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
                    var electrde_csv_parser = new CsvParser<NeuropixelsElectrode>(csv_parser_opts, electrde_csv_mapper);

                    // Parse ADC calibration parameters
                    var electrodes = electrde_csv_parser
                        .ReadFromFile(gain_path[0], Encoding.ASCII)
                        .ToList();

                    // Confirm parse
                    if (!adcs.All(x => x.IsValid))
                    {
                        ShowError("Parse Error", electrodes.First(x => !x.IsValid).Error.Value);
                        return;
                    }

                    // Check length
                    if (electrodes.Count() != Config.Electrodes.Length)
                    {
                        ShowError("Parse Error",
                            String.Format("Too many rows in gain values calibration file. There are {0} when " +
                            "there should be {1}.", electrodes.Count(), Config.Electrodes.Length));
                        return;
                    }

                    // Assign
                    Config.ADCs = adcs.Select(x => x.Result).ToArray();
                    Config.Electrodes = electrodes.Select(x => x.Result).ToArray();

                    // Update gridviews
                    dataGridView_ADCs.DataSource = Config.ADCs;
                    dataGridView_Electrodes.DataSource = Config.Electrodes;

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

        void ShowError(string title, string msg)
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

        private void comboBox_CalibrationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var box = sender as ComboBox;
            Enum.TryParse(box.SelectedValue.ToString(), out NeuropixelsConfiguration.OperationMode mode);
            Config.Mode = mode;
        }
    }
}
