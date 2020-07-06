using System;
using System.Security.Permissions;

namespace Bonsai.ONI
{
    public class ONIController : IDisposable
    {
        public string Driver { get; set; } = "riffa";
        public int Index { get; set; } = 0;
        public int BlockReadSize { get; set; } = 2048;
        public int WritePreAllocSize { get; set; } = 2048;

        public override string ToString()
        {
            return Driver + "-" + Index.ToString();
        }

        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        public oni.Context AcqContext { get; private set; }

        public void Refresh()
        {
            Dispose();
            AcqContext = new oni.Context(Driver, Index);
        }

        public bool TryRefresh()
        {
            try
            {
                Refresh();
            } catch (oni.ONIException)
            {
                Dispose(); 
                return false;
            }
            return true;
        }

        // Safe ReadRegister
        public uint ReadRegister(uint? dev_index, uint register_address)
        {
            if (AcqContext == null || AcqContext.IsClosed || dev_index == null)
            {
                throw new oni.ONIException(oni.lib.Error.READFAILURE);
            }

            return AcqContext.ReadRegister((uint)dev_index, register_address);
        }

        // Safe WriteRegister
        public void WriteRegister(uint? dev_index, uint register_address, uint value)
        {
            if (AcqContext == null || AcqContext.IsClosed || dev_index == null)
            {
                throw new oni.ONIException(oni.lib.Error.WRITEFAILURE);
            }

            AcqContext.WriteRegister((uint)dev_index, register_address, value);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        protected virtual void Dispose(bool disposing)
        {
            if (AcqContext != null && !AcqContext.IsInvalid)
            {
                // Free the handle
                AcqContext.Dispose();
            }
        }
    }
}
