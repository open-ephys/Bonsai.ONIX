namespace Bonsai.ONIX.Design
{
    partial class NeuropixelsV1EditorDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NeuropixelsV1EditorDialog));
            this.contextMenuStrip_ChannelsGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.applyToColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_OK = new System.Windows.Forms.Button();
            this.folderBrowserDialog_CalibrationFiles = new System.Windows.Forms.FolderBrowserDialog();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCalibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxOperationMode = new System.Windows.Forms.ToolStripComboBox();
            this.performSRReadCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl_Electrodes = new System.Windows.Forms.TabControl();
            this.tabPage_ProbeDrawing = new System.Windows.Forms.TabPage();
            this.panelProbeDrawing = new System.Windows.Forms.Panel();
            this.contextMenuStrip_Probe = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.electrodeEnableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referenceToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.aPGainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apGainToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.lFPGainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lfpGainToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.aPFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standbyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage_Channels = new System.Windows.Forms.TabPage();
            this.dataGridView_Channels = new System.Windows.Forms.DataGridView();
            this.tabPage_ADCs = new System.Windows.Forms.TabPage();
            this.dataGridView_ADCs = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripProgressBar_UploadPogress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel_ProbeSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_ConfigSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            this.contextMenuStrip_ChannelsGrid.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl_Electrodes.SuspendLayout();
            this.tabPage_ProbeDrawing.SuspendLayout();
            this.contextMenuStrip_Probe.SuspendLayout();
            this.tabPage_Channels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Channels)).BeginInit();
            this.tabPage_ADCs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ADCs)).BeginInit();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_ChannelsGrid
            // 
            this.contextMenuStrip_ChannelsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applyToColumnToolStripMenuItem});
            this.contextMenuStrip_ChannelsGrid.Name = "contextMenuStrip1";
            this.contextMenuStrip_ChannelsGrid.Size = new System.Drawing.Size(195, 26);
            // 
            // applyToColumnToolStripMenuItem
            // 
            this.applyToColumnToolStripMenuItem.Name = "applyToColumnToolStripMenuItem";
            this.applyToColumnToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.applyToColumnToolStripMenuItem.Text = "Apply value to column";
            this.applyToColumnToolStripMenuItem.Click += new System.EventHandler(this.applyToColumnToolStripMenuItem_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(749, 6);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(85, 23);
            this.button_OK.TabIndex = 21;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog_CalibrationFiles
            // 
            this.folderBrowserDialog_CalibrationFiles.Description = "Navitage to probe calibration files";
            this.folderBrowserDialog_CalibrationFiles.SelectedPath = "C:\\Users\\open-ephys\\Desktop";
            this.folderBrowserDialog_CalibrationFiles.ShowNewFolderButton = false;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(840, 6);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(85, 23);
            this.button_Cancel.TabIndex = 27;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(937, 24);
            this.menuStrip2.TabIndex = 28;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadCalibrationToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadCalibrationToolStripMenuItem
            // 
            this.loadCalibrationToolStripMenuItem.Name = "loadCalibrationToolStripMenuItem";
            this.loadCalibrationToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.loadCalibrationToolStripMenuItem.Text = "Load Calibration...";
            this.loadCalibrationToolStripMenuItem.Click += new System.EventHandler(this.loadCalibrationToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.importToolStripMenuItem.Text = "Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationModeToolStripMenuItem,
            this.performSRReadCheckToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // operationModeToolStripMenuItem
            // 
            this.operationModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxOperationMode});
            this.operationModeToolStripMenuItem.Name = "operationModeToolStripMenuItem";
            this.operationModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.operationModeToolStripMenuItem.Text = "Operation Mode";
            // 
            // toolStripComboBoxOperationMode
            // 
            this.toolStripComboBoxOperationMode.Name = "toolStripComboBoxOperationMode";
            this.toolStripComboBoxOperationMode.Size = new System.Drawing.Size(121, 23);
            // 
            // performSRReadCheckToolStripMenuItem
            // 
            this.performSRReadCheckToolStripMenuItem.Checked = true;
            this.performSRReadCheckToolStripMenuItem.CheckOnClick = true;
            this.performSRReadCheckToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.performSRReadCheckToolStripMenuItem.Name = "performSRReadCheckToolStripMenuItem";
            this.performSRReadCheckToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.performSRReadCheckToolStripMenuItem.Text = "Verify Upload";
            this.performSRReadCheckToolStripMenuItem.CheckedChanged += new System.EventHandler(this.performSRReadCheckToolStripMenuItem_CheckChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl_Electrodes);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(937, 423);
            this.panel1.TabIndex = 31;
            // 
            // tabControl_Electrodes
            // 
            this.tabControl_Electrodes.Controls.Add(this.tabPage_ProbeDrawing);
            this.tabControl_Electrodes.Controls.Add(this.tabPage_Channels);
            this.tabControl_Electrodes.Controls.Add(this.tabPage_ADCs);
            this.tabControl_Electrodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Electrodes.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Electrodes.Name = "tabControl_Electrodes";
            this.tabControl_Electrodes.SelectedIndex = 0;
            this.tabControl_Electrodes.Size = new System.Drawing.Size(937, 423);
            this.tabControl_Electrodes.TabIndex = 20;
            // 
            // tabPage_ProbeDrawing
            // 
            this.tabPage_ProbeDrawing.Controls.Add(this.panelProbeDrawing);
            this.tabPage_ProbeDrawing.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ProbeDrawing.Name = "tabPage_ProbeDrawing";
            this.tabPage_ProbeDrawing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ProbeDrawing.Size = new System.Drawing.Size(929, 397);
            this.tabPage_ProbeDrawing.TabIndex = 3;
            this.tabPage_ProbeDrawing.Text = "Probe";
            this.tabPage_ProbeDrawing.UseVisualStyleBackColor = true;
            // 
            // panelProbeDrawing
            // 
            this.panelProbeDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProbeDrawing.AutoScroll = true;
            this.panelProbeDrawing.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.panelProbeDrawing.AutoSize = true;
            this.panelProbeDrawing.ContextMenuStrip = this.contextMenuStrip_Probe;
            this.panelProbeDrawing.Location = new System.Drawing.Point(3, 3);
            this.panelProbeDrawing.Name = "panelProbeDrawing";
            this.panelProbeDrawing.Size = new System.Drawing.Size(923, 391);
            this.panelProbeDrawing.TabIndex = 0;
            this.panelProbeDrawing.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelProbeDrawing_Scroll);
            this.panelProbeDrawing.Paint += new System.Windows.Forms.PaintEventHandler(this.panelProbeDrawing_Paint);
            this.panelProbeDrawing.MouseHover += new System.EventHandler(this.panelProbeDrawing_MouseHover);
            // 
            // contextMenuStrip_Probe
            // 
            this.contextMenuStrip_Probe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.electrodeEnableToolStripMenuItem,
            this.referenceToolStripMenuItem,
            this.aPGainToolStripMenuItem,
            this.lFPGainToolStripMenuItem,
            this.aPFilterToolStripMenuItem,
            this.standbyToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip_Probe.Name = "contextMenuStrip_Probe";
            this.contextMenuStrip_Probe.Size = new System.Drawing.Size(127, 158);
            this.contextMenuStrip_Probe.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuStrip_Probe_Closing);
            // 
            // electrodeEnableToolStripMenuItem
            // 
            this.electrodeEnableToolStripMenuItem.Name = "electrodeEnableToolStripMenuItem";
            this.electrodeEnableToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.electrodeEnableToolStripMenuItem.Text = "Enable";
            this.electrodeEnableToolStripMenuItem.Click += new System.EventHandler(this.electrodeEnableToolStripMenuItem_Click);
            // 
            // referenceToolStripMenuItem
            // 
            this.referenceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.referenceToolStripComboBox});
            this.referenceToolStripMenuItem.Name = "referenceToolStripMenuItem";
            this.referenceToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.referenceToolStripMenuItem.Text = "Reference";
            // 
            // referenceToolStripComboBox
            // 
            this.referenceToolStripComboBox.Name = "referenceToolStripComboBox";
            this.referenceToolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.referenceToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.referenceToolStripComboBox_SelectedIndexChanged);
            // 
            // aPGainToolStripMenuItem
            // 
            this.aPGainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.apGainToolStripComboBox});
            this.aPGainToolStripMenuItem.Name = "aPGainToolStripMenuItem";
            this.aPGainToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aPGainToolStripMenuItem.Text = "AP Gain";
            // 
            // apGainToolStripComboBox
            // 
            this.apGainToolStripComboBox.Name = "apGainToolStripComboBox";
            this.apGainToolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.apGainToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.apGainToolStripComboBox_SelectedIndexChanged);
            // 
            // lFPGainToolStripMenuItem
            // 
            this.lFPGainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lfpGainToolStripComboBox});
            this.lFPGainToolStripMenuItem.Name = "lFPGainToolStripMenuItem";
            this.lFPGainToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.lFPGainToolStripMenuItem.Text = "LFP Gain";
            // 
            // lfpGainToolStripComboBox
            // 
            this.lfpGainToolStripComboBox.Name = "lfpGainToolStripComboBox";
            this.lfpGainToolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.lfpGainToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.lfpGainToolStripComboBox_SelectedIndexChanged);
            // 
            // aPFilterToolStripMenuItem
            // 
            this.aPFilterToolStripMenuItem.CheckOnClick = true;
            this.aPFilterToolStripMenuItem.Name = "aPFilterToolStripMenuItem";
            this.aPFilterToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aPFilterToolStripMenuItem.Text = "AP Filter";
            this.aPFilterToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.aPFilterToolStripMenuItem_CheckStateChanged);
            // 
            // standbyToolStripMenuItem
            // 
            this.standbyToolStripMenuItem.CheckOnClick = true;
            this.standbyToolStripMenuItem.Name = "standbyToolStripMenuItem";
            this.standbyToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.standbyToolStripMenuItem.Text = "Standby";
            this.standbyToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.standbyToolStripMenuItem_CheckStateChanged);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.exitToolStripMenuItem.Text = "Cancel";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tabPage_Channels
            // 
            this.tabPage_Channels.Controls.Add(this.dataGridView_Channels);
            this.tabPage_Channels.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Channels.Name = "tabPage_Channels";
            this.tabPage_Channels.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Channels.Size = new System.Drawing.Size(929, 397);
            this.tabPage_Channels.TabIndex = 0;
            this.tabPage_Channels.Text = "Channels";
            this.tabPage_Channels.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Channels
            // 
            this.dataGridView_Channels.AllowUserToAddRows = false;
            this.dataGridView_Channels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Channels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Channels.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Channels.Name = "dataGridView_Channels";
            this.dataGridView_Channels.Size = new System.Drawing.Size(923, 391);
            this.dataGridView_Channels.TabIndex = 0;
            this.dataGridView_Channels.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.dataGridView_Channels_CellContextMenuStripNeeded);
            this.dataGridView_Channels.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            // 
            // tabPage_ADCs
            // 
            this.tabPage_ADCs.Controls.Add(this.dataGridView_ADCs);
            this.tabPage_ADCs.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ADCs.Name = "tabPage_ADCs";
            this.tabPage_ADCs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ADCs.Size = new System.Drawing.Size(929, 397);
            this.tabPage_ADCs.TabIndex = 1;
            this.tabPage_ADCs.Text = "ADCs";
            this.tabPage_ADCs.UseVisualStyleBackColor = true;
            // 
            // dataGridView_ADCs
            // 
            this.dataGridView_ADCs.AllowUserToAddRows = false;
            this.dataGridView_ADCs.AllowUserToDeleteRows = false;
            this.dataGridView_ADCs.AllowUserToOrderColumns = true;
            this.dataGridView_ADCs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ADCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ADCs.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_ADCs.Name = "dataGridView_ADCs";
            this.dataGridView_ADCs.ReadOnly = true;
            this.dataGridView_ADCs.Size = new System.Drawing.Size(923, 391);
            this.dataGridView_ADCs.TabIndex = 0;
            this.dataGridView_ADCs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.button_OK);
            this.panel2.Controls.Add(this.button_Cancel);
            this.panel2.Location = new System.Drawing.Point(0, 450);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(937, 32);
            this.panel2.TabIndex = 32;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton,
            this.toolStripProgressBar_UploadPogress,
            this.toolStripStatusLabel_ProbeSN,
            this.toolStripStatusLabel_ConfigSN,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 485);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(937, 22);
            this.statusStrip1.TabIndex = 33;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSplitButton
            // 
            this.toolStripSplitButton.DropDownButtonWidth = 0;
            this.toolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton.Image")));
            this.toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton.Name = "toolStripSplitButton";
            this.toolStripSplitButton.Size = new System.Drawing.Size(66, 20);
            this.toolStripSplitButton.Text = "Upload";
            this.toolStripSplitButton.ButtonClick += new System.EventHandler(this.toolStripSplitButton_ButtonClick);
            // 
            // toolStripProgressBar_UploadPogress
            // 
            this.toolStripProgressBar_UploadPogress.Name = "toolStripProgressBar_UploadPogress";
            this.toolStripProgressBar_UploadPogress.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel_ProbeSN
            // 
            this.toolStripStatusLabel_ProbeSN.Name = "toolStripStatusLabel_ProbeSN";
            this.toolStripStatusLabel_ProbeSN.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel_ProbeSN.Text = "Probe SN:";
            // 
            // toolStripStatusLabel_ConfigSN
            // 
            this.toolStripStatusLabel_ConfigSN.Name = "toolStripStatusLabel_ConfigSN";
            this.toolStripStatusLabel_ConfigSN.Size = new System.Drawing.Size(67, 17);
            this.toolStripStatusLabel_ConfigSN.Text = "Config. SN:";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Image = global::Bonsai.ONIX.Design.Properties.Resources.StatusReadyImage;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(16, 17);
            // 
            // linkLabelDocumentation
            // 
            this.linkLabelDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelDocumentation.AutoSize = true;
            this.linkLabelDocumentation.Location = new System.Drawing.Point(846, 6);
            this.linkLabelDocumentation.Name = "linkLabelDocumentation";
            this.linkLabelDocumentation.Size = new System.Drawing.Size(79, 13);
            this.linkLabelDocumentation.TabIndex = 34;
            this.linkLabelDocumentation.TabStop = true;
            this.linkLabelDocumentation.Text = "Documentation";
            this.linkLabelDocumentation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDocumentation_LinkClicked);
            // 
            // NeuropixelsV1EditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 507);
            this.Controls.Add(this.linkLabelDocumentation);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip2);
            this.MainMenuStrip = this.menuStrip2;
            this.Name = "NeuropixelsV1EditorDialog";
            this.ShowIcon = false;
            this.Text = " Neuropixels Configuration";
            this.contextMenuStrip_ChannelsGrid.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl_Electrodes.ResumeLayout(false);
            this.tabPage_ProbeDrawing.ResumeLayout(false);
            this.tabPage_ProbeDrawing.PerformLayout();
            this.contextMenuStrip_Probe.ResumeLayout(false);
            this.tabPage_Channels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Channels)).EndInit();
            this.tabPage_ADCs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ADCs)).EndInit();
            this.panel2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_CalibrationFiles;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCalibrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ChannelsGrid;
        private System.Windows.Forms.ToolStripMenuItem applyToColumnToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_UploadPogress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_ProbeSN;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_ConfigSN;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Probe;
        private System.Windows.Forms.ToolStripMenuItem standbyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPGainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lFPGainToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl_Electrodes;
        private System.Windows.Forms.TabPage tabPage_ProbeDrawing;
        private System.Windows.Forms.Panel panelProbeDrawing;
        private System.Windows.Forms.TabPage tabPage_Channels;
        private System.Windows.Forms.DataGridView dataGridView_Channels;
        private System.Windows.Forms.TabPage tabPage_ADCs;
        private System.Windows.Forms.DataGridView dataGridView_ADCs;
        private System.Windows.Forms.ToolStripComboBox referenceToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox apGainToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox lfpGainToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem electrodeEnableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxOperationMode;
        private System.Windows.Forms.ToolStripMenuItem performSRReadCheckToolStripMenuItem;
    }
}