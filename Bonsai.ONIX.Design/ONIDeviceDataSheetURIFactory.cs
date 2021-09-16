using System;

namespace Bonsai.ONIX.Design
{
    internal static class ONIDeviceDataSheetURIFactory
    {

        public static Uri Make(ONIXDevices.ID device)
        {
            var baseUri = "https://open-ephys.github.io/onix-docs/Hardware%20Guide/Datasheets/";

            switch (device)
            {
                //case ONIXDevices.ID.NULL:
                //case ONIXDevices.ID.INFO:
                //case ONIXDevices.ID.RHD2132:
                case ONIXDevices.ID.RHD2164:
                    return new Uri(baseUri + "rhd2164.html");
                case ONIXDevices.ID.ElectricalStimulator:
                    return new Uri(baseUri + "estim-hs64.html");
                case ONIXDevices.ID.OpticalStimulator:
                    return new Uri(baseUri + "ostim-hs64.html");
                //case ONIXDevices.ID.TS4231:
                //case ONIXDevices.ID.DINPUT32:
                //case ONIXDevices.ID.DOUTPUT32:
                case ONIXDevices.ID.BNO055:
                    return new Uri(baseUri + "bno055.html");
                //case ONIXDevices.ID.Test:
                case ONIXDevices.ID.NeuropixelsV1:
                    return new Uri(baseUri + "neuropixels-v1.html");
                case ONIXDevices.ID.Heartbeat:
                    return new Uri(baseUri + "heartbeat.html");
                //case ONIXDevices.ID.AD51X2:
                //case ONIXDevices.ID.FMCVCTRL:
                //case ONIXDevices.ID.AD7617:
                //case ONIXDevices.ID.AD576X:
                //case ONIXDevices.ID.TESTREG0:
                case ONIXDevices.ID.BreakoutDigitalIO:
                    return new Uri(baseUri + "fmc-digital-io.html");
                //case ONIXDevices.ID.FMCCLKIN1R3:
                case ONIXDevices.ID.FMCClockOutput:
                    return new Uri(baseUri + "fmc-clock-out.html");
                //case ONIXDevices.ID.TS4231V2ARR:
                //    return new TS4231V2Device();
                case ONIXDevices.ID.BreakoutAnalogIO:
                    return new Uri(baseUri + "fmc-analog-io.html");
                case ONIXDevices.ID.FMCLinkController:
                    return new Uri(baseUri + "fmc-link-control.html");
                case ONIXDevices.ID.DS90UB9X:
                    return new Uri(baseUri + "ds90ub9x-raw.html");
                case ONIXDevices.ID.TS4231V1Array:
                    return new Uri(baseUri + "ts4231-v1-array.html");
                //case ONIXDevices.ID.MAX10ADCCORE:
                case ONIXDevices.ID.LoadTest:
                    return new Uri(baseUri + "load-test.html");
                case ONIXDevices.ID.MemoryUsage:
                    return new Uri(baseUri + "memory-usage.html");
                default:
                    return null;
            }
        }
    }
}
