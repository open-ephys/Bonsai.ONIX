using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{

    [XmlRoot("ONIControllerConfigurationSettings")]
    public class ONIHardwareSlotCollection : KeyedCollection<string, ONIHardwareSlot>
    {
        protected override string GetKeyForItem(ONIHardwareSlot item)
        {
            return item.ToString();
        }
    }
}
