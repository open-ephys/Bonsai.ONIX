using OpenCV.Net;
using System;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class MiniscopeDataFrame : U16DataSplitFrame
    {
        public MiniscopeDataFrame(IList<ONIManagedFrame<ushort>> frameBlock, int rows, int columns)
            : base(frameBlock)
        {

            var data = new ushort[rows * columns];

            for (int i = 0; i < rows; i++)
            {
                Array.Copy(frameBlock[i].Sample, 4, data, columns * i, columns);
            }

            var source = Mat.FromArray(data, rows, columns, Depth.U16, 1);
            CV.ConvertScale(source.GetRow(0), source.GetRow(0), 1.0f, -32768.0f); // Get rid of mark bit
            Image = new Mat(source.Size, Depth.U8, 1).GetImage();
            CV.ConvertScale(source, Image, 0.25); // Move 8 MSBs to positions 7 downto 0
        }

        public IplImage Image { get; private set; }
    }
}
