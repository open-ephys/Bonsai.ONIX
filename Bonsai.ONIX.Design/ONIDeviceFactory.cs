namespace Bonsai.ONIX.Design
{
    internal static class ONIDeviceFactory
    {
        public static ONIDevice Make(DeviceID device)
        {
            switch (device)
            {
                //case ID.NULL:
                //case ID.INFO:
                //case ID.RHD2132:
                case DeviceID.RHD2164:
                    return new RHD2164Device();
                case DeviceID.ElectricalStimulator:
                    return new ElectricalStimulationDevice();
                case DeviceID.OpticalStimulator:
                    return new OpticalStimulationDevice();
                //case ID.TS4231:
                //case ID.DINPUT32:
                //case ID.DOUTPUT32:
                case DeviceID.BNO055:
                    return new BNO055Device();
                case DeviceID.Test:
                    return new TestDevice();
                case DeviceID.NeuropixelsV1:
                    return new NeuropixelsV1Device();
                case DeviceID.Heartbeat:
                    return new HeartbeatDevice();
                //case ID.AD51X2:
                //case ID.FMCVCTRL:
                //case ID.AD7617:
                //case ID.AD576X:
                //case ID.TESTREG0:
                case DeviceID.BreakoutDigitalIO:
                    return new DigitalIODevice();
                //case ID.FMCCLKIN1R3:
                case DeviceID.FMCClockOutput:
                    return new ClockOutputDevice();
                //case ID.TS4231V2ARR:
                //    return new TS4231V2Device();
                case DeviceID.BreakoutAnalogIO:
                    return new AnalogIODevice();
                case DeviceID.FMCLinkController:
                    return new HeadstagePortControlDevice();
                case DeviceID.DS90UB9X:
                    return new DS90UB9xDevice();
                case DeviceID.TS4231V1Array:
                    return new TS4231V1Device();
                //case ID.MAX10ADCCORE:
                case DeviceID.LoadTest:
                    return new LoadTestingDevice();
                case DeviceID.MemoryUsage:
                    return new MemoryUsageDevice();
                case DeviceID.HARPSyncInput:
                    return new HARPSyncInputDevice();
                //case DeviceID.RHS2116:
                //    return new RHS2116Device();
                default:
                    return null;
            }
        }
    }
}
