using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class RawBNO055DataFrame
    {
        public RawBNO055DataFrame(ushort[] sample, ulong clock)
        {
            Euler = GetEuler(sample, 0);
            Quaternion = GetQuat(sample, 3);
            LinearAcceleration = GetAcceleration(sample, 7);
            GravityVector = GetAcceleration(sample, 10);
            Temperature = (byte)(sample[13] & 0x00FF); // 1°C = 1 LSB
            Calibration = (byte)((sample[13] & 0xFF00) >> 8); // Full calibration byte
            SystemCalibrated = (sample[13] & 0x0300) > 0; // 3 = calibrated, 0 = not calibrated
            AccelerometerCalibrated = (sample[13] & 0x0C00) > 0;
            GyroscopeCalibrated = (sample[13] & 0x3000) > 0;
            MagnitometerCalibrated = (sample[13] & 0xC000) > 0;
            Clock = clock;
        }

        public ulong Clock { get; private set; }

        public byte Calibration { get; private set; }

        public bool SystemCalibrated { get; private set; }

        public bool AccelerometerCalibrated { get; private set; }

        public bool GyroscopeCalibrated { get; private set; }

        public bool MagnitometerCalibrated { get; private set; }

        public byte Temperature { get; private set; }

        public Mat Quaternion { get; private set; }

        public Mat LinearAcceleration { get; private set; }

        public Mat GravityVector { get; private set; }

        public Mat Euler { get; private set; }

        private static Mat GetEuler(ushort[] sample, int begin)
        {
            // 1 degree = 16 LSB
            const double scale = 0.0625;
            var vec = new double[3];

            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] = scale * (short)sample[i + begin];
            }

            return Mat.FromArray(vec, vec.Length, 1, Depth.F64, 1);
        }

        private static Mat GetAcceleration(ushort[] sample, int begin)
        {
            // 1m/s^2 = 100 LSB
            const double scale = 0.01;
            var vec = new double[3];

            for (int i = 0; i < vec.Length; i++)
            {
                vec[i] = scale * (short)sample[i + begin];
            }

            return Mat.FromArray(vec, vec.Length, 1, Depth.F64, 1);
        }

        private static Mat GetQuat(ushort[] sample, int begin)
        {
            // 1 quaternion (unitless) = 2^14 LSB
            const double scale = (1.0 / (1 << 14));
            var vec = new double[4];

            for (int i = 0; i < vec.Length; i++)
            {
                var tmp = (short)sample[i + begin];
                vec[i] = scale * tmp;
            }

            return Mat.FromArray(vec, vec.Length, 1, Depth.F64, 1);
        }
    }
}
