using System.Collections.Generic;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    public static class RHS2116Configuration
    {

        static readonly public Dictionary<AnalogLowCutoff, int[]> AnalogLowCutoffToRegisters;
        static readonly public Dictionary<AnalogHighCutoff, int[]> AnalogHighCutoffToRegisters;
        static readonly public Dictionary<AnalogHighCutoff, uint> AnalogHighCutoffToFastSettleSamples;
        static readonly public Dictionary<RHS2116StimulusSequence.StepSize, int[]> StimulatorStepSizeToRegisters;

        static RHS2116Configuration()
        {
            AnalogLowCutoffToRegisters = new Dictionary<AnalogLowCutoff, int[]>();
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low1000Hz, new[] { 10, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low500Hz, new[] { 13, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low300Hz, new[] { 15, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low250Hz, new[] { 17, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low200Hz, new[] { 18, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low150Hz, new[] { 21, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low100Hz, new[] { 25, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low75Hz, new[] { 28, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low50Hz, new[] { 34, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low30Hz, new[] { 44, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low25Hz, new[] { 48, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low20Hz, new[] { 54, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low15Hz, new[] { 62, 0, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low10Hz, new[] { 5, 1, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low7500mHz, new[] { 18, 1, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low5000mHz, new[] { 40, 1, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low3090mHz, new[] { 20, 2, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low2500mHz, new[] { 42, 2, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low2000mHz, new[] { 8, 3, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low1500mHz, new[] { 9, 4, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low1000mHz, new[] { 44, 6, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low750mHz, new[] { 49, 9, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low500mHz, new[] { 35, 17, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low300mHz, new[] { 1, 40, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low250mHz, new[] { 56, 54, 0 });
            AnalogLowCutoffToRegisters.Add(AnalogLowCutoff.Low100mHz, new[] { 16, 60, 1 });

            // NB: Page 11 of datasheet
            // Order: RH1_SEL1, RH1_SEL2, RH2_SEL1, RH2_SEL2
            AnalogHighCutoffToRegisters = new Dictionary<AnalogHighCutoff, int[]>();
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High20000Hz, new[] { 8, 0, 4, 0 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High15000Hz, new[] { 11, 0, 8, 0 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High10000Hz, new[] { 17, 0, 16, 0 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High7500Hz, new[] { 22, 0, 23, 0 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High5000Hz, new[] { 33, 0, 37, 0 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High3000Hz, new[] { 3, 1, 13, 1 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High2500Hz, new[] { 13, 1, 25, 1 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High2000Hz, new[] { 27, 1, 44, 1 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High1500Hz, new[] { 1, 2, 23, 2 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High1000Hz, new[] { 46, 2, 30, 3 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High750Hz, new[] { 41, 3, 36, 4 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High500Hz, new[] { 30, 5, 43, 6 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High300Hz, new[] { 6, 9, 2, 11 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High250Hz, new[] { 42, 10, 5, 13 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High200Hz, new[] { 24, 13, 7, 16 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High150Hz, new[] { 44, 17, 8, 21 });
            AnalogHighCutoffToRegisters.Add(AnalogHighCutoff.High100Hz, new[] { 38, 26, 5, 31 });

            // NB: Page 9 of datasheet shays this should be 2.5 seconds / AnalogHighCutoff in Hz
            // NB: max value allowed by gateware is 30 (1 msec)
            AnalogHighCutoffToFastSettleSamples = new Dictionary<AnalogHighCutoff, uint>();
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High20000Hz, 4);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High15000Hz, 5);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High10000Hz, 8);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High7500Hz, 10);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High5000Hz, 15);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High3000Hz, 25);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High2500Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High2000Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High1500Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High1000Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High750Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High500Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High300Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High250Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High200Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High150Hz, 30);
            AnalogHighCutoffToFastSettleSamples.Add(AnalogHighCutoff.High100Hz, 30);

            // NB: Page 12 of datasheet
            // Order: SEL1, SEL2, SEL3
            StimulatorStepSizeToRegisters = new Dictionary<RHS2116StimulusSequence.StepSize, int[]>();
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step10nA, new[] { 64, 19, 3 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step20nA, new[] { 40, 40, 1 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step50nA, new[] { 64, 40, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step100nA, new[] { 30, 20, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step200nA, new[] { 25, 10, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step500nA, new[] { 101, 3, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step1000nA, new[] { 98, 1, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step2000nA, new[] { 94, 0, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step5000nA, new[] { 38, 0, 0 });
            StimulatorStepSizeToRegisters.Add(RHS2116StimulusSequence.StepSize.Step10000nA, new[] { 15, 0, 0 });
        }

        public enum AnalogLowCutoff
        {
            Low1000Hz,
            Low500Hz,
            Low300Hz,
            Low250Hz,
            Low200Hz,
            Low150Hz,
            Low100Hz,
            Low75Hz,
            Low50Hz,
            Low30Hz,
            Low25Hz,
            Low20Hz,
            Low15Hz,
            Low10Hz,
            Low7500mHz,
            Low5000mHz,
            Low3090mHz,
            Low2500mHz,
            Low2000mHz,
            Low1500mHz,
            Low1000mHz,
            Low750mHz,
            Low500mHz,
            Low300mHz,
            Low250mHz,
            Low100mHz,
        }

        public enum AnalogHighCutoff
        {
            High20000Hz,
            High15000Hz,
            High10000Hz,
            High7500Hz,
            High5000Hz,
            High3000Hz,
            High2500Hz,
            High2000Hz,
            High1500Hz,
            High1000Hz,
            High750Hz,
            High500Hz,
            High300Hz,
            High250Hz,
            High200Hz,
            High150Hz,
            High100Hz,
        }

        // TODO: Description does not work
        // TODO: Recalculate -- this depends on the exact sampling frequency
        public enum DSPCutoff
        {
            Differential = 0,

            [Description("3310 Hz")]
            DSP3309Hz,
            [Description("1370 Hz")]
            DSP1374Hz,
            [Description("638 Hz")]
            DSP638Hz,
            [Description("308 Hz")]
            DSP308Hz,
            [Description("152 Hz")]
            DSP152Hz,
            [Description("75.2 Hz")]
            DSP75Hz,
            [Description("37.4 Hz")]
            DSP37Hz,
            [Description("18.7 Hz")]
            DSP19Hz,
            [Description("9.34 Hz")]
            DSP9336mHz,
            [Description("4.67 Hz")]
            DSP4665mHz,
            [Description("2.33 Hz")]
            DSP2332mHz,
            [Description("1.17 Hz")]
            DSP1166mHz,
            [Description("0.583 Hz")]
            DSP583mHz,
            [Description("0.291 Hz")]
            DSP291mHz,
            [Description("0.146 Hz")]
            DSP146mHz,

            Off
        }

        public enum DataFormat
        {
            Unsigned,
            TwosCompliment,
            Volts
        }
    }
}
