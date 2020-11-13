using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bonsai.ONIX
{
    // This class cannot have a constructor with parameters because we want to automatically serialize to XML
    public class ControllerSelection
    {
        [Description("The selected device index.")]
        public ONIController SelectedController {
            get {
                if (SelectedKey != null && controllers.ContainsKey(SelectedKey))
                    return controllers[SelectedKey];
                else
                    return null;
            } 
            set {
                if (controllers.ContainsKey(value.ToString()))
                    SelectedKey = value.ToString(); 
            }
        }

        private Dictionary<string, ONIController> controllers;
        [Description("Available hardware controllers.")]
        public List<ONIController> Controllers {
            get {
                return controllers.Values.ToList(); 
            }
            set {
                controllers = new Dictionary<string, ONIController>();
                foreach (var c in value)
                {
                    controllers.Add(c.ToString(), c);
                }

                if (controllers.Count() > 0 && (SelectedKey == null || !controllers.ContainsKey(SelectedKey)))
                {
                    SelectedKey = controllers.ElementAt(0).Key;
                }
            }
        }

        public string SelectedKey { get; set; } = null;

        public ControllerSelection()
        {
            controllers = new Dictionary<string, ONIController>();
        }

        public override string ToString()
        {
            return SelectedKey;
        }
    }
}
