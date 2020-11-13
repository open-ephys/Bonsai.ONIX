namespace Bonsai.ONIX.Design
{
    partial class ControllerSelectionControl
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
            this.controllerListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // controllerListBox
            // 
            this.controllerListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controllerListBox.FormattingEnabled = true;
            this.controllerListBox.Location = new System.Drawing.Point(0, 0);
            this.controllerListBox.Name = "controllerListBox";
            this.controllerListBox.Size = new System.Drawing.Size(128, 57);
            this.controllerListBox.TabIndex = 0;
            this.controllerListBox.SelectedIndexChanged += new System.EventHandler(this.controllerListBox_SelectedValueChanged);
            // 
            // ControllerSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.controllerListBox);
            this.Name = "ControllerSelectionControl";
            this.Size = new System.Drawing.Size(128, 57);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox controllerListBox;
    }
}
