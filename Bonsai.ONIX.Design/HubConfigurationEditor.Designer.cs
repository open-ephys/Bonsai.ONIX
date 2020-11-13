namespace Bonsai.ONIX.Design
{
    partial class HubConfigurationEditor
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
            this.radioButtonAStandard = new System.Windows.Forms.RadioButton();
            this.radioButtonAPassthrough = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonBPassthrough = new System.Windows.Forms.RadioButton();
            this.radioButtonBStandard = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonDPassthrough = new System.Windows.Forms.RadioButton();
            this.radioButtonDStandard = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonCPassthrough = new System.Windows.Forms.RadioButton();
            this.radioButtonCStandard = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonAStandard
            // 
            this.radioButtonAStandard.AutoSize = true;
            this.radioButtonAStandard.Checked = true;
            this.radioButtonAStandard.Location = new System.Drawing.Point(6, 19);
            this.radioButtonAStandard.Name = "radioButtonAStandard";
            this.radioButtonAStandard.Size = new System.Drawing.Size(68, 17);
            this.radioButtonAStandard.TabIndex = 0;
            this.radioButtonAStandard.TabStop = true;
            this.radioButtonAStandard.Text = "Standard";
            this.radioButtonAStandard.UseVisualStyleBackColor = true;
            this.radioButtonAStandard.CheckedChanged += new System.EventHandler(this.radioButtonAStandard_CheckedChanged);
            // 
            // radioButtonAPassthrough
            // 
            this.radioButtonAPassthrough.AutoSize = true;
            this.radioButtonAPassthrough.Location = new System.Drawing.Point(80, 19);
            this.radioButtonAPassthrough.Name = "radioButtonAPassthrough";
            this.radioButtonAPassthrough.Size = new System.Drawing.Size(84, 17);
            this.radioButtonAPassthrough.TabIndex = 1;
            this.radioButtonAPassthrough.Text = "Passthrough";
            this.radioButtonAPassthrough.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonAPassthrough);
            this.groupBox1.Controls.Add(this.radioButtonAStandard);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hub A";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonBPassthrough);
            this.groupBox2.Controls.Add(this.radioButtonBStandard);
            this.groupBox2.Location = new System.Drawing.Point(14, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 50);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hub B";
            // 
            // radioButtonBPassthrough
            // 
            this.radioButtonBPassthrough.AutoSize = true;
            this.radioButtonBPassthrough.Location = new System.Drawing.Point(80, 19);
            this.radioButtonBPassthrough.Name = "radioButtonBPassthrough";
            this.radioButtonBPassthrough.Size = new System.Drawing.Size(84, 17);
            this.radioButtonBPassthrough.TabIndex = 1;
            this.radioButtonBPassthrough.Text = "Passthrough";
            this.radioButtonBPassthrough.UseVisualStyleBackColor = true;
            // 
            // radioButtonBStandard
            // 
            this.radioButtonBStandard.AutoSize = true;
            this.radioButtonBStandard.Checked = true;
            this.radioButtonBStandard.Location = new System.Drawing.Point(6, 19);
            this.radioButtonBStandard.Name = "radioButtonBStandard";
            this.radioButtonBStandard.Size = new System.Drawing.Size(68, 17);
            this.radioButtonBStandard.TabIndex = 0;
            this.radioButtonBStandard.TabStop = true;
            this.radioButtonBStandard.Text = "Standard";
            this.radioButtonBStandard.UseVisualStyleBackColor = true;
            this.radioButtonBStandard.CheckedChanged += new System.EventHandler(this.radioButtonBStandard_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonDPassthrough);
            this.groupBox3.Controls.Add(this.radioButtonDStandard);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(14, 180);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(170, 50);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hub D";
            // 
            // radioButtonDPassthrough
            // 
            this.radioButtonDPassthrough.AutoSize = true;
            this.radioButtonDPassthrough.Location = new System.Drawing.Point(80, 19);
            this.radioButtonDPassthrough.Name = "radioButtonDPassthrough";
            this.radioButtonDPassthrough.Size = new System.Drawing.Size(84, 17);
            this.radioButtonDPassthrough.TabIndex = 1;
            this.radioButtonDPassthrough.Text = "Passthrough";
            this.radioButtonDPassthrough.UseVisualStyleBackColor = true;
            // 
            // radioButtonDStandard
            // 
            this.radioButtonDStandard.AutoSize = true;
            this.radioButtonDStandard.Checked = true;
            this.radioButtonDStandard.Location = new System.Drawing.Point(6, 19);
            this.radioButtonDStandard.Name = "radioButtonDStandard";
            this.radioButtonDStandard.Size = new System.Drawing.Size(68, 17);
            this.radioButtonDStandard.TabIndex = 0;
            this.radioButtonDStandard.TabStop = true;
            this.radioButtonDStandard.Text = "Standard";
            this.radioButtonDStandard.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonCPassthrough);
            this.groupBox4.Controls.Add(this.radioButtonCStandard);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(14, 124);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(170, 50);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Hub C";
            // 
            // radioButtonCPassthrough
            // 
            this.radioButtonCPassthrough.AutoSize = true;
            this.radioButtonCPassthrough.Location = new System.Drawing.Point(80, 19);
            this.radioButtonCPassthrough.Name = "radioButtonCPassthrough";
            this.radioButtonCPassthrough.Size = new System.Drawing.Size(84, 17);
            this.radioButtonCPassthrough.TabIndex = 1;
            this.radioButtonCPassthrough.Text = "Passthrough";
            this.radioButtonCPassthrough.UseVisualStyleBackColor = true;
            // 
            // radioButtonCStandard
            // 
            this.radioButtonCStandard.AutoSize = true;
            this.radioButtonCStandard.Checked = true;
            this.radioButtonCStandard.Location = new System.Drawing.Point(6, 19);
            this.radioButtonCStandard.Name = "radioButtonCStandard";
            this.radioButtonCStandard.Size = new System.Drawing.Size(68, 17);
            this.radioButtonCStandard.TabIndex = 0;
            this.radioButtonCStandard.TabStop = true;
            this.radioButtonCStandard.Text = "Standard";
            this.radioButtonCStandard.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(104, 236);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(14, 236);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // HubConfigurationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 272);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "HubConfigurationEditor";
            this.ShowIcon = false;
            this.Text = "Hub Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonAStandard;
        private System.Windows.Forms.RadioButton radioButtonAPassthrough;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonBPassthrough;
        private System.Windows.Forms.RadioButton radioButtonBStandard;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonDPassthrough;
        private System.Windows.Forms.RadioButton radioButtonDStandard;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButtonCPassthrough;
        private System.Windows.Forms.RadioButton radioButtonCStandard;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}

