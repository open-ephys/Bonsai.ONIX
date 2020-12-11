using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

// TODO: Sweep frequency needs to be in terms of DataClock cycles for this to work or we need to convert the pulse width etc to time in seconds
namespace Bonsai.ONIX
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Calculates uncalibrated 3D position of a single photodiode for V1 base stations.")]
    public class TS4321V1FrameToPosition
    {
        // Template pattern
        static readonly bool[] template = {
        //  bad    skip   axis   sweep
            false, false, false, false,
            false, true,  false, false,
            false, false, false, true, // axis 0, station 0
            false, false, true,  false,
            false, true,  true,  false,
            false, false, false, true, // axis 1, station 0
            false, true,  false, false,
            false, false, false, false,
            false, false, false, true, // axis 0, station 1
            false, true,  true,  false,
            false, false, true,  false,
            false, false, false, true  // axis 1, station 1
        };

        // Max seconds it should take to receive an entire valid template
        // TODO: Figure out the correct value...
        //const double max_packet_duration = 0.03;

        private Queue<double> pulse_times;
        private Queue<double> pulse_widths;
        private Queue<bool> pulse_parse;

        // Origins in Mat format
        Mat p, q;

        // Calculated position
        Mat position = new Mat(3, 1, Depth.F64, 1);
        double position_time;

        public TS4321V1FrameToPosition()
        {
            P = new Point3d(0, 0, 0);
            Q = new Point3d(1, 0, 0);

            var fill0 = new double[template.Count() / 4];
            var fill1 = new bool[template.Count()];
            pulse_times = new Queue<double>(fill0);
            pulse_widths = new Queue<double>(fill0);
            pulse_parse = new Queue<bool>(fill1);
        }

        bool Process(TS4231V1DataFrame source)
        {
            // Push pulse time into buffer and pop oldest
            pulse_times.Dequeue();
            pulse_times.Enqueue(source.DataClock / source.DataClockHz);

            // Push pulse width into buffer and pop oldest
            pulse_widths.Dequeue();
            pulse_widths.Enqueue(source.PulseWidth / source.DataClockHz);

            // Push pulse parse info into buffer and pop oldest 4x
            pulse_parse.Dequeue();
            pulse_parse.Dequeue();
            pulse_parse.Dequeue();
            pulse_parse.Dequeue();

            var type = source.PulseType;
            pulse_parse.Enqueue(type == -1); // bad
            pulse_parse.Enqueue(type >= 4 & type != 8); // skip
            pulse_parse.Enqueue(type % 2 == 1 & type != 8); // axis
            pulse_parse.Enqueue(type == 8); // sweep

            // Test template match and make sure time between pulses does 
            // not integrate to more than two periods
            if (!pulse_parse.SequenceEqual(template) || pulse_times.Last() - pulse_times.First() > 2 / SweepFrequency)
                return false;

            // Time is the mean of the data used
            position_time = 0.5 * (pulse_times.Last() + pulse_times.First());

            var time = pulse_times.ToArray();
            var width = pulse_widths.ToArray();

            var t11 = time[2] + width[2] / 2 - time[0];
            var t21 = time[5] + width[5] / 2 - time[3];
            var theta0 = 2 * Math.PI * SweepFrequency * t11 - Math.PI / 2;
            var gamma0 = 2 * Math.PI * SweepFrequency * t21 - Math.PI / 2;

            var u = new Mat(3, 1, Depth.F64, 1);
            u[0] = new Scalar(Math.Tan(theta0));
            u[1] = new Scalar(Math.Tan(gamma0));
            u[2] = new Scalar(1);
            CV.Normalize(u, u);

            var t12 = time[8] + width[8] / 2 - time[7];
            var t22 = time[11] + width[11] / 2 - time[10];
            var theta1 = 2 * Math.PI * SweepFrequency * t12 - Math.PI / 2;
            var gamma1 = 2 * Math.PI * SweepFrequency * t22 - Math.PI / 2;

            var v = new Mat(3, 1, Depth.F64, 1);
            v[0] = new Scalar(Math.Tan(theta1));
            v[1] = new Scalar(Math.Tan(gamma1));
            v[2] = new Scalar(1);
            CV.Normalize(v, v);

            // Base station origin vector
            var d = q - p;

            // Linear transform
            // A = [a11 a12]
            //     [a21 a22]
            var a11 = 1.0;
            var a12 = -CV.DotProduct(u, v);
            var a21 = CV.DotProduct(u, v);
            var a22 = -1.0;

            // Result
            // B = [b1]
            //     [b2]
            var b1 = CV.DotProduct(u, d);
            var b2 = CV.DotProduct(v, d);

            // Solve Ax = B
            var x2 = (b2 - b1 * a21 / a11) / (a22 - (a12 * a21) / a11);
            var x1 = (b1 - a12 * x2) / a11;

            // TODO: If non-singular solution else send NaNs
            //if (x)
            //{
            var p1 = p + x1 * u;
            var q1 = q + x2 * v;
            //}

            // Or single matrix with columns as results
            position = 0.5 * (p1 + q1);

            return true;
        }

        public IObservable<Position3D> Process(IObservable<TS4231V1DataFrame> source)
        {
            return source.Where(input => input.Index == Index)
                         .Where(input => Process(input))
                         .Select(input => new Position3D(position_time, position));
        }

        [Description("Index of the photodiode 3D position is calculated for.")]
        public int Index { get; set; }

        [Description("Position of the first base station in units of your choice.")]
        public Point3d P
        {
            get
            {
                return new Point3d(p[0].Val0, p[1].Val0, p[2].Val0);
            }
            set
            {
                p = new Mat(3, 1, Depth.F64, 1);
                p[0] = new Scalar(value.X);
                p[1] = new Scalar(value.Y);
                p[2] = new Scalar(value.Z);
            }
        }

        [Description("Position of the second base station in units of your choice.")]
        public Point3d Q
        {
            get
            {
                return new Point3d(q[0].Val0, q[1].Val0, q[2].Val0);
            }
            set
            {
                q = new Mat(3, 1, Depth.F64, 1);
                q[0] = new Scalar(value.X);
                q[1] = new Scalar(value.Y);
                q[2] = new Scalar(value.Z);
            }
        }

        [Description("Base station IR sweep frequency in Hz.")]
        public double SweepFrequency { get; set; } = 60;
    }
}
