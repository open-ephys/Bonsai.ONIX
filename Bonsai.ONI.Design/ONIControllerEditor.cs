using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Bonsai.ONI.Design
{
    public class ONIControllerEditor : UITypeEditor
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
                var editorDialog = new ONIControllerEditorDialog(value as ONIController);
                if (editorService.ShowDialog(editorDialog) == DialogResult.OK)
                {
                    return editorDialog.CtrlRef;
                }
            }

            return base.EditValue(context, provider, value);
        }


        //public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        //{
        //    var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
        //    if (editorService != null)
        //    {
        //        //var script = value as string;
        //        var editorDialog = new ONIControllerEditorDialog(component);
        //        //editorDialog.Script = script;
        //        if (editorService.ShowDialog(editorDialog) == DialogResult.OK)
        //        {
        //            return true;
        //        }
        //    }

        //    return false; // base.EditValue(context, provider, value);


        //    //if (editorDialog == null)
        //    //{
        //    //    editorDialog = new RiffaConfigurationEditorDialog(component);
        //    //    editorDialog.FormClosed += (sender, e) => editorDialog = null;
        //    //    editorDialog.Show(owner);

        //    //    return true; // Trigger rebuild
        //    //}
        //    //else
        //    //{
        //    //    if (editorDialog.WindowState == FormWindowState.Minimized)
        //    //    {
        //    //        editorDialog.WindowState = FormWindowState.Normal;
        //    //    }
        //    //    editorDialog.Activate();

        //    //    return false;
        //    //}
            
        //}
    }
}
