namespace Bonsai.ONIX.Design
{
    partial class StimulatorEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StimulatorEditorDialog));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.propertyGridStimulator = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zedGraphWaveform = new ZedGraph.ZedGraphControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // propertyGridStimulator
            // 
            this.propertyGridStimulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridStimulator.Location = new System.Drawing.Point(0, 0);
            this.propertyGridStimulator.Name = "propertyGridStimulator";
            this.propertyGridStimulator.Size = new System.Drawing.Size(277, 484);
            this.propertyGridStimulator.TabIndex = 19;
            this.propertyGridStimulator.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridStimulator_PropertyValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.zedGraphWaveform);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGridStimulator);
            this.splitContainer1.Size = new System.Drawing.Size(1175, 484);
            this.splitContainer1.SplitterDistance = 894;
            this.splitContainer1.TabIndex = 25;
            // 
            // zedGraphWaveform
            // 
            this.zedGraphWaveform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphWaveform.Location = new System.Drawing.Point(0, 0);
            this.zedGraphWaveform.Name = "zedGraphWaveform";
            this.zedGraphWaveform.ScrollGrace = 0D;
            this.zedGraphWaveform.ScrollMaxX = 0D;
            this.zedGraphWaveform.ScrollMaxY = 0D;
            this.zedGraphWaveform.ScrollMaxY2 = 0D;
            this.zedGraphWaveform.ScrollMinX = 0D;
            this.zedGraphWaveform.ScrollMinY = 0D;
            this.zedGraphWaveform.ScrollMinY2 = 0D;
            this.zedGraphWaveform.Size = new System.Drawing.Size(894, 484);
            this.zedGraphWaveform.TabIndex = 0;
            this.zedGraphWaveform.UseExtendedPrintDialog = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1175, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // linkLabelDocumentation
            // 
            this.linkLabelDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelDocumentation.AutoSize = true;
            this.linkLabelDocumentation.Location = new System.Drawing.Point(1087, 4);
            this.linkLabelDocumentation.Name = "linkLabelDocumentation";
            this.linkLabelDocumentation.Size = new System.Drawing.Size(79, 13);
            this.linkLabelDocumentation.TabIndex = 27;
            this.linkLabelDocumentation.TabStop = true;
            this.linkLabelDocumentation.Text = "Documentation";
            this.linkLabelDocumentation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDocumentation_LinkClicked);
            // 
            // StimulatorEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1175, 508);
            this.Controls.Add(this.linkLabelDocumentation);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "StimulatorEditorDialog";
            this.Text = "Stimulus Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.PropertyGrid propertyGridStimulator;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ZedGraph.ZedGraphControl zedGraphWaveform;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}