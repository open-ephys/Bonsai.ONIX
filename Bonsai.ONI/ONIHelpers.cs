using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    public static class ONIHelpers
    {
        public static System.Collections.Generic.Dictionary<int, oni.lib.device_t> FindMachingDevices(oni.Context ctx, oni.Device.DeviceID dev_id)
        {
            // If ID is NULL, then just return all devices because its used for debugging
            if (dev_id == oni.Device.DeviceID.NULL)
            {
                return ctx.DeviceMap;
            }
            else
            {
                // Find all matching devices
                return ctx.DeviceMap.Where(
                    pair => pair.Value.id == (uint)dev_id
                ).ToDictionary(x => x.Key, x => x.Value);
            }
        }

    }
}
