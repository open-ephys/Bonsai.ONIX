using System;
using System.Drawing;
using System.Windows.Forms;
using Bonsai.ONI;

namespace Bonsai.Design
{
    public partial class ControllerSelectionControl : UserControl
    {
        const float DefaultDpi = 96f;
        DeviceIndexSelectionEditorService editorService;

        public ControllerSelectionControl(ControllerSelection selection)
            : this(null, selection)
        {
        }

        public ControllerSelectionControl(IServiceProvider provider, ControllerSelection selection)
        {
            InitializeComponent();
            editorService = new DeviceIndexSelectionEditorService(this, provider);

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                SuspendLayout();

                foreach (var c in selection.Controllers)
                {
                    if (!string.IsNullOrWhiteSpace(c.ToString()))
                    {
                        controllerListBox.Items.Add(c.ToString());
                    }
                }

                var drawScale = graphics.DpiY / DefaultDpi;
                controllerListBox.Height = (int)Math.Ceiling(controllerListBox.ItemHeight * controllerListBox.Items.Count * drawScale);
                ResumeLayout();
            }
        }

        public object SelectedValue
        {
            get { return controllerListBox.SelectedItem; }
            set { controllerListBox.SelectedItem = value; }
        }

        public event EventHandler SelectedValueChanged;

        private void OnSelectedValueChanged(EventArgs e)
        {
            SelectedValueChanged?.Invoke(this, e);
        }

        private void controllerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedValueChanged(e);
        }

        private void controllerListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            OnSelectedValueChanged(e);
        }
    }
}