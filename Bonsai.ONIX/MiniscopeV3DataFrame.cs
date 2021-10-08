using OpenCV.Net;
using System;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class MiniscopeV3DataFrame : U16DataSplitFrame
    {
        public const int NumRows = 480;
        public const int NumCols = 752;

        public MiniscopeV3DataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset)
            : base(frameBlock, frameOffset)
        {

            var data = new ushort[NumRows * NumCols];

            for (int i = 0; i < NumRows; i++)
            {
                Array.Copy(frameBlock[i].Sample, 4, data, NumCols * i, NumCols);
            }

            var image = Mat.FromArray(data, NumRows, NumCols, Depth.U16, 1).GetImage();
            CV.ScaleAdd(image, new Scalar(64), Mat.Zeros(NumRows, NumCols, Depth.U16, 1), image); // Move 10 LSBs to positions 15 downto 6

            Image = image;
        }

        public IplImage Image { get; private set; }
    }
}
