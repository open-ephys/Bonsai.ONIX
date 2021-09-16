using OpenCV.Net;
using System;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class UCLAMiniscopeV3DataFrame : U16DataSplitFrame
    {
        public const int NumRows = 480;
        public const int NumCols = 752;

        public UCLAMiniscopeV3DataFrame(IList<ONIManagedFrame<ushort>> frameBlock)
            : base(frameBlock)
        {

            var data = new ushort[NumRows * NumCols];

            for (int i = 0; i < NumRows; i++)
            {
                Array.Copy(frameBlock[i].Sample, 4, data, NumCols * i, NumCols);
            }

            var mat = Mat.FromArray(data, 480, 752, Depth.U16, 1).GetImage();
            CV.ScaleAdd(mat, new Scalar(64), Mat.Zeros(NumRows, NumCols, Depth.U16, 1), mat);
            Image = mat;
        }

        public IplImage Image { get; private set; }
    }
}
