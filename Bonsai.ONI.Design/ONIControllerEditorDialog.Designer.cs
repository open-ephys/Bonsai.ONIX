namespace Bonsai.ONI.Design
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
            this.listBoxDeviceTable = new System.Windows.Forms.ListBox();
            this.buttonRefreshContext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPCIeIndex = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownReadSize = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTipReadSize = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipIndex = new System.Windows.Forms.ToolTip(this.components);
            this.labelConnected = new System.Windows.Forms.Label();
            this.comboBoxDriver = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPCIeIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadSize)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxDeviceTable
            // 
            this.listBoxDeviceTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDeviceTable.FormattingEnabled = true;
            this.listBoxDeviceTable.HorizontalScrollbar = true;
            this.listBoxDeviceTable.Location = new System.Drawing.Point(15, 25);
            this.listBoxDeviceTable.Name = "listBoxDeviceTable";
            this.listBoxDeviceTable.Size = new System.Drawing.Size(577, 329);
            this.listBoxDeviceTable.TabIndex = 1;
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Device Table";
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
            this.numericUpDownReadSize.Size = new System.Drawing.Size(107, 20);
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
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Read Size (Bytes):";
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
            "riffa"});
            this.comboBoxDriver.Location = new System.Drawing.Point(128, 383);
            this.comboBoxDriver.Name = "comboBoxDriver";
            this.comboBoxDriver.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDriver.TabIndex = 12;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(517, 381);
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
            // ONIControllerEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 416);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.comboBoxDriver);
            this.Controls.Add(this.labelConnected);
            this.Controls.Add(this.numericUpDownReadSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownPCIeIndex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRefreshContext);
            this.Controls.Add(this.listBoxDeviceTable);
            this.Name = "ONIControllerEditorDialog";
            this.ShowIcon = false;
            this.Text = "ONI Controller Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPCIeIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxDeviceTable;
        private System.Windows.Forms.Button buttonRefreshContext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
    }
}