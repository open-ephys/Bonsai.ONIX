
namespace Bonsai.ONIX.Design
{
    partial class MultiBoardSyncDialog
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
            this.groupMode = new System.Windows.Forms.GroupBox();
            this.radioButtonListener = new System.Windows.Forms.RadioButton();
            this.radioButtonSource = new System.Windows.Forms.RadioButton();
            this.radioButonStandalone = new System.Windows.Forms.RadioButton();
            this.groupChannel = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupMode.SuspendLayout();
            this.groupChannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupMode
            // 
            this.groupMode.Controls.Add(this.radioButtonListener);
            this.groupMode.Controls.Add(this.radioButtonSource);
            this.groupMode.Controls.Add(this.radioButonStandalone);
            this.groupMode.Location = new System.Drawing.Point(12, 12);
            this.groupMode.Name = "groupMode";
            this.groupMode.Size = new System.Drawing.Size(226, 49);
            this.groupMode.TabIndex = 0;
            this.groupMode.TabStop = false;
            this.groupMode.Text = "Sync Mode";
            // 
            // radioButtonListener
            // 
            this.radioButtonListener.AutoSize = true;
            this.radioButtonListener.Location = new System.Drawing.Point(157, 20);
            this.radioButtonListener.Name = "radioButtonListener";
            this.radioButtonListener.Size = new System.Drawing.Size(62, 17);
            this.radioButtonListener.TabIndex = 2;
            this.radioButtonListener.TabStop = true;
            this.radioButtonListener.Text = "Listener";
            this.radioButtonListener.UseVisualStyleBackColor = true;
            this.radioButtonListener.CheckedChanged += new System.EventHandler(this.radioButonMode_CheckedChanged);
            // 
            // radioButtonSource
            // 
            this.radioButtonSource.AutoSize = true;
            this.radioButtonSource.Location = new System.Drawing.Point(92, 19);
            this.radioButtonSource.Name = "radioButtonSource";
            this.radioButtonSource.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSource.TabIndex = 1;
            this.radioButtonSource.TabStop = true;
            this.radioButtonSource.Text = "Source";
            this.radioButtonSource.UseVisualStyleBackColor = true;
            this.radioButtonSource.CheckedChanged += new System.EventHandler(this.radioButonMode_CheckedChanged);
            // 
            // radioButonStandalone
            // 
            this.radioButonStandalone.AutoSize = true;
            this.radioButonStandalone.Location = new System.Drawing.Point(7, 20);
            this.radioButonStandalone.Name = "radioButonStandalone";
            this.radioButonStandalone.Size = new System.Drawing.Size(79, 17);
            this.radioButonStandalone.TabIndex = 0;
            this.radioButonStandalone.TabStop = true;
            this.radioButonStandalone.Text = "Standalone";
            this.radioButonStandalone.UseVisualStyleBackColor = true;
            this.radioButonStandalone.CheckedChanged += new System.EventHandler(this.radioButonMode_CheckedChanged);
            // 
            // groupChannel
            // 
            this.groupChannel.Controls.Add(this.radioButton4);
            this.groupChannel.Controls.Add(this.radioButton3);
            this.groupChannel.Controls.Add(this.radioButton2);
            this.groupChannel.Controls.Add(this.radioButton1);
            this.groupChannel.Location = new System.Drawing.Point(12, 81);
            this.groupChannel.Name = "groupChannel";
            this.groupChannel.Size = new System.Drawing.Size(225, 51);
            this.groupChannel.TabIndex = 1;
            this.groupChannel.TabStop = false;
            this.groupChannel.Text = "Sync Channel";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(187, 20);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(31, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(127, 20);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(31, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(67, 20);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(31, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(31, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(14, 138);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(139, 138);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(99, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // MultiBoardSyncDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 170);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupChannel);
            this.Controls.Add(this.groupMode);
            this.Name = "MultiBoardSyncDialog";
            this.ShowIcon = false;
            this.Text = "Host Synchronization";
            this.groupMode.ResumeLayout(false);
            this.groupMode.PerformLayout();
            this.groupChannel.ResumeLayout(false);
            this.groupChannel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMode;
        private System.Windows.Forms.RadioButton radioButonStandalone;
        private System.Windows.Forms.RadioButton radioButtonSource;
        private System.Windows.Forms.RadioButton radioButtonListener;
        private System.Windows.Forms.GroupBox groupChannel;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}