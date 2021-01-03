using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("Sends 8-bit digital data to an Open-Ephys FMC breakout board Rev. 1.3.")]
    public class BreakoutDigitalOutputDevice : ONIFrameWriterDeviceBuilder<byte>
    {
        public BreakoutDigitalOutputDevice() : base(ONIXDevices.ID.BREAKDIG1R3) { }

        public override void Write(ONIContextTask ctx, byte input)
        {
            ctx.Write((uint)DeviceIndex.SelectedIndex, (uint)input);
        }
    }
}
