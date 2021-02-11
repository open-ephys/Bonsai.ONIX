namespace Bonsai.ONIX.Design
{
    internal static class ONIDeviceExpressionBuilderFactory
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
                case ONIXDevices.ID.ESTIM:
                    return new ElectricalStimulationDevice();
                case ONIXDevices.ID.OSTIM:
                    return new OpticalStimulationDevice();
                //case ONIXDevices.ID.TS4231:
                //case ONIXDevices.ID.DINPUT32:
                //case ONIXDevices.ID.DOUTPUT32:
                case ONIXDevices.ID.BNO055:
                    return new BNO055Device();
                case ONIXDevices.ID.TEST:
                    return new TestDevice();
                case ONIXDevices.ID.NEUROPIX1R0:
                    return new NeuropixelsV1Device();
                case ONIXDevices.ID.HEARTBEAT:
                //case ONIXDevices.ID.AD51X2:
                //case ONIXDevices.ID.FMCVCTRL:
                //case ONIXDevices.ID.AD7617:
                //case ONIXDevices.ID.AD576X:
                //case ONIXDevices.ID.TESTREG0:
                case ONIXDevices.ID.BREAKDIG1R3:
                    return new BreakoutDigitalInputDevice();
                //case ONIXDevices.ID.FMCCLKIN1R3:
                case ONIXDevices.ID.FMCCLKOUT1R3:
                    return new FMCCLKOutDevice();
                case ONIXDevices.ID.TS4231V2ARR:
                    return new TS4231V2Device();
                case ONIXDevices.ID.FMCANALOG1R3:
                    return new FMCAnalogIODevice();
                case ONIXDevices.ID.FMCLINKCTRL:
                    return new FMCHeadstageControlDevice();
                //case ONIXDevices.ID.DS90UB9X:
                case ONIXDevices.ID.TS4231V1ARR:
                    return new TS4231V1Device();
                //case ONIXDevices.ID.MAX10ADCCORE:
                case ONIXDevices.ID.LOADTEST:
                    return new LoadTestingDevice();
                case ONIXDevices.ID.MEMUSAGE:
                    return new MemoryUsageDevice();
                default:
                    return null;
            }
        }
    }
}
