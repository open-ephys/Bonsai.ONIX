using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    public class HARPSyncInputDataFrame : U16DataFrame
    {
        public HARPSyncInputDataFrame(ONIManagedFrame<ushort> frame)
            : base(frame)
        {
            HARPTime = ((uint)frame.Sample[4] << 16) | ((uint)frame.Sample[5] << 0);
        }

        public uint HARPTime { get; private set; }
    }
}
