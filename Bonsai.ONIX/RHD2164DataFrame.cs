using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class RHD2164DataFrame : U16DataBlockFrame
    {
        public const int NumberOfEphysChannels = 64;
        public const int NumberOfAuxChannels = 3;
        public readonly int NumberOfSamples;

        public RHD2164DataFrame(IList<ONIManagedFrame<ushort>> frameBlock)
            : base(frameBlock)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("RHD2164 frame buffer is empty.");
            }

            NumberOfSamples = frameBlock.Count;
            var ephysData = new ushort[NumberOfEphysChannels, NumberOfSamples];
            var auxiliaryData = new int[NumberOfAuxChannels, NumberOfSamples];

            for (int i = 0; i < NumberOfSamples; i++)
            {
                int chan = 4; // Data starts at index 4

                for (int j = 0; j < NumberOfEphysChannels; j++, chan++)
                {
                    ephysData[j, i] = frameBlock[i].Sample[chan];
                }

                for (int j = 0; j < NumberOfAuxChannels; j++, chan++)
                {
                    auxiliaryData[j, i] = frameBlock[i].Sample[chan];
                }
            }

            EphysData = GetEphysData(ephysData);
            AuxiliaryData = GetAuxiliaryData(auxiliaryData);
        }

        Mat GetEphysData(ushort[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        Mat GetAuxiliaryData(int[,] data)
        {
            var output = new Mat(NumberOfAuxChannels, NumberOfSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat EphysData { get; private set; }

        public Mat AuxiliaryData { get; private set; }
    }
}
