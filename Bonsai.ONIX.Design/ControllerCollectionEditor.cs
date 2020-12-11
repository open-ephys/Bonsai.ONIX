using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Bonsai.ONIX.Design
{
    public class ControllerCollectionEditor : UITypeEditor
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
                var control = new ControllerSelectionControl((ControllerSelection)value);
                control.SelectedValue = value;
                control.SelectedValueChanged += delegate { editorService.CloseDropDown(); };
                editorService.DropDownControl(control);
                var a = value as ControllerSelection;
                a.SelectedKey = (string)control.SelectedValue;
                return a;
            }

            return base.EditValue(context, provider, value);
        }
    }
}