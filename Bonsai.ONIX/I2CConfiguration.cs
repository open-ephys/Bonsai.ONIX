using System.Linq;
using System.Text;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Device configuration using the bus_to_i2c_raw.vhd core. Converts the ONI configuration programming interface into I2C.
    /// </summary>
    public class I2CConfiguration
    {

        ONIController ctrl;
        uint? dev_idx;

        public readonly uint I2C_ADDR;

        public I2CConfiguration(ONIController controller, uint? device_index, uint i2c_addr)
        {
            ctrl = controller;
            dev_idx = device_index;
            I2C_ADDR = i2c_addr;
        }

        public byte? ReadByte(uint addr)
        {
            uint reg_addr = (addr << 8) | I2C_ADDR;
            return (byte?)ctrl?.ReadRegister(dev_idx, reg_addr);
        }

        public void WriteByte(uint addr, uint value)
        {
            uint reg_addr = (addr << 8) | I2C_ADDR;
            ctrl?.WriteRegister(dev_idx, reg_addr, value);
        }

        public byte[] ReadBytes(uint offset, int size)
        {
            var data = new byte[size];

            for (uint i = 0; i < size; i++)
            {
                uint reg_addr = ((offset + i) << 8) | I2C_ADDR;
                var val = ctrl?.ReadRegister(dev_idx, reg_addr);

                if (val == null)
                    return null;

                data[i] = (byte)val;

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
