namespace Bonsai.ONIX.Design
{
    internal static class ONIDeviceFactory
    {
        public static ONIDevice Make(ONIXDevices.ID device)
        {
            switch (device)
            {
                //case ONIXDevices.ID.NULL:
                //case ONIXDevices.ID.INFO:
                //case ONIXDevices.ID.RHD2132:
                case ONIXDevices.ID.RHD2164:
                    return new RHD2164Device();
                case ONIXDevices.ID.ElectricalStimulator:
                    return new ElectricalStimulationDevice();
                case ONIXDevices.ID.OpticalStimulator:
                    return new OpticalStimulationDevice();
                //case ONIXDevices.ID.TS4231:
                //case ONIXDevices.ID.DINPUT32:
                //case ONIXDevices.ID.DOUTPUT32:
                case ONIXDevices.ID.BNO055:
                    return new BNO055Device();
                case ONIXDevices.ID.Test:
                    return new TestDevice();
                case ONIXDevices.ID.NeuropixelsV1:
                    return new NeuropixelsV1Device();
                case ONIXDevices.ID.Heartbeat:
                    return new HeartbeatDevice();
                //case ONIXDevices.ID.AD51X2:
                //case ONIXDevices.ID.FMCVCTRL:
                //case ONIXDevices.ID.AD7617:
                //case ONIXDevices.ID.AD576X:
                //case ONIXDevices.ID.TESTREG0:
                case ONIXDevices.ID.BreakoutDigitalIO:
                    return new DigitalIODevice();
                //case ONIXDevices.ID.FMCCLKIN1R3:
                case ONIXDevices.ID.FMCClockOutput:
                    return new ClockOutputDevice();
                //case ONIXDevices.ID.TS4231V2ARR:
                //    return new TS4231V2Device();
                case ONIXDevices.ID.BreakoutAnalogIO:
                    return new AnalogIODevice();
                case ONIXDevices.ID.FMCLinkController:
                    return new HeadstagePortControlDevice();
                case ONIXDevices.ID.DS90UB9X:
                    return new DS90UB9xDevice();
                case ONIXDevices.ID.TS4231V1Array:
                    return new TS4231V1Device();
                //case ONIXDevices.ID.MAX10ADCCORE:
                case ONIXDevices.ID.LoadTest:
                    return new LoadTestingDevice();
                case ONIXDevices.ID.MemoryUsage:
                    return new MemoryUsageDevice();
                case ONIXDevices.ID.HARPSyncInput:
                    return new HARPSyncInputDevice();
                default:
                    return null;
            }
        }
    }
}
