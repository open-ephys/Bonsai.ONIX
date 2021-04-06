using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("Sends 8-bit digital data to an Open-Ephys FMC breakout board Rev. 1.3.")]
    public class BreakoutDigitalOutputDevice : ONIFrameWriter<byte>
    {
        public BreakoutDigitalOutputDevice() : base(ONIXDevices.ID.BREAKDIG1R3) { }

        protected override void Write(ONIContextTask ctx, byte input)
        {
            ctx.Write((uint)DeviceIndex.SelectedIndex, (uint)input);
        }
    }
}
