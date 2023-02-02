using System;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Device register configuration with persistant context.
    /// </summary>
    public class RegisterConfiguration : IDisposable
    {
        private readonly ONIContextDisposable context;
        private readonly bool valid;

        public readonly ONIDeviceAddress DeviceAddress;

        public RegisterConfiguration(ONIDeviceAddress address, DeviceID id)
        {
            valid = ONIXDeviceDescriptor.IsValid(id, address);
            context = ONIContextManager.ReserveContext(address.HardwareSlot);
            DeviceAddress = address;

#if DEBUG
            Console.WriteLine("Context reserved by " + this.GetType());
#endif
        }

        public uint? ReadRegister(uint registerAddress)
        {
            if (!valid)
            {
                Console.Error.WriteLine("Register read attempted with an invalid device descriptor.");
                return null;
            }

            if (DeviceAddress.Address == null)
            {
                throw new ArgumentNullException(nameof(DeviceAddress.Address), "Attempt to read register from invalid device.");
            }

            try
            {
                return context.Context.ReadRegister((uint)DeviceAddress.Address, registerAddress);
            }
            catch (oni.ONIException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return null;
            }
        }

        public void WriteRegister(uint registerAddress, uint? value)
        {
            if (!valid)
            {
                Console.Error.WriteLine("Register write attempted with an invalid device descriptor.");
                return;
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Attempt to write null value to register.");
            }

            if (DeviceAddress.Address == null)
            {
                throw new ArgumentNullException(nameof(DeviceAddress.Address), "Attempt to write to register of invalid device.");
            }

            context.Context.WriteRegister((uint)DeviceAddress.Address, registerAddress, (uint)value);
        }

        public void Dispose()
        {
#if DEBUG
            Console.WriteLine("Context disposed by " + this.GetType());
#endif
            context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
