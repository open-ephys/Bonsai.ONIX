using System;
using System.ComponentModel;
using System.Linq;

namespace Bonsai.ONIX
{
    // This class cannot have a constructor with parameters because we want to automatically serialize to XML
    public class DeviceIndexSelection
    {

        public event Action IndexChanged;

        protected virtual void OnIndexChanged()
        {
            IndexChanged?.Invoke();
        }

        uint? idx = null;
        [Description("The selected device index.")]
        public uint? SelectedIndex
        {
            get
            {
                return idx;
            }
            set
            {
                idx = value;
                OnIndexChanged();
            }
        }

        private uint[] indices;
        [Description("The valid device indices.")]
        public uint[] Indices
        {
            get { return indices; }
            set
            {
                indices = value;
                if (SelectedIndex == null && value.Count() > 0)
                {
                    SelectedIndex = value[0];
                }
                else if (value.Count() > 0)
                {
                    if (!value.Contains((uint)SelectedIndex))
                        SelectedIndex = value[0];
                    // else no change
                }
                else
                {
                    SelectedIndex = null;
                }
            }
        }

        public override string ToString() => DevIndexToString(SelectedIndex);

        public string DevIndexToString(uint? idx)
        {
            if (idx == null)
                return "";
            else
                return $@"0x{idx:X4} ({(byte)(idx >> 8):D3}.{(byte)(idx >> 0):D3})";
        }

        public void StringToSelection(string str_idx)
        {
            if (str_idx != null)
            {
                string[] words = str_idx.Split(' ');
                SelectedIndex = Convert.ToUInt32(words[0], 16);
            }
        }
    }
}
