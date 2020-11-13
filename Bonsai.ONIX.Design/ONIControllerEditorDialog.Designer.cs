namespace Bonsai.ONIX.Design
{
    partial class ONIControllerEditorDialog
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
            this.buttonRefreshContext = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPCIeIndex = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownReadSize = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTipReadSize = new System.Windows.Forms.ToolTip(this.components);
            this.numericUpDownWriteAlloc = new System.Windows.Forms.NumericUpDown();
            this.toolTipIndex = new System.Windows.Forms.ToolTip(this.components);
            this.labelConnected = new System.Windows.Forms.Label();
            this.comboBoxDriver = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridViewDeviceTable = new System.Windows.Forms.DataGridView();
            this.DeviceIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirmwareVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReadSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hubsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPCIeIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteAlloc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceTable)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRefreshContext
            // 
            this.buttonRefreshContext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRefreshContext.Location = new System.Drawing.Point(15, 381);
            this.buttonRefreshContext.Name = "buttonRefreshContext";
            this.buttonRefreshContext.Size = new System.Drawing.Size(85, 23);
            this.buttonRefreshContext.TabIndex = 2;
            this.buttonRefreshContext.Text = "Refresh";
            this.buttonRefreshContext.UseVisualStyleBackColor = true;
            this.buttonRefreshContext.Click += new System.EventHandler(this.buttonRefreshContext_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(252, 367);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Index:";
            // 
            // numericUpDownPCIeIndex
            // 
            this.numericUpDownPCIeIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownPCIeIndex.Location = new System.Drawing.Point(255, 384);
            this.numericUpDownPCIeIndex.Name = "numericUpDownPCIeIndex";
            this.numericUpDownPCIeIndex.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownPCIeIndex.TabIndex = 8;
            this.toolTipIndex.SetToolTip(this.numericUpDownPCIeIndex, "Index of RIFFA PCIe module.");
            this.numericUpDownPCIeIndex.ValueChanged += new System.EventHandler(this.numericUpDownPCIeIndexChanged);
            // 
            // numericUpDownReadSize
            // 
            this.numericUpDownReadSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownReadSize.Location = new System.Drawing.Point(307, 384);
            this.numericUpDownReadSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownReadSize.Name = "numericUpDownReadSize";
            this.numericUpDownReadSize.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownReadSize.TabIndex = 10;
            this.toolTipReadSize.SetToolTip(this.numericUpDownReadSize, "The block read size (bytes). Smaller values have shorter latencies. Larger values" +
        " support higher bandwidth. Defaults to minimum.");
            this.numericUpDownReadSize.ValueChanged += new System.EventHandler(this.numericUpDownReadSizeChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 367);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Block Read Size (Bytes):\r\n";
            // 
            // numericUpDownWriteAlloc
            // 
            this.numericUpDownWriteAlloc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownWriteAlloc.Location = new System.Drawing.Point(433, 384);
            this.numericUpDownWriteAlloc.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownWriteAlloc.Name = "numericUpDownWriteAlloc";
            this.numericUpDownWriteAlloc.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWriteAlloc.TabIndex = 17;
            this.toolTipReadSize.SetToolTip(this.numericUpDownWriteAlloc, "The size of pre-allocated write memory (bytes). Larger values will require less f" +
        "requent allocations.");
            this.numericUpDownWriteAlloc.ValueChanged += new System.EventHandler(this.numericUpDownWriteAlloc_ValueChanged);
            // 
            // labelConnected
            // 
            this.labelConnected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelConnected.AutoSize = true;
            this.labelConnected.ForeColor = System.Drawing.Color.Red;
            this.labelConnected.Location = new System.Drawing.Point(106, 386);
            this.labelConnected.Name = "labelConnected";
            this.labelConnected.Size = new System.Drawing.Size(16, 13);
            this.labelConnected.TabIndex = 11;
            this.labelConnected.Text = "✘";
            // 
            // comboBoxDriver
            // 
            this.comboBoxDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxDriver.FormattingEnabled = true;
            this.comboBoxDriver.Items.AddRange(new object[] {
            "riffa",
            "test"});
            this.comboBoxDriver.Location = new System.Drawing.Point(128, 383);
            this.comboBoxDriver.Name = "comboBoxDriver";
            this.comboBoxDriver.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDriver.TabIndex = 12;
            this.comboBoxDriver.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriver_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(805, 381);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(125, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Driver:";
            // 
            // dataGridViewDeviceTable
            // 
            this.dataGridViewDeviceTable.AllowUserToAddRows = false;
            this.dataGridViewDeviceTable.AllowUserToDeleteRows = false;
            this.dataGridViewDeviceTable.AllowUserToOrderColumns = true;
            this.dataGridViewDeviceTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDeviceTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewDeviceTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridViewDeviceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeviceIndex,
            this.DeviceID,
            this.FirmwareVersion,
            this.ReadSize,
            this.WriteSize,
            this.Description});
            this.dataGridViewDeviceTable.Location = new System.Drawing.Point(15, 27);
            this.dataGridViewDeviceTable.Name = "dataGridViewDeviceTable";
            this.dataGridViewDeviceTable.ReadOnly = true;
            this.dataGridViewDeviceTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewDeviceTable.Size = new System.Drawing.Size(865, 337);
            this.dataGridViewDeviceTable.TabIndex = 15;
            this.dataGridViewDeviceTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewDeviceTable_KeyDown);
            // 
            // DeviceIndex
            // 
            this.DeviceIndex.HeaderText = "Device Index (Hub.Device)";
            this.DeviceIndex.Name = "DeviceIndex";
            this.DeviceIndex.ReadOnly = true;
            this.DeviceIndex.Width = 161;
            // 
            // DeviceID
            // 
            this.DeviceID.HeaderText = "ID";
            this.DeviceID.Name = "DeviceID";
            this.DeviceID.ReadOnly = true;
            this.DeviceID.Width = 43;
            // 
            // FirmwareVersion
            // 
            this.FirmwareVersion.HeaderText = "Firmware Version";
            this.FirmwareVersion.Name = "FirmwareVersion";
            this.FirmwareVersion.ReadOnly = true;
            this.FirmwareVersion.Width = 112;
            // 
            // ReadSize
            // 
            this.ReadSize.HeaderText = "Read Size";
            this.ReadSize.Name = "ReadSize";
            this.ReadSize.ReadOnly = true;
            this.ReadSize.Width = 81;
            // 
            // WriteSize
            // 
            this.WriteSize.HeaderText = "WriteSize";
            this.WriteSize.Name = "WriteSize";
            this.WriteSize.ReadOnly = true;
            this.WriteSize.Width = 77;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 85;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(430, 367);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Write Alloc. Size (Bytes):\r\n";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(892, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hubsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // hubsToolStripMenuItem
            // 
            this.hubsToolStripMenuItem.Name = "hubsToolStripMenuItem";
            this.hubsToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.hubsToolStripMenuItem.Text = "Hubs...";
            this.hubsToolStripMenuItem.Click += new System.EventHandler(this.hubsToolStripMenuItem_Click);
            // 
            // ONIControllerEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 416);
            this.Controls.Add(this.numericUpDownWriteAlloc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataGridViewDeviceTable);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.comboBoxDriver);
            this.Controls.Add(this.labelConnected);
            this.Controls.Add(this.numericUpDownReadSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownPCIeIndex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonRefreshContext);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ONIControllerEditorDialog";
            this.ShowIcon = false;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPCIeIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteAlloc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceTable)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonRefreshContext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPCIeIndex;
        private System.Windows.Forms.NumericUpDown numericUpDownReadSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTipIndex;
        private System.Windows.Forms.ToolTip toolTipReadSize;
        private System.Windows.Forms.Label labelConnected;
        private System.Windows.Forms.ComboBox comboBoxDriver;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridViewDeviceTable;
        private System.Windows.Forms.NumericUpDown numericUpDownWriteAlloc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hubsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirmwareVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReadSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn WriteSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}