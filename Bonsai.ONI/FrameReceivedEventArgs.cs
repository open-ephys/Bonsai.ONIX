using System;

namespace Bonsai.ONI
{
    public class FrameReceivedEventArgs : EventArgs
    {
        public FrameReceivedEventArgs(oni.Frame frame)
        {
            Value = frame;
        }

        public oni.Frame Value { get; private set; }
    }
}
