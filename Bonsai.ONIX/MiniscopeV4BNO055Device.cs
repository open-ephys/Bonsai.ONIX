using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    // TODO: If you double click on this node to show a visualizer during operation,
    // the I2C interface stops and it hangs. No clue why.
    [Combinator(MethodName = "Generate")]
    [WorkflowElementCategory(ElementCategory.Source)]
    [ONIXDeviceID(DeviceID.DS90UB9X)]
    [Description("Acquire image data from UCLA Miniscope V4's integrated BNO055 inertial measurement unit.")]
    public class MiniscopeV4BNO055Device : ONIDevice
    {
        private const int BNO055Address = 0x28;

        private ONIDeviceAddress deviceAddress = new ONIDeviceAddress();
        public override ONIDeviceAddress DeviceAddress
        {
            get { return deviceAddress; }
            set
            {
                deviceAddress = value;

                // Configuration I2C aliases
                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, DS90UB9xConfiguration.DeserializerDefaultAddress))
                {
                    uint val = BNO055Address << 1;
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveID4, val);
                    i2c.WriteByte((uint)DS90UB9xConfiguration.DesI2CRegister.SlaveAlias4, val);
                }

                // Setup BNO055
                using (var i2c = new I2CRegisterConfiguration(DeviceAddress, ID, BNO055Address))
                {
                    i2c.WriteByte(0x3E, 0x00); // Power mode normal
                    i2c.WriteByte(0x07, 0x00);// Page ID address 0
                    i2c.WriteByte(0x3F, 0x00); // Interal oscillator
                    i2c.WriteByte(0x41, 0b00000110); // Axis map config (configured to match hs64; X => Z, Y => -Y, Z => X)
                    i2c.WriteByte(0x42, 0b000000010); // Axis sign (negate Y)
                    i2c.WriteByte(0x3D, 8); // Operation mode is NOF
                }
            }
        }

        public IObservable<MiniscopeV4BNO055DataFrame> Generate()
        {
            // Max of 100 Hz, but limited by I2C bus
            var source = Observable.Interval(TimeSpan.FromSeconds(0.01));

            return Observable.Using(
                () => new I2CRegisterConfiguration(DeviceAddress, ID, BNO055Address),
                i2c => source.Select(_ =>
                {
                    var data = new byte[28];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = i2c.ReadByte((uint)(0x1A + i)) ?? 0;
                    }

                    var words = new ushort[data.Length / 2];
                    Buffer.BlockCopy(data, 0, words, 0, data.Length);

                    return new MiniscopeV4BNO055DataFrame(words);
                })
            );
        }
    }
}
