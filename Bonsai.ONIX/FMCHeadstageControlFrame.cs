﻿
namespace Bonsai.ONIX
{
    public class FMCHeadstageControlFrame : DataFrame
    {
        public FMCHeadstageControlFrame(oni.Frame frame)
            : base(frame)
        {
            Lock = (sample[4] & 0x0001) == 1;
            Pass = (sample[4] & 0x0002) == 2;
            Code = (sample[4] & 0x0004) == 4 ? (sample[4] & 0xFF00) >> 8 : 0;
        }

        public bool Lock { get; private set; }

        public bool Pass { get; private set; }

        public int Code { get; private set; }

    }
}
