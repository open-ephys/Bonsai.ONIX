
namespace Bonsai.ONIX
{
    public class FMCHeadstageControlFrame : U16DataFrame
    {
        public FMCHeadstageControlFrame(ONIManagedFrame<ushort> frame)
            : base(frame)
        {
            Lock = (frame.Sample[4] & 0x0001) == 1;
            Pass = (frame.Sample[4] & 0x0002) == 2;
            Code = (frame.Sample[4] & 0x0004) == 4 ? (frame.Sample[4] & 0xFF00) >> 8 : 0;
        }

        public bool Lock { get; private set; }

        public bool Pass { get; private set; }

        public int Code { get; private set; }

    }
}
