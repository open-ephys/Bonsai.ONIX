using System.Collections.Generic;
using System.Linq;

namespace Bonsai.ONIX
{
    public static class ONIHelpers
    {
        public static Dictionary<uint, oni.lib.device_t> FindMachingDevices(oni.Context ctx, oni.Device.DeviceID dev_id)
        {
            // Find all matching devices
            return ctx.DeviceTable.Where(
                dev => dev.Value.id == (uint)dev_id
            ).ToDictionary(x => x.Value.idx, x => x.Value);
        }
    }
}
