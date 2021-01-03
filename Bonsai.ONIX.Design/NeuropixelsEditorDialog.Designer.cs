namespace Bonsai.ONIX.Design
{
    partial class NeuropixelsEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NeuropixelsEditorDialog));
            this.tabControl_Electrodes = new System.Windows.Forms.TabControl();
            this.tabPage_Channels = new System.Windows.Forms.TabPage();
            this.dataGridView_Channels = new System.Windows.Forms.DataGridView();
            this.tabPage_ADCs = new System.Windows.Forms.TabPage();
            this.dataGridView_ADCs = new System.Windows.Forms.DataGridView();
            this.tabPage_Electrodes = new System.Windows.Forms.TabPage();
            this.dataGridView_Electrodes = new System.Windows.Forms.DataGridView();
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
            this.comboBox_CalibrationMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl_Electrodes.SuspendLayout();
            this.tabPage_Channels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Channels)).BeginInit();
            this.tabPage_ADCs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ADCs)).BeginInit();
            this.tabPage_Electrodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Electrodes)).BeginInit();
            this.contextMenuStrip_ChannelsGrid.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Electrodes
            // 
            this.tabControl_Electrodes.Controls.Add(this.tabPage_Channels);
            this.tabControl_Electrodes.Controls.Add(this.tabPage_ADCs);
            this.tabControl_Electrodes.Controls.Add(this.tabPage_Electrodes);
            this.tabControl_Electrodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Electrodes.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Electrodes.Name = "tabControl_Electrodes";
            this.tabControl_Electrodes.SelectedIndex = 0;
            this.tabControl_Electrodes.Size = new System.Drawing.Size(937, 423);
            this.tabControl_Electrodes.TabIndex = 20;
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
            this.tabPage_ADCs.Size = new System.Drawing.Size(909, 359);
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
            this.dataGridView_ADCs.Size = new System.Drawing.Size(903, 353);
            this.dataGridView_ADCs.TabIndex = 0;
            this.dataGridView_ADCs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            // 
            // tabPage_Electrodes
            // 
            this.tabPage_Electrodes.Controls.Add(this.dataGridView_Electrodes);
            this.tabPage_Electrodes.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Electrodes.Name = "tabPage_Electrodes";
            this.tabPage_Electrodes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Electrodes.Size = new System.Drawing.Size(909, 359);
            this.tabPage_Electrodes.TabIndex = 2;
            this.tabPage_Electrodes.Text = "Electrodes";
            this.tabPage_Electrodes.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Electrodes
            // 
            this.dataGridView_Electrodes.AllowUserToAddRows = false;
            this.dataGridView_Electrodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Electrodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Electrodes.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Electrodes.Name = "dataGridView_Electrodes";
            this.dataGridView_Electrodes.Size = new System.Drawing.Size(903, 353);
            this.dataGridView_Electrodes.TabIndex = 1;
            this.dataGridView_Electrodes.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            // 
            // contextMenuStrip_ChannelsGrid
            // 
            this.contextMenuStrip_ChannelsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applyToColumnToolStripMenuItem});
            this.contextMenuStrip_ChannelsGrid.Name = "contextMenuStrip1";
            this.contextMenuStrip_ChannelsGrid.Size = new System.Drawing.Size(228, 26);
            // 
            // applyToColumnToolStripMenuItem
            // 
            this.applyToColumnToolStripMenuItem.Name = "applyToColumnToolStripMenuItem";
            this.applyToColumnToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.applyToColumnToolStripMenuItem.Text = "Apply value to entire column";
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
            this.fileToolStripMenuItem});
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
            // comboBox_CalibrationMode
            // 
            this.comboBox_CalibrationMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_CalibrationMode.FormattingEnabled = true;
            this.comboBox_CalibrationMode.Location = new System.Drawing.Point(622, 7);
            this.comboBox_CalibrationMode.Name = "comboBox_CalibrationMode";
            this.comboBox_CalibrationMode.Size = new System.Drawing.Size(121, 21);
            this.comboBox_CalibrationMode.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(527, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Calibration Mode:";
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
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.button_Cancel);
            this.panel2.Controls.Add(this.button_OK);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.comboBox_CalibrationMode);
            this.panel2.Location = new System.Drawing.Point(0, 450);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(937, 32);
            this.panel2.TabIndex = 32;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 485);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(937, 22);
            this.statusStrip1.TabIndex = 33;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel.Text = "Probe SN:";
            // 
            // NeuropixelsEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 507);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip2;
            this.Name = "NeuropixelsEditorDialog";
            this.Text = " Neuropixels Configuration";
            this.tabControl_Electrodes.ResumeLayout(false);
            this.tabPage_Channels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Channels)).EndInit();
            this.tabPage_ADCs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ADCs)).EndInit();
            this.tabPage_Electrodes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Electrodes)).EndInit();
            this.contextMenuStrip_ChannelsGrid.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl_Electrodes;
        private System.Windows.Forms.TabPage tabPage_Channels;
        private System.Windows.Forms.TabPage tabPage_ADCs;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.DataGridView dataGridView_ADCs;
        private System.Windows.Forms.TabPage tabPage_Electrodes;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_CalibrationFiles;
        private System.Windows.Forms.DataGridView dataGridView_Channels;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCalibrationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView_Electrodes;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ChannelsGrid;
        private System.Windows.Forms.ToolStripMenuItem applyToColumnToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox_CalibrationMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}