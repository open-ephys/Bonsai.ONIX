using OpenCV.Net;

namespace Bonsai.ONIX
{
    public class Position3D
    {
        public Position3D(ulong clock, ulong hubSyncCounter, Mat position)
        {
            Clock = clock;
            HubSyncCounter = hubSyncCounter;
            Matrix = position;
            Point = new Point3d(position[0].Val0, position[1].Val0, position[2].Val0);
        }

        public ulong Clock { get; private set; }

        public ulong HubSyncCounter { get; private set; }

        public Point3d Point { get; private set; }

        public Mat Matrix { get; private set; }

    }
}
