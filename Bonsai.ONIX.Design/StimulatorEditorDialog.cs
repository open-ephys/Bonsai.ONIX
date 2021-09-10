using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Bonsai.ONIX.Design
{
    public partial class StimulatorEditorDialog : Form
    {
        // Hold reference to the device being manipulated
        private readonly ONIDevice device;

        public StimulatorEditorDialog(ONIDeviceAddress address)
        {
            InitializeComponent();

            // Make device
            using (var c = ONIContextManager.ReserveContext(address.HardwareSlot))
            {

                var context = c.Context;
                if (context.DeviceTable.TryGetValue((uint)address.Address, out oni.Device dev))
                {
                    device = ONIDeviceFactory.Make((ONIXDevices.ID)dev.ID);
                    if (device != null)
                    {
                        device.DeviceAddress = address;

                        // Bind to properties pane
                        propertyGridStimulator.SelectedObject = device;

                        // Copy Device metadata
                        device.FrameClockHz = c.Context.AcquisitionClockHz;
                        device.Hub = c.Context.GetHub((uint)device.DeviceAddress.Address);
                    }
                    else
                    {
                        propertyGridStimulator.SelectedObject = null;
                    }
                }
                else
                {
                    propertyGridStimulator.SelectedObject = null;
                }
            }

            SetupDisplay();
            DrawWaveform();
            FitWaveform();

            zedGraphWaveform.KeyDown += ZedGraphWaveform_KeyDown;
            zedGraphWaveform.KeyUp += ZedGraphWaveform_KeyUp;
        }

        private void ZedGraphWaveform_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    zedGraphWaveform.IsEnableHZoom = true;
                    break;
                case Keys.ShiftKey:
                    zedGraphWaveform.IsEnableVZoom = true;
                    break;
            }
        }

        private void ZedGraphWaveform_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    zedGraphWaveform.IsEnableHZoom = false;
                    break;
                case Keys.ShiftKey:
                    zedGraphWaveform.IsEnableVZoom = false;
                    break;
            }
        }

        private void propertyGridStimulator_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DrawWaveform();
        }

        private void DrawWaveform()
        {

            zedGraphWaveform.MasterPane.PaneList.ForEach(p => p.CurveList.Clear());

            for (int i = 0; i < StimulatorExtension.NumberOfChannels(device as dynamic); i++)
            {
                var pane = zedGraphWaveform.MasterPane.PaneList[i];

                var curve = pane.AddCurve($"Channel {i}", StimulatorExtension.Waveform(device as dynamic, i), Color.CornflowerBlue, ZedGraph.SymbolType.None);
                curve.Line.Width = 3;
                curve.Line.IsAntiAlias = true;
                curve.Label.IsVisible = false;

                pane.Title.Text = $"Channel {i}";
                var labels = StimulatorExtension.WaveformAxisLabels(device as dynamic);
                pane.XAxis.Title.Text = labels[0];
                pane.YAxis.Title.Text = labels[1];
            }

            zedGraphWaveform.Refresh();
        }

        void SetupDisplay()
        {

            zedGraphWaveform.MasterPane.PaneList.Clear();
            zedGraphWaveform.MasterPane.Border.IsVisible = false;

            for (int i = 0; i < StimulatorExtension.NumberOfChannels(device as dynamic); i++)
            {
                GraphPane pane = new GraphPane();
                zedGraphWaveform.MasterPane.Add(pane);
                pane.Border.IsVisible = false;
                pane.XAxis.MajorGrid.IsVisible = true;
                pane.YAxis.MajorGrid.IsVisible = true;
            }
        }

        void FitWaveform()
        {
            // Refigure the axis ranges for the GraphPanes
            zedGraphWaveform.AxisChange();

            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = CreateGraphics())
            {
                zedGraphWaveform.MasterPane.SetLayout(g, PaneLayout.SingleColumn);
            }

            zedGraphWaveform.MasterPane.PaneList.ForEach(p => p.XAxis.ResetAutoScale(zedGraphWaveform.GraphPane, CreateGraphics()));
            zedGraphWaveform.MasterPane.PaneList.ForEach(p => p.YAxis.ResetAutoScale(zedGraphWaveform.GraphPane, CreateGraphics()));
        }

        private void linkLabelDocumentation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var page = device.GetType().Name;
                System.Diagnostics.Process.Start("https://open-ephys.github.io/onix-docs/Software%20Guide/Bonsai/" + page + ".html");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open documentation link.");
            }
        }
    }
}
