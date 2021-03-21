using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Managed copy of <see cref="oni.Frame"/> with strongly-typed data array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RawDataFrame<T> where T : unmanaged
    {
        public T[] sample;

        public RawDataFrame(oni.Frame frame)
        {
            sample = frame.Data<T>();
            //copy frame-specific properties
            FrameClock = frame.Clock;
            DeviceAddress = frame.DeviceAddress;
        }

        public RawDataFrame(RawDataFrame<T> orig)
        {
            sample = orig.sample;
        }

        public ulong FrameClock { get; private set; }
        public uint DeviceAddress { get; private set; }

    }

}
