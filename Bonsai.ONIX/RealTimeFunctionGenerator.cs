using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    class DepthConverter : NullableConverter
    {
        public DepthConverter()
            : base(typeof(Depth?))
        {
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var values = base.GetStandardValues(context);
            return new StandardValuesCollection(values.Cast<object>().Where(value => (Depth?)value != Depth.UserType).ToArray());
        }
    }

    [Description("Produces waveforms from various periodic functions of wall-clock time as fast as possible. Due to the nature of computer operating systems, these waveforms will be jumpy. Without a data rate limiting downstream operation, this node will use 100% of your CPU.")]
    public class RealTimeFunctionGenerator : Source<Mat>
    {
        const double TwoPI = 2 * Math.PI;
        public enum FunctionWaveform
        {
            Sine,
            Square,
            Triangular,
            Sawtooth
        }

        [Description("The number of samples in each output buffer.")]
        public int BufferLength { get; set; } = 100;

        [Range(1, int.MaxValue)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The frequency of the signal waveform, in Hz.")]
        public double Frequency { get; set; } = 100;

        [Description("The periodic waveform used to sample the signal.")]
        public FunctionWaveform Waveform { get; set; }

        [TypeConverter(typeof(DepthConverter))]
        [Description("The optional target bit depth of individual buffer elements.")]
        public Depth? Depth { get; set; }

        [Browsable(false)]
        public bool DepthSpecified
        {
            get { return Depth.HasValue; }
        }

        [Description("The amplitude of the signal waveform.")]
        public double Amplitude { get; set; } = 1;

        [Description("The optional DC-offset of the signal waveform.")]
        public double Offset { get; set; }

        [Range(-Math.PI, Math.PI)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The optional phase offset, in radians, of the signal waveform.")]
        public double Phase { get; set; }

        static double NormalizedPhase(double phase)
        {
            return phase + Math.Ceiling(-phase / TwoPI) * TwoPI;
        }
        static void FrequencyPhaseShift(
            Stopwatch stopWatch,
            double newFrequency,
            ref double frequency,
            ref double phase)
        {
            newFrequency = Math.Max(0, newFrequency);
            if (frequency != newFrequency)
            {
                phase = NormalizedPhase(stopWatch.Elapsed.TotalSeconds * TwoPI * (frequency - newFrequency) + phase);
                frequency = newFrequency;
            }
        }

        Mat CreateBuffer(int bufferLength, Stopwatch stopWatch, double frequency, double phase)
        {
            var buffer = new double[bufferLength];
            if (frequency > 0)
            {
                var period = 1.0 / frequency;
                var waveform = Waveform;
                switch (waveform)
                {
                    default:
                    case FunctionWaveform.Sine:
                        frequency = frequency * TwoPI;
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = Math.Sin(frequency * (i + stopWatch.Elapsed.TotalSeconds) + phase);
                        }
                        break;
                    case FunctionWaveform.Triangular:
                        phase = NormalizedPhase(phase) / TwoPI;
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            var t = frequency * (i + stopWatch.Elapsed.TotalSeconds + period / 4) + phase;
                            buffer[i] = (1 - (4 * Math.Abs((t % 1) - 0.5) - 1)) - 1;
                        }
                        break;
                    case FunctionWaveform.Square:
                    case FunctionWaveform.Sawtooth:
                        phase = NormalizedPhase(phase) / TwoPI;
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            var t = frequency * (i + stopWatch.Elapsed.TotalSeconds + period / 2) + phase;
                            buffer[i] = 2 * (t % 1) - 1;
                            if (waveform == FunctionWaveform.Square)
                            {
                                buffer[i] = Math.Sign(buffer[i]);
                            }
                        }
                        break;
                }
            }

            var result = new Mat(1, buffer.Length, Depth.GetValueOrDefault(OpenCV.Net.Depth.F64), 1);
            using (var bufferHeader = Mat.CreateMatHeader(buffer))
            {
                CV.ConvertScale(bufferHeader, result, Amplitude, Offset);
                return result;
            }
        }

        public override IObservable<Mat> Generate()
        {
            return Observable.Create<Mat>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var bufferLength = BufferLength;
                    if (bufferLength <= 0)
                    {
                        throw new InvalidOperationException("Buffer length must be a positive integer.");
                    }

                    using (var sampleSignal = new ManualResetEvent(false))
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        var frequency = 0.0;
                        var phaseShift = 0.0;

                        while (!cancellationToken.IsCancellationRequested)
                        {
                            FrequencyPhaseShift(stopwatch, Frequency, ref frequency, ref phaseShift);
                            var buffer = CreateBuffer(bufferLength, stopwatch, frequency, Phase + phaseShift);
                            observer.OnNext(buffer);
                        }
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            });
        }
    }
}