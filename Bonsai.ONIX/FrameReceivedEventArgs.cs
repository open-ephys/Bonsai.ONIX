using System;
namespace Bonsai.ONIX
{
    public class FrameReceivedEventArgs : EventArgs
    {
        public FrameReceivedEventArgs(oni.Frame frame)
        {
            Frame = frame;
        }

        public oni.Frame Frame { get; private set; }
    }
}