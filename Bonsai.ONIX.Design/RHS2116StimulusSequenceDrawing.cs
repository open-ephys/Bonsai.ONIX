using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public class RHS2116StimulusSequenceDrawing
    {
        private float scale = 1.0f;
        private const float zoomCoeff = 1.5f;
        private const float invZoomCoeff = 1.0f / 1.5f;
        private readonly Point emptyPoint = Point.Empty;

        private PointF translate = PointF.Empty;
        private Point mouseLocation = Point.Empty;
        private Point selectionStart = Point.Empty;
        private Point selectionEnd = Point.Empty;

        public RHS2116StimulusSequenceDrawing(Panel panel)
        {
            translate.X = panel.Width / 20;
            translate.Y = panel.Height / 2;
        }

        // Draws the probe and returns the selected electrode indicies
        public void DrawSequence(RHS2116StimulusSequence sequence, Graphics g, Panel panel)
        {
            var selectLocations = new PointF[] {
                mouseLocation,
                selectionStart,
                selectionEnd,
                emptyPoint,
                new Point(panel.Size) };

            g.ResetTransform();
            g.TranslateTransform(translate.X, translate.Y);
            g.ScaleTransform(scale, scale);
            g.TransformPoints(CoordinateSpace.World, CoordinateSpace.Page, selectLocations);

            var brushRed = new SolidBrush(Color.Red);
            var brushBlack = new SolidBrush(Color.Black);
            var pen = new Pen(brushBlack);

            // If the stimulus is not well defined, then dont draw anything
            if (!sequence.Valid) return;

            var p2p = (float)sequence.MaximumPeakToPeakAmplitudeSteps;
            
            float seqLengthSamples = sequence.SequenceLengthSamples;

            var xScale = seqLengthSamples < 100 ? 100.0f / seqLengthSamples : 1.0f;
            var fontSize = p2p / 4 < 4 ? 4.0f : p2p / 4;

            float yZero = 0;
            float xOffset = pen.Width * 10;
            float yOffset = pen.Width * 5;

            var chLabelFormat = new StringFormat
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center
            };

            for (int i = 0; i < sequence.Stimuli.Length; i++) 
            {
                var stimulus = sequence.Stimuli[i];
                yZero = (p2p + yOffset) * i;

                // Electrode Number
                g.DrawString(i.ToString(), new Font(FontFamily.GenericSansSerif, fontSize, GraphicsUnit.Pixel), brushBlack, new PointF(-12, yZero), chLabelFormat);

                // Stimulus waveform
                var waveform = new List<PointF> { new PointF(-xOffset, yZero), new PointF(stimulus.DelaySamples, yZero) };

                for (int j = 0; j < stimulus.NumberOfStimuli; j++)
                {
                    float amp = stimulus.AnodicFirst ? stimulus.AnodicAmplitudeSteps : -stimulus.CathodicAmplitudeSteps;
                    float width = xScale * (stimulus.AnodicFirst ? stimulus.AnodicWidthSamples : stimulus.CathodicWidthSamples);

                    waveform.Add(new PointF(waveform.Last().X, amp + yZero));
                    waveform.Add(new PointF(waveform.Last().X + width, amp + yZero));
                    waveform.Add(new PointF(waveform.Last().X, yZero));

                    waveform.Add(new PointF(waveform.Last().X + xScale * stimulus.DwellSamples, yZero));

                    amp = stimulus.AnodicFirst ? -stimulus.CathodicAmplitudeSteps : stimulus.AnodicAmplitudeSteps;
                    width = xScale * (stimulus.AnodicFirst ? stimulus.CathodicWidthSamples : stimulus.AnodicWidthSamples);

                    waveform.Add(new PointF(waveform.Last().X, amp + yZero));
                    waveform.Add(new PointF(waveform.Last().X + width, amp + yZero));
                    waveform.Add(new PointF(waveform.Last().X, yZero));

                    waveform.Add(new PointF(waveform.Last().X + xScale * stimulus.InterStimulusIntervalSamples, yZero));

                }

                waveform.Add(new PointF(xScale * seqLengthSamples, yZero));

                g.DrawLines(pen, waveform.ToArray());
            }

            // Draw scale bar
            float yScale = p2p > yOffset ? p2p: yOffset;
            float a = -2f * yScale - pen.Width * 5;
            float b = a + yScale;
            float c = 0.05f * seqLengthSamples;
            var scaleBar = new List<PointF> { new PointF(0, a), new PointF(0, b), new PointF(c * xScale, b) };

            g.DrawLines(pen, scaleBar.ToArray());
            g.DrawString((yScale * sequence.CurrentStepSizeuA).ToString("0.## uA"), new Font(FontFamily.GenericSansSerif, fontSize, GraphicsUnit.Pixel), brushBlack, new PointF(0, a));
            g.DrawString((c * (float)RHS2116Device.SamplePeriodMicroSeconds).ToString("0.## usec"), new Font(FontFamily.GenericSansSerif, fontSize, GraphicsUnit.Pixel), brushBlack, new PointF(c * xScale, b - fontSize / 2));

            // Draw trigger
            pen.Brush = brushRed;
            pen.Width = 0.5f;
            var trigger = new List<PointF> { new PointF(0, -0.5f * p2p), new PointF(0, yZero + p2p) };
            g.DrawLines(pen, trigger.ToArray());

        }

        public void UpdateZoom(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                scale *= zoomCoeff;
                translate.X = e.X - zoomCoeff * (e.X - translate.X);
                translate.Y = e.Y - zoomCoeff * (e.Y - translate.Y);
            }
            else
            {
                scale /= zoomCoeff;
                translate.X = e.X - invZoomCoeff * (e.X - translate.X);
                translate.Y = e.Y - invZoomCoeff * (e.Y - translate.Y);
            }
        }

        public void UpdateMouseLocation(MouseEventArgs e)
        {
            mouseLocation = e.Location;
            selectionStart = e.Location;
            selectionEnd = Point.Empty;
        }

        public void Drag(MouseEventArgs e)
        {
            translate.X += e.Location.X - mouseLocation.X;
            translate.Y += e.Location.Y - mouseLocation.Y;
            mouseLocation = e.Location;
        }
    }
}
