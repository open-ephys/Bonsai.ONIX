using System.ComponentModel;
using System.Linq;

namespace Bonsai.ONI
{
    // This class cannot have a constructor with parameters because we want to automatically serialize to XML
    public class DeviceIndexSelection
    {
        private bool needs_init = true;

        [Description("The selected device index.")]
        [Range(0, int.MaxValue)]
        public int SelectedIndex { get; set; } = -1;

        private int[] indices;
        [Description("The valid device indices.")]
        public int[] Indices {
            get { return indices; }
            set {
                indices = value;
                if (needs_init || !value.Contains(SelectedIndex))
                {
                    SelectedIndex = value[0];
                    needs_init = false;
                }
            }
        }

        public override string ToString()
        {
            return SelectedIndex.ToString();
        }

        public void StringToSelection(string str_idx)
        {
            if (str_idx != null)
            {
                SelectedIndex = int.Parse(str_idx);
            }
        }
    }
}
