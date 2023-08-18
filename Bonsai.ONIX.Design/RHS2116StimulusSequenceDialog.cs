using System;
using System.Reflection;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class RHS2116StimulusSequenceDialog : Form
    {
        public RHS2116StimulusSequence Sequence;
        private readonly RHS2116StimulusSequenceDrawing sequenceDrawing;

        public RHS2116StimulusSequenceDialog(RHS2116StimulusSequence sequence)
        {

            InitializeComponent();

            // Create a deep copy of the configuration to work with internally that won't 
            // commit changes to config until user clicks "OK"
            Sequence = ObjectExtensions.Copy(sequence);

            // Stimulus sequence drawing
            panelSequenceDrawing.MouseWheel += panelSequenceDrawing_MouseWheel;
            sequenceDrawing = new RHS2116StimulusSequenceDrawing(panelSequenceDrawing);

            // Drawing panel options
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                panelSequenceDrawing, new object[] { true });

            // Immediate update
            dataGridView_StimulusSequence.CellEndEdit += dataGridView_StimulusSequence_CellEndEdit;
            dataGridView_StimulusSequence.DataSource = Sequence.Stimuli;
            dataGridView_StimulusSequence.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            comboBox_StimulatorStepSize.BindingContext = BindingContext;
            comboBox_StimulatorStepSize.DataSource = Enum.GetValues(typeof(RHS2116StimulusSequence.StepSize));
            comboBox_StimulatorStepSize.SelectedItem = Sequence.CurrentStepSize;

            // Need to do here because assigning DataSource above triggers SelectedIndexChanged
            comboBox_StimulatorStepSize.SelectedIndexChanged += new EventHandler(comboBox_StimulatorStepSize_SelectedIndexChanged);

            label_Info.Text = String.Format("1 sample = {0:F2} μsec; 1 step = {1:F2} μA; Max amplitude: {2:F2} μA/phase", 
                RHS2116Device.SamplePeriodMicroSeconds, 
                Sequence.CurrentStepSizeuA, 
                Sequence.MaxPossibleAmplitudePerPhaseMicroAmps);

            SetStatus(Sequence.Valid);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void dataGridView_StimulusSequence_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView_StimulusSequence.BindingContext[dataGridView_StimulusSequence.DataSource].EndCurrentEdit();
            SetStatus(Sequence.Valid);
        }

        private void panelSequenceDrawing_Paint(object sender, PaintEventArgs e)
        {
            
            sequenceDrawing.DrawSequence(Sequence, e.Graphics, panelSequenceDrawing);
        }

        private void panelSequenceDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                panelSequenceDrawing.Cursor = Cursors.NoMove2D;
            }
        }

        private void panelSequenceDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                sequenceDrawing.Drag(e);
            }
            else
            {
                sequenceDrawing.UpdateMouseLocation(e);
            }

            panelSequenceDrawing.Invalidate();
        }

        private void panelSequenceDrawing_MouseHover(object sender, EventArgs e)
        {
            panelSequenceDrawing.Focus();
        }

        private void panelSequenceDrawing_Scroll(object sender, ScrollEventArgs e)
        {
            panelSequenceDrawing.Invalidate();
        }

        private void panelSequenceDrawing_MouseWheel(object sender, MouseEventArgs e)
        {
            sequenceDrawing.UpdateZoom(e);
            panelSequenceDrawing.Invalidate();
        }

        private void dataGridView_StimulusSequence_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView gridView = sender as DataGridView;

            if (null != gridView)
            {
                foreach (DataGridViewRow r in gridView.Rows)
                {
                    gridView.Rows[r.Index].HeaderCell.Value = r.Index.ToString();
                }
            }
            gridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }

        private void comboBox_StimulatorStepSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sequence.CurrentStepSize = (RHS2116StimulusSequence.StepSize)comboBox_StimulatorStepSize.SelectedItem;
            label_Info.Text = String.Format("1 sample = {0:F2} μsec; 1 step = {1:F2} μA; Max amplitude: {2:F2} μA/phase",
                 RHS2116Device.SamplePeriodMicroSeconds,
                 Sequence.CurrentStepSizeuA,
                 Sequence.MaxPossibleAmplitudePerPhaseMicroAmps);
            panelSequenceDrawing.Invalidate();
        }

        private void SetStatus(bool valid)
        {
            toolStripStatusLabel.Text = "";

            if (!Sequence.Valid)
            {
                toolStripStatusLabel.Image = Properties.Resources.StatusWarningImage;
                toolStripStatusLabel.Text = "Stimulus is not well defined.";
                toolStripStatusLabel_numSlots.Text = String.Format(" (? / {0} slots used)", RHS2116Device.StimMemorySlotsAvailable);
                toolStripStatusLabel_numSlots.ForeColor = System.Drawing.Color.Black;
            } else if (!Sequence.FitsInHardware)
            {
                toolStripStatusLabel.Image = Properties.Resources.StatusWarningImage;
                toolStripStatusLabel.Text = "Stimulus is too complex.";
                toolStripStatusLabel_numSlots.Text = String.Format(" ({0} / {1} slots used)", Sequence.StimulusSlotsRequired, RHS2116Device.StimMemorySlotsAvailable);
                toolStripStatusLabel_numSlots.ForeColor = System.Drawing.Color.Red;
            } else
            {
                toolStripStatusLabel.Image = Properties.Resources.StatusReadyImage;
                toolStripStatusLabel.Text = "Stimulus OK.";
                toolStripStatusLabel_numSlots.Text = String.Format(" ({0} / {1} slots used)", Sequence.StimulusSlotsRequired, RHS2116Device.StimMemorySlotsAvailable);
                toolStripStatusLabel_numSlots.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void linkLabelDocumentation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://open-ephys.github.io/onix-docs/Software%20Guide/Bonsai.ONIX/Nodes/RHS2116Device.html");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open documentation link.");
            }
        }

    }
}
