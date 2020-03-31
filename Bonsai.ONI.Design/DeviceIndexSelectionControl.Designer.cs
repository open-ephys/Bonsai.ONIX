namespace Bonsai.Design
{
    partial class DeviceIndexSelectionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.deviceIndexListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // deviceIndexListBox
            // 
            this.deviceIndexListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceIndexListBox.FormattingEnabled = true;
            this.deviceIndexListBox.Location = new System.Drawing.Point(0, 0);
            this.deviceIndexListBox.Name = "deviceIndexListBox";
            this.deviceIndexListBox.Size = new System.Drawing.Size(128, 57);
            this.deviceIndexListBox.TabIndex = 0;
            this.deviceIndexListBox.SelectedIndexChanged += new System.EventHandler(this.deviceIndexListBox_SelectedValueChanged);
            // 
            // DeviceIndexSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deviceIndexListBox);
            this.Name = "DeviceIndexSelectionControl";
            this.Size = new System.Drawing.Size(128, 57);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox deviceIndexListBox;
    }
}
