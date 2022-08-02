using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace Bonsai.ONIX.Design
{
    public class DocumentationLink : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                var deviceString = context.Instance.GetType().ToString();
                var page = deviceString.Split('.').ToList().Last();
                System.Diagnostics.Process.Start("https://open-ephys.github.io/onix-docs/Software%20Guide/Bonsai.ONIX/" + page + ".html");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open documentation link.");
            }

            return base.EditValue(context, provider, value);
        }
    }
}
