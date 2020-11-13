using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace Bonsai.ONIX.Design
{
    public class DeviceIndexCollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            
            if (editorService != null)
            {
                var control = new DeviceIndexSelectionControl((DeviceIndexSelection)value);
                control.SelectedValue = value;
                control.SelectedValueChanged += delegate { editorService.CloseDropDown(); };
                editorService.DropDownControl(control);
                ((DeviceIndexSelection)value).StringToSelection((string)control.SelectedValue);
                return value;
            }

            return base.EditValue(context, provider, value);
        }
    }
}