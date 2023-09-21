namespace Bonsai.ONIX.Design
{
    partial class ONIContextConfigurationEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ONIContextConfigurationEditorDialog));
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPCIeIndex = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownReadSize = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTipReadSize = new System.Windows.Forms.ToolTip(this.components);
            this.numericUpDownWriteAlloc = new System.Windows.Forms.NumericUpDown();
            this.comboBoxDriver = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hubsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boardSyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.propertyGridSelecteDevice = new System.Windows.Forms.PropertyGrid();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonBlockMin = new System.Windows.Forms.Button();
            this.dataGridViewDeviceTable = new System.Windows.Forms.DataGridView();
            this.DeviceID = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReadSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirmwareVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlHubs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPCIeIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteAlloc)).BeginInit();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceTable)).BeginInit();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlHubs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Slot:";
            // 
            // numericUpDownPCIeIndex
            // 
            this.numericUpDownPCIeIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownPCIeIndex.Location = new System.Drawing.Point(208, 34);
            this.numericUpDownPCIeIndex.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownPCIeIndex.Name = "numericUpDownPCIeIndex";
            this.numericUpDownPCIeIndex.Size = new System.Drawing.Size(69, 26);
            this.numericUpDownPCIeIndex.TabIndex = 8;
            this.toolTipReadSize.SetToolTip(this.numericUpDownPCIeIndex, "The index of the host acquisition hardware within the host computer.");
            this.numericUpDownPCIeIndex.ValueChanged += new System.EventHandler(this.numericUpDownPCIeIndex_ValueChanged);
            // 
            // numericUpDownReadSize
            // 
            this.numericUpDownReadSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownReadSize.Location = new System.Drawing.Point(286, 34);
            this.numericUpDownReadSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownReadSize.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownReadSize.Name = "numericUpDownReadSize";
            this.numericUpDownReadSize.Size = new System.Drawing.Size(180, 26);
            this.numericUpDownReadSize.TabIndex = 10;
            this.toolTipReadSize.SetToolTip(this.numericUpDownReadSize, "The block read size (bytes). Smaller values have shorter latencies. Larger values" +
        " support higher bandwidth. Defaults to minimum.");
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 9);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Block Read Size (Bytes):\r\n";
            // 
            // numericUpDownWriteAlloc
            // 
            this.numericUpDownWriteAlloc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownWriteAlloc.Location = new System.Drawing.Point(476, 34);
            this.numericUpDownWriteAlloc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownWriteAlloc.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownWriteAlloc.Name = "numericUpDownWriteAlloc";
            this.numericUpDownWriteAlloc.Size = new System.Drawing.Size(180, 26);
            this.numericUpDownWriteAlloc.TabIndex = 17;
            this.toolTipReadSize.SetToolTip(this.numericUpDownWriteAlloc, "The size of pre-allocated write memory (bytes). Larger values will require less f" +
        "requent allocations.");
            // 
            // comboBoxDriver
            // 
            this.comboBoxDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxDriver.FormattingEnabled = true;
            this.comboBoxDriver.Items.AddRange(new object[] {
            "riffa",
            "test",
            "ft600"});
            this.comboBoxDriver.Location = new System.Drawing.Point(18, 32);
            this.comboBoxDriver.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxDriver.Name = "comboBoxDriver";
            this.comboBoxDriver.Size = new System.Drawing.Size(180, 28);
            this.comboBoxDriver.TabIndex = 12;
            this.toolTipReadSize.SetToolTip(this.comboBoxDriver, "The ONI translation device driver.");
            this.comboBoxDriver.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriver_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(1575, 32);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(112, 32);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Driver:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(472, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(181, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Write Alloc. Size (Bytes):\r\n";
            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1701, 35);
            this.menuStrip.TabIndex = 18;
            this.menuStrip.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hubsToolStripMenuItem,
            this.boardSyncToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(92, 29);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // hubsToolStripMenuItem
            // 
            this.hubsToolStripMenuItem.Name = "hubsToolStripMenuItem";
            this.hubsToolStripMenuItem.Size = new System.Drawing.Size(205, 34);
            this.hubsToolStripMenuItem.Text = "Hubs...";
            this.hubsToolStripMenuItem.Click += new System.EventHandler(this.hubsToolStripMenuItem_Click);
            // 
            // boardSyncToolStripMenuItem
            // 
            this.boardSyncToolStripMenuItem.Name = "boardSyncToolStripMenuItem";
            this.boardSyncToolStripMenuItem.Size = new System.Drawing.Size(205, 34);
            this.boardSyncToolStripMenuItem.Text = "Host Sync...";
            this.boardSyncToolStripMenuItem.Click += new System.EventHandler(this.boardSyncToolStripMenuItem_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // propertyGridSelecteDevice
            // 
            this.propertyGridSelecteDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSelecteDevice.Location = new System.Drawing.Point(0, 0);
            this.propertyGridSelecteDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGridSelecteDevice.Name = "propertyGridSelecteDevice";
            this.propertyGridSelecteDevice.Size = new System.Drawing.Size(381, 628);
            this.propertyGridSelecteDevice.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonBlockMin);
            this.panel3.Controls.Add(this.okButton);
            this.panel3.Controls.Add(this.numericUpDownWriteAlloc);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.numericUpDownReadSize);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.comboBoxDriver);
            this.panel3.Controls.Add(this.numericUpDownPCIeIndex);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 668);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.MinimumSize = new System.Drawing.Size(0, 71);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1701, 71);
            this.panel3.TabIndex = 23;
            // 
            // buttonBlockMin
            // 
            this.buttonBlockMin.Location = new System.Drawing.Point(664, 32);
            this.buttonBlockMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonBlockMin.Name = "buttonBlockMin";
            this.buttonBlockMin.Size = new System.Drawing.Size(102, 32);
            this.buttonBlockMin.TabIndex = 18;
            this.buttonBlockMin.Text = "Set to Min.";
            this.buttonBlockMin.UseVisualStyleBackColor = true;
            this.buttonBlockMin.Click += new System.EventHandler(this.buttonBlockMin_Click);
            // 
            // dataGridViewDeviceTable
            // 
            this.dataGridViewDeviceTable.AllowUserToAddRows = false;
            this.dataGridViewDeviceTable.AllowUserToDeleteRows = false;
            this.dataGridViewDeviceTable.AllowUserToOrderColumns = true;
            this.dataGridViewDeviceTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewDeviceTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridViewDeviceTable.ColumnHeadersHeight = 34;
            this.dataGridViewDeviceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeviceID,
            this.Description,
            this.Address,
            this.ReadSize,
            this.WriteSize,
            this.FirmwareVersion});
            this.dataGridViewDeviceTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDeviceTable.Location = new System.Drawing.Point(4, 5);
            this.dataGridViewDeviceTable.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewDeviceTable.MultiSelect = false;
            this.dataGridViewDeviceTable.Name = "dataGridViewDeviceTable";
            this.dataGridViewDeviceTable.ReadOnly = true;
            this.dataGridViewDeviceTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewDeviceTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDeviceTable.Size = new System.Drawing.Size(1298, 585);
            this.dataGridViewDeviceTable.TabIndex = 20;
            this.dataGridViewDeviceTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDeviceTable_CellContentClick);
            this.dataGridViewDeviceTable.SelectionChanged += new System.EventHandler(this.dataGridViewDeviceTable_SelectionChanged);
            // 
            // DeviceID
            // 
            this.DeviceID.HeaderText = "Device";
            this.DeviceID.MinimumWidth = 8;
            this.DeviceID.Name = "DeviceID";
            this.DeviceID.ReadOnly = true;
            this.DeviceID.Width = 63;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 8;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Address
            // 
            this.Address.HeaderText = "Address (Hub.Dev)";
            this.Address.MinimumWidth = 8;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Width = 180;
            // 
            // ReadSize
            // 
            this.ReadSize.HeaderText = "Read Size (Bytes)";
            this.ReadSize.MinimumWidth = 8;
            this.ReadSize.Name = "ReadSize";
            this.ReadSize.ReadOnly = true;
            this.ReadSize.Width = 173;
            // 
            // WriteSize
            // 
            this.WriteSize.HeaderText = "Write Size (Bytes)";
            this.WriteSize.MinimumWidth = 8;
            this.WriteSize.Name = "WriteSize";
            this.WriteSize.ReadOnly = true;
            this.WriteSize.Width = 171;
            // 
            // FirmwareVersion
            // 
            this.FirmwareVersion.HeaderText = "Firmware Version";
            this.FirmwareVersion.MinimumWidth = 8;
            this.FirmwareVersion.Name = "FirmwareVersion";
            this.FirmwareVersion.ReadOnly = true;
            this.FirmwareVersion.Width = 168;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton});
            this.statusStrip.Location = new System.Drawing.Point(0, 739);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(1701, 32);
            this.statusStrip.TabIndex = 20;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripSplitButton
            // 
            this.toolStripSplitButton.DropDownButtonWidth = 0;
            this.toolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton.Image")));
            this.toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton.Name = "toolStripSplitButton";
            this.toolStripSplitButton.Size = new System.Drawing.Size(99, 29);
            this.toolStripSplitButton.Text = "Refresh";
            this.toolStripSplitButton.ButtonClick += new System.EventHandler(this.toolStripSplitButton_ButtonClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 42);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlHubs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridSelecteDevice);
            this.splitContainer1.Size = new System.Drawing.Size(1701, 628);
            this.splitContainer1.SplitterDistance = 1314;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 25;
            // 
            // tabControlHubs
            // 
            this.tabControlHubs.Controls.Add(this.tabPage1);
            this.tabControlHubs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlHubs.Location = new System.Drawing.Point(0, 0);
            this.tabControlHubs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlHubs.Name = "tabControlHubs";
            this.tabControlHubs.SelectedIndex = 0;
            this.tabControlHubs.Size = new System.Drawing.Size(1314, 628);
            this.tabControlHubs.TabIndex = 21;
            this.tabControlHubs.SelectedIndexChanged += new System.EventHandler(this.tabControlHubs_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewDeviceTable);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(1306, 595);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // linkLabelDocumentation
            // 
            this.linkLabelDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelDocumentation.AutoSize = true;
            this.linkLabelDocumentation.Location = new System.Drawing.Point(1569, 6);
            this.linkLabelDocumentation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabelDocumentation.Name = "linkLabelDocumentation";
            this.linkLabelDocumentation.Size = new System.Drawing.Size(118, 20);
            this.linkLabelDocumentation.TabIndex = 26;
            this.linkLabelDocumentation.TabStop = true;
            this.linkLabelDocumentation.Text = "Documentation";
            this.linkLabelDocumentation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDocumentation_LinkClicked);
            // 
            // ONIContextConfigurationEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1701, 771);
            this.Controls.Add(this.linkLabelDocumentation);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ONIContextConfigurationEditorDialog";
            this.Text = " ONI Acqusition Context";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPCIeIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteAlloc)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceTable)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlHubs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPCIeIndex;
        private System.Windows.Forms.NumericUpDown numericUpDownReadSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTipReadSize;
        private System.Windows.Forms.ComboBox comboBoxDriver;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownWriteAlloc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hubsToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.PropertyGrid propertyGridSelecteDevice;
        private System.Windows.Forms.Panel panel3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton;
        private System.Windows.Forms.DataGridView dataGridViewDeviceTable;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControlHubs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripMenuItem boardSyncToolStripMenuItem;
        private System.Windows.Forms.Button buttonBlockMin;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
        private System.Windows.Forms.DataGridViewLinkColumn DeviceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReadSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn WriteSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirmwareVersion;
    }
}