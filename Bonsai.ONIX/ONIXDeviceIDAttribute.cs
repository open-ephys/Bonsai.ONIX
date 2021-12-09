namespace Bonsai.ONIX
{
    public enum DeviceID
    {
        Null = 0,
        Info = 1,
        RHD2132 = 2,
        RHD2164 = 3,
        ElectricalStimulator = 4,
        OpticalStimulator = 5,
        TS4231 = 6,
        DigitalInput32 = 7,
        DigitalOutput32 = 8,
        BNO055 = 9,
        Test = 10,
        NeuropixelsV1 = 11,
        Heartbeat = 12,
        AD51X2 = 13,
        FMCVoltageController = 14,
        AD7617 = 15,
        AD576X = 16,
        TestRegisterV0 = 17,
        BreakoutDigitalIO = 18,
        FMCClockInput = 19,
        FMCClockOutput = 20,
        TS4231V2Array = 21,
        BreakoutAnalogIO = 22,
        FMCLinkController = 23,
        DS90UB9X = 24,
        TS4231V1Array = 25,
        MAX10ADCCore = 26,
        LoadTest = 27,
        MemoryUsage = 28,
        HARPSyncInput = 30
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class ONIXDeviceIDAttribute : System.Attribute
    {
        public DeviceID deviceID;

        public ONIXDeviceIDAttribute(DeviceID id)
        {
            deviceID = id;
        }
    }
}
