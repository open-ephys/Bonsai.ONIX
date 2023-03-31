using System.Linq;
using System.Text;

namespace Bonsai.ONIX
{
    public class I2CRegisterConfiguration : RegisterConfiguration
    {

        public readonly uint I2CAddress;

        /// <summary>
        /// Device configuration using the bus_to_i2c_raw.vhd core, which converts the ONI register
        /// programming interface into I2C.
        /// </summary>
        /// <param name="address">Device Address</param>
        /// <param name="id">Device ID</param>
        /// <param name="i2cAddress">7-bit I2C address</param>
        public I2CRegisterConfiguration(ONIDeviceAddress address, DeviceID id, uint i2cAddress) 
            : base(address, id)
        {
            I2CAddress = i2cAddress;
        }

        public byte? ReadByte(uint address)
        {
            uint reg_addr = (address << 7) | (I2CAddress & 0x7F);
            var val = ReadRegister(reg_addr);

            return val != null && val <= byte.MaxValue ? (byte?)val : null;
        }

        public void WriteByte(uint address, uint value)
        {
            uint reg_addr = (address << 7) | (I2CAddress & 0x7F);
            WriteRegister(reg_addr, value);
        }

        public byte[] ReadBytes(uint offset, int size)
        {
            var data = new byte[size];

            for (uint i = 0; i < size; i++)
            {
                uint reg_addr = ((offset + i) << 7) | (I2CAddress & 0x7F);
                var val = ReadRegister(reg_addr);

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

    }
}
