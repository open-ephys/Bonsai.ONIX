namespace Bonsai.ONIX.Design
{
    partial class RHS2116StimulusSequenceDialog
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
            this.contextMenuStrip_ChannelsGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.applyToColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_OK = new System.Windows.Forms.Button();
            this.folderBrowserDialog_CalibrationFiles = new System.Windows.Forms.FolderBrowserDialog();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl_Electrodes = new System.Windows.Forms.TabControl();
            this.tabPage_StimDefinition = new System.Windows.Forms.TabPage();
            this.dataGridView_StimulusSequence = new System.Windows.Forms.DataGridView();
            this.tabPage_StimDrawing = new System.Windows.Forms.TabPage();
            this.panelSequenceDrawing = new System.Windows.Forms.Panel();
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_StepSelect = new System.Windows.Forms.Label();
            this.label_Info = new System.Windows.Forms.Label();
            this.comboBox_StimulatorStepSize = new System.Windows.Forms.ComboBox();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_numSlots = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip_ChannelsGrid.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl_Electrodes.SuspendLayout();
            this.tabPage_StimDefinition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_StimulusSequence)).BeginInit();
            this.tabPage_StimDrawing.SuspendLayout();
            this.contextMenuStrip_Probe.SuspendLayout();
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
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(937, 24);
            this.menuStrip2.TabIndex = 28;
            this.menuStrip2.Text = "menuStrip2";
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
            this.tabControl_Electrodes.Controls.Add(this.tabPage_StimDefinition);
            this.tabControl_Electrodes.Controls.Add(this.tabPage_StimDrawing);
            this.tabControl_Electrodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Electrodes.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Electrodes.Name = "tabControl_Electrodes";
            this.tabControl_Electrodes.SelectedIndex = 0;
            this.tabControl_Electrodes.Size = new System.Drawing.Size(937, 423);
            this.tabControl_Electrodes.TabIndex = 20;
            // 
            // tabPage_StimDefinition
            // 
            this.tabPage_StimDefinition.Controls.Add(this.dataGridView_StimulusSequence);
            this.tabPage_StimDefinition.Location = new System.Drawing.Point(4, 22);
            this.tabPage_StimDefinition.Name = "tabPage_StimDefinition";
            this.tabPage_StimDefinition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_StimDefinition.Size = new System.Drawing.Size(929, 397);
            this.tabPage_StimDefinition.TabIndex = 0;
            this.tabPage_StimDefinition.Text = "Stimulus Definition";
            this.tabPage_StimDefinition.UseVisualStyleBackColor = true;
            // 
            // dataGridView_StimulusSequence
            // 
            this.dataGridView_StimulusSequence.AllowUserToAddRows = false;
            this.dataGridView_StimulusSequence.AllowUserToDeleteRows = false;
            this.dataGridView_StimulusSequence.AllowUserToOrderColumns = true;
            this.dataGridView_StimulusSequence.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView_StimulusSequence.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_StimulusSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_StimulusSequence.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_StimulusSequence.Name = "dataGridView_StimulusSequence";
            this.dataGridView_StimulusSequence.Size = new System.Drawing.Size(923, 391);
            this.dataGridView_StimulusSequence.TabIndex = 0;
            this.dataGridView_StimulusSequence.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_StimulusSequence_DataBindingComplete);
            // 
            // tabPage_StimDrawing
            // 
            this.tabPage_StimDrawing.Controls.Add(this.panelSequenceDrawing);
            this.tabPage_StimDrawing.Location = new System.Drawing.Point(4, 22);
            this.tabPage_StimDrawing.Name = "tabPage_StimDrawing";
            this.tabPage_StimDrawing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_StimDrawing.Size = new System.Drawing.Size(929, 397);
            this.tabPage_StimDrawing.TabIndex = 3;
            this.tabPage_StimDrawing.Text = "Stimulus Waveform";
            this.tabPage_StimDrawing.UseVisualStyleBackColor = true;
            // 
            // panelSequenceDrawing
            // 
            this.panelSequenceDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSequenceDrawing.AutoScroll = true;
            this.panelSequenceDrawing.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.panelSequenceDrawing.AutoSize = true;
            this.panelSequenceDrawing.Location = new System.Drawing.Point(3, 3);
            this.panelSequenceDrawing.Name = "panelSequenceDrawing";
            this.panelSequenceDrawing.Size = new System.Drawing.Size(923, 391);
            this.panelSequenceDrawing.TabIndex = 0;
            this.panelSequenceDrawing.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panelSequenceDrawing_Scroll);
            this.panelSequenceDrawing.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSequenceDrawing_Paint);
            this.panelSequenceDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSequenceDrawing_MouseDown);
            this.panelSequenceDrawing.MouseHover += new System.EventHandler(this.panelSequenceDrawing_MouseHover);
            this.panelSequenceDrawing.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelSequenceDrawing_MouseMove);
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
            // 
            // electrodeEnableToolStripMenuItem
            // 
            this.electrodeEnableToolStripMenuItem.Name = "electrodeEnableToolStripMenuItem";
            this.electrodeEnableToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.electrodeEnableToolStripMenuItem.Text = "Enable";
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
            // 
            // aPFilterToolStripMenuItem
            // 
            this.aPFilterToolStripMenuItem.CheckOnClick = true;
            this.aPFilterToolStripMenuItem.Name = "aPFilterToolStripMenuItem";
            this.aPFilterToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aPFilterToolStripMenuItem.Text = "AP Filter";
            // 
            // standbyToolStripMenuItem
            // 
            this.standbyToolStripMenuItem.CheckOnClick = true;
            this.standbyToolStripMenuItem.Name = "standbyToolStripMenuItem";
            this.standbyToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.standbyToolStripMenuItem.Text = "Standby";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.exitToolStripMenuItem.Text = "Cancel";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label_StepSelect);
            this.panel2.Controls.Add(this.label_Info);
            this.panel2.Controls.Add(this.comboBox_StimulatorStepSize);
            this.panel2.Controls.Add(this.button_OK);
            this.panel2.Controls.Add(this.button_Cancel);
            this.panel2.Location = new System.Drawing.Point(0, 450);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(937, 32);
            this.panel2.TabIndex = 32;
            // 
            // label_StepSelect
            // 
            this.label_StepSelect.AutoSize = true;
            this.label_StepSelect.Location = new System.Drawing.Point(12, 11);
            this.label_StepSelect.Name = "label_StepSelect";
            this.label_StepSelect.Size = new System.Drawing.Size(55, 13);
            this.label_StepSelect.TabIndex = 30;
            this.label_StepSelect.Text = "Step Size:";
            // 
            // label_Info
            // 
            this.label_Info.AutoSize = true;
            this.label_Info.Location = new System.Drawing.Point(200, 11);
            this.label_Info.Name = "label_Info";
            this.label_Info.Size = new System.Drawing.Size(25, 13);
            this.label_Info.TabIndex = 29;
            this.label_Info.Text = "Info";
            // 
            // comboBox_StimulatorStepSize
            // 
            this.comboBox_StimulatorStepSize.FormattingEnabled = true;
            this.comboBox_StimulatorStepSize.Location = new System.Drawing.Point(73, 6);
            this.comboBox_StimulatorStepSize.Name = "comboBox_StimulatorStepSize";
            this.comboBox_StimulatorStepSize.Size = new System.Drawing.Size(121, 21);
            this.comboBox_StimulatorStepSize.TabIndex = 28;
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabel_numSlots});
            this.statusStrip1.Location = new System.Drawing.Point(0, 485);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(937, 22);
            this.statusStrip1.TabIndex = 33;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Image = global::Bonsai.ONIX.Design.Properties.Resources.StatusReadyImage;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(16, 17);
            // 
            // toolStripStatusLabel_numSlots
            // 
            this.toolStripStatusLabel_numSlots.Name = "toolStripStatusLabel_numSlots";
            this.toolStripStatusLabel_numSlots.Size = new System.Drawing.Size(85, 17);
            this.toolStripStatusLabel_numSlots.Text = "(?/? slots used)";
            // 
            // RHS2116StimulusSequenceDialog
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
            this.Name = "RHS2116StimulusSequenceDialog";
            this.ShowIcon = false;
            this.Text = "RHS2116 Stimulus Sequence Configuration";
            this.contextMenuStrip_ChannelsGrid.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl_Electrodes.ResumeLayout(false);
            this.tabPage_StimDefinition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_StimulusSequence)).EndInit();
            this.tabPage_StimDrawing.ResumeLayout(false);
            this.tabPage_StimDrawing.PerformLayout();
            this.contextMenuStrip_Probe.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ChannelsGrid;
        private System.Windows.Forms.ToolStripMenuItem applyToColumnToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Probe;
        private System.Windows.Forms.ToolStripMenuItem standbyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPGainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lFPGainToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl_Electrodes;
        private System.Windows.Forms.TabPage tabPage_StimDrawing;
        private System.Windows.Forms.Panel panelSequenceDrawing;
        private System.Windows.Forms.TabPage tabPage_StimDefinition;
        private System.Windows.Forms.DataGridView dataGridView_StimulusSequence;
        private System.Windows.Forms.ToolStripComboBox referenceToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox apGainToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox lfpGainToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem electrodeEnableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
        private System.Windows.Forms.Label label_Info;
        private System.Windows.Forms.ComboBox comboBox_StimulatorStepSize;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Label label_StepSelect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_numSlots;
    }
}