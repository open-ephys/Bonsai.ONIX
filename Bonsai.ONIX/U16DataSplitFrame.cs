using System.Collections.Generic;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Base class for buffered groups of data frames that are used to create a single sample and
    /// use <see cref="ushort"/> as the underlying data type.
    /// </summary>
    public class U16DataSplitFrame
    {
        public U16DataSplitFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong offset)
        {
            Clock = frameBlock[0].FrameClock - offset;
            HubSyncCounter = ((ulong)frameBlock[0].Sample[0] << 48) |
                        ((ulong)frameBlock[0].Sample[1] << 32) |
                        ((ulong)frameBlock[0].Sample[2] << 16) |
                        ((ulong)frameBlock[0].Sample[3] << 0);
        }

        /// <summary>
        /// The frame clock. Created by the host when receiving the sample from the device.
        /// </summary>
        public ulong Clock { get; private set; }

        /// <summary>
        /// The sample clock, create locally alongside the source device.
        /// </summary>
        public ulong HubSyncCounter { get; private set; }
    }
}
