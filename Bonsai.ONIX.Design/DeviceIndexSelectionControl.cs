using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public partial class DeviceIndexSelectionControl : UserControl
    {
        const float DefaultDpi = 96f;
        //DeviceIndexSelectionEditorService editorService;

        public DeviceIndexSelectionControl(DeviceIndexSelection selection)
            : this(null, selection)
        {
        }

        public DeviceIndexSelectionControl(IServiceProvider provider, DeviceIndexSelection selection)
        {
            InitializeComponent();
            //editorService = new DeviceIndexSelectionEditorService(this, provider);

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                SuspendLayout();

                if (selection.Indices != null)
                {
                    foreach (var idx in selection.Indices)
                    {
                        if (!string.IsNullOrWhiteSpace(selection.DevIndexToString(idx)))
                        {
                            deviceIndexListBox.Items.Add(selection.DevIndexToString(idx));
                        }
                    }
                }

                var drawScale = graphics.DpiY / DefaultDpi;
                deviceIndexListBox.Height = (int)Math.Ceiling(deviceIndexListBox.ItemHeight * deviceIndexListBox.Items.Count * drawScale);
                ResumeLayout();
            }
        }

        public object SelectedValue
        {
            get { return deviceIndexListBox.SelectedItem; }
            set { deviceIndexListBox.SelectedItem = value; }
        }

        public event EventHandler SelectedValueChanged;

        private void OnSelectedValueChanged(EventArgs e)
        {
            SelectedValueChanged?.Invoke(this, e);
        }

        private void deviceIndexListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            OnSelectedValueChanged(e);
        }

        private void deviceIndexListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedValueChanged(e);
        }
    }
}