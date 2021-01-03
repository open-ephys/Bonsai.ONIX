using OpenCV.Net;
using System;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("Sends data to the twelve 16-bit analog outputs on the Open Ephys FMC Host. Used in concert with FMCAnalogInputDevice.")]
    public class FMCAnalogOutputDevice : ONIFrameWriterDeviceBuilder<Arr>
    {
        const int Rows = 12;
        const int Cols = 1;

        public FMCAnalogOutputDevice() : base(ONIXDevices.ID.FMCANALOG1R3) { }

        public override void Write(ONIContextTask ctx, Arr input)
        {
            var m = input.GetMat();

            // Check dims
            if (m.Rows * m.Cols != Rows * Cols)
            {
                throw new IndexOutOfRangeException("Source must be a 12 element vector.");
            }

            if (m.Depth != Depth.U16)
            {
                throw new InvalidOperationException("Source elements must be unsigned 16 bit integers");
            }

            ctx.Write((uint)DeviceIndex.SelectedIndex, m.Data, 2 * Rows);
        }
    }
}
