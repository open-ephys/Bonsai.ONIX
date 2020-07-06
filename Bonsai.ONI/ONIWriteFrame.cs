using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    /// <summary>
    /// A frame that can be written to an ONIController
    /// </summary>
    class ONIWriteFrame<T>
    {
        ONIWriteFrame(int dev_idx, T[] data)
        {
            DeviceIndex = dev_idx;
            Data = data;
        }

        public int DeviceIndex { get; private set; }

        public T[] Data { get; private set; }
    }
}
