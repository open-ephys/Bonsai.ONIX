using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class ONIXDeviceIDAttribute : System.Attribute
    {
        public ONIXDevices.ID deviceID;

        public ONIXDeviceIDAttribute(ONIXDevices.ID id)
        {
            deviceID = id;
        }
    }
}
