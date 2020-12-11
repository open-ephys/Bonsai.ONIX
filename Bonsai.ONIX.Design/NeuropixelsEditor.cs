using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Bonsai.ONIX.Design
{
    public class NeuropixelsEditor : UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService != null)
            {
                var editorDialog = new NeuropixelsEditorDialog(value as NeuropixelsConfiguration);

                if (editorService.ShowDialog(editorDialog) == DialogResult.OK)
                {
                    return editorDialog.Config;
                }
            }

            return base.EditValue(context, provider, value);
        }
    }
}
