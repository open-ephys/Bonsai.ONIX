
using System;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class HeadstagePortControlFrame : U16DataFrame
    {
        public HeadstagePortControlFrame(ONIManagedFrame<ushort> frame)
            : base(frame)
        {
            Lock = (frame.Sample[4] & 0x0001) == 1;
            Pass = (frame.Sample[4] & 0x0002) == 2;
            Code = (frame.Sample[4] & 0x0004) == 4 ? (frame.Sample[4] & 0xFF00) >> 8 : 0;
            Message = CodeToString(Code);
        }

        private string CodeToString(int code)
        {
            var messages = new List<string>();

            if ((Code & 0x0001) > 0)
            {
                messages.Add("CRC error");
            }

            if ((Code & 0x0002) > 0) 
            {
                messages.Add("Too many devices in device table");
            }

            if ((Code & 0x0004) > 0)
            {
                messages.Add("Remote initialization error");
            }
            
            if ((Code & 0x0008) > 0)
            {
                messages.Add("Bad remote packet formatting");
            }

            return string.Join(", ", messages);
        }

        public bool Lock { get; private set; }

        public bool Pass { get; private set; }

        public int Code { get; private set; }

        public string Message { get; private set; }

    }
}
