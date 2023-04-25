namespace Bonsai.ONIX
{
    public static class DS90UB9xConfiguration
    {
        public enum Register
        {
            // Managed register map
            Enable = 0x00008000,
            ReadSize = 0x00008001,
            Trigger = 0x00008002,
            TriggerOffset = 0x00008003,
            PixelGate = 0x00008004,
            IncludeSyncBits = 0x00008005,
            MarkMode = 0x00008006,
            GPIODirection = 0x00008007,
            GPIOValue = 0x00008008,
            LinkStatus = 0x00008009,
        }

        public enum TriggerMode
        {
            Continuous = 0,
            HsyncEdgePositive = 0b0001,
            HsyncEgeNegative = 0b1001,
            HsyncLevelPositive = 0b0101,
            HsyncLevelNegative = 0b1101,
            VsyncEdgePositive = 0b0011,
            VsyncEdgeNegative = 0b1011,
            VsyncLevelPositive = 0b0111,
            VsyncLevelNegative = 0b1111,
        }

        public enum PixelGate
        {
            Disabled = 0,
            HsyncPositive = 0b001,
            HsyncNegative = 0b101,
            VsyncPositive = 0b011,
            VsyncNegative = 0b111,
        }

        public enum MarkMode
        {
            Disabled = 0,
            HsyncRising = 0b001,
            HsyncFalling = 0b101,
            VsyncRising = 0b011,
            VsyncFalling = 0b111,
        }

        public enum SpecialBits
        {
            BitMark = 15,
            BitVsync = 14,
            BitHsync = 13,
        }

        public const uint DeserializerDefaultAddress = 0x30;
        public const uint SerializerDefaultAddress = 0x58;

        public enum DesI2CRegister
        {
            PortMode = 0x6D,

            SlaveID1 = 0x5E,
            SlaveID2 = 0x5F,
            SlaveID3 = 0x60,
            SlaveID4 = 0x61,
            SlaveID5 = 0x62,
            SlaveID6 = 0x63,
            SlaveID7 = 0x64,

            SlaveAlias1 = 0x66,
            SlaveAlias2 = 0x67,
            SlaveAlias3 = 0x68,
            SlaveAlias4 = 0x69,
            SlaveAlias5 = 0x6A,
            SlaveAlias6 = 0x6B,
            SlaveAlias7 = 0x6C,
        }

        public enum SerI2CRegister
        {
            GPIO10 = 0x0D,
            GPIO32 = 0x0E,
        }

        public enum Mode
        {
            Raw12BitLowFrequency = 1,
            Raw12BitHighFrequency = 2,
            Raw10Bit = 3,
        }

        public enum Direction
        {
            Input = 0,
            Output = 1
        }
    }
}
