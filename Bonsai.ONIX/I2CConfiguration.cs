using System;
using System.Linq;
using System.Text;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Device configuration using the bus_to_i2c_raw.vhd core. Converts the ONI configuration programming interface into I2C.
    /// </summary>
    public class I2CConfiguration : IDisposable
    {
        readonly ONIContextDisposable ctx;
        readonly uint? dev_idx;

        public readonly uint I2C_ADDR;

        public I2CConfiguration(ONIDeviceAddress device, uint i2c_addr)
        {
            ctx = ONIContextManager.ReserveContext(device.HardwareSlot);
            dev_idx = device.Address;
            I2C_ADDR = i2c_addr;

#if DEBUG
            Console.WriteLine("I2C context reserved by " + this.GetType());
#endif
        }

        uint? ReadRegister(uint? dev_index, uint register_address)
        {
            if (dev_index == null)
            {
                throw new ArgumentNullException("dev_index", "Attempt to read register from invalid device.");
            }

            try
            {
                return ctx.Context.ReadRegister((uint)dev_index, register_address);
            }
            catch (oni.ONIException)
            {
                return null;
            }
        }

        void WriteRegister(uint? dev_index, uint register_address, uint? value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "Attempt to write null value to register.");
            }

            if (dev_index == null)
            {
                throw new ArgumentNullException("dev_index", "Attempt to write to register of invalid device.");
            }

            ctx.Context.WriteRegister((uint)dev_index, register_address, (uint)value);
        }

        public uint? ReadManagedRegister(uint register_address)
        {
            return ReadRegister(dev_idx, register_address);
        }

        public void WriteManagedRegister(uint register_address, uint value)
        {
            WriteRegister(dev_idx, register_address, value);
        }

        public byte? ReadByte(uint addr)
        {
            uint reg_addr = (addr << 8) | I2C_ADDR;
            var val = ReadRegister(dev_idx, reg_addr);

            if (val != null && val <= byte.MaxValue)
            {
                return (byte?)val;
            }
            else
            {
                return null;
            }
        }

        public void WriteByte(uint addr, uint value)
        {
            uint reg_addr = (addr << 8) | I2C_ADDR;
            WriteRegister(dev_idx, reg_addr, value);
        }

        public byte[] ReadBytes(uint offset, int size)
        {
            var data = new byte[size];

            for (uint i = 0; i < size; i++)
            {
                uint reg_addr = ((offset + i) << 8) | I2C_ADDR;
                var val = ReadRegister(dev_idx, reg_addr);

                if (val != null && val <= byte.MaxValue)
                {
                    data[i] = (byte)val;
                }
                else
                {
                    return null;
                }
            }

            return data;
        }

        public string ReadASCIIString(uint offset, int size)
        {
            var data = ReadBytes(offset, size);
            if (data != null)
            {
                var len = data.TakeWhile(d => d != 0).Count();
                return Encoding.ASCII.GetString(data, 0, len);
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
#if DEBUG
            Console.WriteLine("I2C context disposed by " + this.GetType());
#endif
            ctx?.Dispose();
        }
    }
}
