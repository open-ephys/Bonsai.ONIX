using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    public class ONIController //: IDisposable
    {
        public string Driver { get; set; } = "riffa";
        public int Index { get; set; } = 0;
        public int BlockReadSize { get; set; } = 2048;

        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public oni.Context AcqContext { get; private set; }

        public void Refresh()
        {
            // Make sure existing context is fully cleaned up
            if (AcqContext != null)
            {
                AcqContext.Dispose();
            }

            AcqContext = new oni.Context(Driver, Index);
        }


        public bool TryRefresh()
        {
            try
            {
                Refresh();
            } catch (oni.ONIException)
            {
                if (AcqContext != null)
                {
                    AcqContext.Dispose();
                }

                return false;
            }

            return true;
        }


        // Safe ReadRegister
        public int ReadRegister(int dev_index, int register_address)
        {
            if (AcqContext == null || AcqContext.IsClosed)
            {
                throw new oni.ONIException(oni.lib.Error.READFAILURE);
            }

            return (int)AcqContext.ReadRegister((uint)dev_index, (uint)register_address);
        }

        // Safe WriteRegister
        public void WriteRegister(int dev_index, int register_address, int value)
        {
            if (AcqContext == null || AcqContext.IsClosed)
            {
                throw new oni.ONIException(oni.lib.Error.WRITEFAILURE);
            }

            AcqContext.WriteRegister((uint)dev_index, (uint)register_address, (uint)value);
        }

    }
}
