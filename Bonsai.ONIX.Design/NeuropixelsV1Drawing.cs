using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public class NeuropixelsV1Drawing
    {
        private float scale = 1.0f;
        private const float zoomCoeff = 1.5f;
        private const float invZoomCoeff = 1.0f / 1.5f;
        private readonly Point emptyPoint = Point.Empty;

        private PointF translate = PointF.Empty;
        private Point mouseLocation = Point.Empty;
        private Point selectionStart = Point.Empty;
        private Point selectionEnd = Point.Empty;
        private bool electrodeSelect;
        private bool multiSelect = false;
        private bool drawMaintainedSelection = false;
        private readonly List<int> selectedElectrodes = new List<int>();

        public NeuropixelsV1Drawing(Panel panel)
        {
            translate.X = panel.Width / 20;
            translate.Y = panel.Height / 2;
        }

        // Draws the probe and returns the selected electrode indicies
        public List<int> DrawProbe(NeuropixelsV1Configuration config, Graphics g, Panel panel)
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

            var pen = new Pen(Color.Black);
            var brushGrey = new SolidBrush(Color.Gray);
            var brushOrange = new SolidBrush(Color.Orange);
            var brushBlue = new SolidBrush(Color.CornflowerBlue);
            var brushGreen = new SolidBrush(Color.LightSeaGreen);
            var brushBlack = new SolidBrush(Color.Black);

            // 1 world unit = 1 uM
            const int tipLength = 175;
            const int pixelPitch = 20;
            const int pixelDiam = 12;
            const int shankWidth = 70;
            const int arrayLength = 10000; // (NeuropixelsV1Probe.ELECTRODE_COUNT + 10) * pixelPitch / 2;

            var active_electrodes = config.Channels.Select(x => x.ElectrodeNumber);

            if (!drawMaintainedSelection)
            {
                selectedElectrodes.Clear();
            }

            int k = 0;

            Point mouseLoc = new Point((int)selectLocations[0].X, (int)selectLocations[0].Y);

            var selection = new Rectangle(
                (int)selectLocations[1].X,
                (int)selectLocations[1].Y,
                (int)(selectLocations[2].X - selectLocations[1].X),
                (int)(selectLocations[2].Y - selectLocations[1].Y));

            var panelBounds = new Rectangle(
                (int)selectLocations[3].X,
                (int)selectLocations[3].Y,
                (int)(selectLocations[4].X - selectLocations[3].X),
                (int)(selectLocations[4].Y - selectLocations[3].Y)
            );

            for (int i = 0; i < NeuropixelsV1Probe.ELECTRODE_COUNT; i++)
            {
                int sh = (k % 2) == 0 ? -pixelPitch / 4 : pixelPitch / 4;
                int y_pos = (i % 2) == 0 ? -pixelPitch / 2 : pixelPitch / 2;
                k += i % 2;
                var loc = new Point(tipLength + i / 2 * pixelPitch - pixelDiam / 2, sh + y_pos - pixelDiam / 2);
                var r = new Rectangle(loc, new Size(pixelDiam, pixelDiam));
                var eLoc = new Point(r.Location.X + pixelDiam / 2, r.Location.Y + pixelDiam / 2);

                // If the electrode is not in the panel then ignore it
                if (!RectangleOverlap(r, panelBounds)) continue;

                // Draw selection boundary
                if (selection.Width != 0 && selection.Height != 0)
                {
                    g.DrawRectangle(pen, selection);
                }

                // Draw electrode
                g.DrawRectangle(pen, r);

                // Various electrode fill options
                if (drawMaintainedSelection && selectedElectrodes.Contains(i))
                {
                    g.FillRectangle(brushOrange, r);
                }
                else if (!selection.IsEmpty && electrodeSelect && selection.Contains(eLoc))
                {
                    g.FillRectangle(brushOrange, r);
                    selectedElectrodes.Add(i);
                    multiSelect = true;
                }
                else if (r.Contains(mouseLoc) && electrodeSelect && !drawMaintainedSelection)
                {
                    g.FillRectangle(brushOrange, r);
                    selectedElectrodes.Add(i);
                    multiSelect = false;
                }
                else if (active_electrodes.Contains(i))
                {
                    selectedElectrodes.RemoveAll(x => x == i);

                    if (NeuropixelsV1Probe.ElectrodeToChannel(i) == NeuropixelsV1Probe.INTERNAL_REF_CHANNEL)
                        g.FillRectangle(brushGreen, r);
                    else
                        g.FillRectangle(brushBlue, r);
                }

                // Electrode information
                var c = config.Channels[NeuropixelsV1Probe.ElectrodeToChannel(i)];
                g.DrawString(i.ToString(), new Font(FontFamily.GenericSansSerif, 4, GraphicsUnit.Pixel), brushBlack, loc);

                var info = string.Format("Chan.: {0}\nRef.: {1}\nLFP Gain: {2}\nAP Gain: {3}\nAP Filt.: {4}\nStandby: {5}\nBank: {6}", c.Index, c.Reference, c.LFPGain, c.APGain, c.APFilter, c.Standby, c.Bank);
                g.DrawString(info, new Font(FontFamily.GenericSansSerif, 1, GraphicsUnit.Pixel), brushBlack, new Point(loc.X, loc.Y + 4));
            }

            // Probe outline
            var tip = new Point(0, 0);
            var start_top = new Point(tipLength, shankWidth / 2);
            var end_top = new Point(arrayLength, shankWidth / 2);
            var start_bottom = new Point(tipLength, -shankWidth / 2);
            var end_bottom = new Point(arrayLength, -shankWidth / 2);

            g.DrawLine(pen, tip, start_top);
            g.DrawLine(pen, tip, start_bottom);
            g.DrawLine(pen, start_top, end_top);
            g.DrawLine(pen, start_bottom, end_bottom);

            // Tip reference
            g.FillPolygon(brushGreen, new Point[] {
                new Point(tip.X + 40, tip.Y),
                new Point(start_top.X - 10, start_top.Y - 10),
                new Point(start_bottom.X - 10, start_bottom.Y + 10) });

            // Draw ruler
            var ruler_start = new Point(tip.X, start_top.Y + 10);
            var ruler_end = new Point(ruler_start.X + arrayLength, ruler_start.Y);
            g.DrawLine(pen, ruler_start, ruler_end);

            // Minor ticks
            for (var i = 0; i <= arrayLength; i += 10)
            {
                var tick_start = new Point(ruler_start.X + i, ruler_start.Y);
                var tick_end = new Point(ruler_start.X + i, ruler_start.Y + 4);
                g.DrawLine(pen, tick_start, tick_end);
            }

            // Major ticks
            for (var i = 0; i <= arrayLength; i += 100)
            {
                var tick_start = new Point(ruler_start.X + i, ruler_start.Y);
                var tick_end = new Point(ruler_start.X + i, ruler_start.Y + 12);
                g.DrawLine(pen, tick_start, tick_end);
                g.DrawString((i).ToString() + " μm", new Font(FontFamily.GenericSansSerif, 10), brushBlack, tick_end);
            }

            return selectedElectrodes;
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

        public void UpdateMouseLocation(MouseEventArgs e, bool electrodeSelect = false)
        {
            mouseLocation = e.Location;
            selectionStart = e.Location;
            selectionEnd = Point.Empty;
            this.electrodeSelect = electrodeSelect;
        }

        public void MouseUp()
        {
            drawMaintainedSelection = electrodeSelect && multiSelect;
            if (drawMaintainedSelection)
            {
                electrodeSelect = false;
            }
        }

        public void Drag(MouseEventArgs e, bool electrodeSelect = false)
        {
            this.electrodeSelect = electrodeSelect;

            if (electrodeSelect)
            {
                selectionEnd = e.Location;
            }
            else
            {
                translate.X += e.Location.X - mouseLocation.X;
                translate.Y += e.Location.Y - mouseLocation.Y;
            }

            mouseLocation = e.Location;
        }

        public void ClearSelections()
        {
            electrodeSelect = true;
            drawMaintainedSelection = false;
            selectedElectrodes.Clear();
        }

        private bool RectangleOverlap(Rectangle rect0, Rectangle rect1)
        {
            var p00 = rect0.Location;
            var p01 = new Point(rect0.Location.X + rect0.Width, rect0.Location.Y + rect0.Height);

            var p10 = rect1.Location;
            var p11 = new Point(rect1.Location.X + rect1.Width, rect1.Location.Y + rect1.Height);

            return !(p00.X > p11.X ||
                     p10.X > p01.X ||
                     p10.Y > p01.Y ||
                     p00.Y > p11.Y);
        }
    }
}
