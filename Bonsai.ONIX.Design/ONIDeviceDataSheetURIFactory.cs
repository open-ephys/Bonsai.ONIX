using System;

namespace Bonsai.ONIX.Design
{
    internal static class ONIDeviceDataSheetURIFactory
    {
        public static Uri Make(DeviceID device)
        {
            var baseUri = "https://open-ephys.github.io/onix-docs/Hardware%20Guide/Datasheets/";

            switch (device)
            {
                //case DeviceID.NULL:
                //case DeviceID.INFO:
                //case DeviceID.RHD2132:
                case DeviceID.RHD2164:
                    return new Uri(baseUri + "rhd2164.html");
                case DeviceID.ElectricalStimulator:
                    return new Uri(baseUri + "estim-hs64.html");
                case DeviceID.OpticalStimulator:
                    return new Uri(baseUri + "ostim-hs64.html");
                //case DeviceID.TS4231:
                //case DeviceID.DINPUT32:
                //case DeviceID.DOUTPUT32:
                case DeviceID.BNO055:
                    return new Uri(baseUri + "bno055.html");
                //case DeviceID.Test:
                case DeviceID.NeuropixelsV1:
                    return new Uri(baseUri + "neuropixels-v1.html");
                case DeviceID.Heartbeat:
                    return new Uri(baseUri + "heartbeat.html");
                //case DeviceID.AD51X2:
                //case DeviceID.FMCVCTRL:
                //case DeviceID.AD7617:
                //case DeviceID.AD576X:
                //case DeviceID.TESTREG0:
                case DeviceID.BreakoutDigitalIO:
                    return new Uri(baseUri + "fmc-digital-io.html");
                //case DeviceIDx.FMCCLKIN1R3:
                case DeviceID.FMCClockOutput:
                    return new Uri(baseUri + "fmc-clock-out.html");
                //case DeviceID.TS4231V2ARR:
                //    return new TS4231V2Device();
                case DeviceID.BreakoutAnalogIO:
                    return new Uri(baseUri + "fmc-analog-io.html");
                case DeviceID.FMCLinkController:
                    return new Uri(baseUri + "fmc-link-control.html");
                case DeviceID.DS90UB9X:
                    return new Uri(baseUri + "ds90ub9x-raw.html");
                case DeviceID.TS4231V1Array:
                    return new Uri(baseUri + "ts4231-v1-array.html");
                //case DeviceID.MAX10ADCCORE:
                case DeviceID.LoadTest:
                    return new Uri(baseUri + "load-test.html");
                case DeviceID.MemoryUsage:
                    return new Uri(baseUri + "memory-usage.html");
                //case DeviceID.HARPSyncInput:
                case DeviceID.RHS2116:
                    return new Uri(baseUri + "rhs2116.html");
                case DeviceID.RHS2116Trigger:
                    return new Uri(baseUri + "rhs2116-trigger.html");
                default:
                    return null;
            }
        }
    }
}
