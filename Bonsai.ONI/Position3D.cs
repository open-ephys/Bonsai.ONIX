using System.Linq;
using OpenCV.Net;

namespace Bonsai.ONI
{
    public class Position3D
    {
        public Position3D(double time, Mat position)
        {
            Time = time;
            Matrix = position;
            Point = new Point3d(position[0].Val0, position[1].Val0, position[2].Val0);
        }

        public double Time { get; private set; }

        public Point3d Point { get; private set; }

        public Mat Matrix { get; private set; }

    }
}
