using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class RHD2164DataFrame : U16DataBlockFrame
    {
        // Position in raw frame data (16-bit words) of corresponding Intan ephys channel
        // i.e. 0 -> 4
        //      1 -> 6
        //      ...
        //      63 -> 67
       private readonly static int[] EphysChannelMap = new[] {4 ,  6,  8, 10, 12, 14, 16, 18,
                                                              20, 22, 24, 26, 28, 30, 32, 34,
                                                              36, 38, 40, 42, 44, 46, 48, 50,
                                                              52, 54, 56, 58, 60, 62, 64, 66,
                                                              5 , 7 , 9 , 11, 13, 15, 17, 19,
                                                              21, 23, 25, 27, 29, 31, 33, 35,
                                                              37, 39, 41, 43, 45, 47, 49, 51,
                                                              53, 55, 57, 59, 61, 63, 65, 67};
        private readonly static int[] AuxChannelMap = new[] {68, 69, 70};

        public const int NumberOfEphysChannels = 64;
        public const int NumberOfAuxChannels = 3;
        public readonly int NumberOfSamples;
        public readonly RHD2164Configuration.EphysDataFormat EphysFormat;
        public readonly RHD2164Configuration.AuxDataFormat AuxFormat;

        public RHD2164DataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset,
                                RHD2164Configuration.EphysDataFormat ephysFormat,
                                RHD2164Configuration.AuxDataFormat auxFormat)
            : base(frameBlock, frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new WorkflowRuntimeException("RHD2164 frame buffer is empty.");
            }

            NumberOfSamples = frameBlock.Count;
            EphysFormat = ephysFormat;
            AuxFormat = auxFormat;

            if (EphysFormat == RHD2164Configuration.EphysDataFormat.Unsigned)
            {
                var ephysData = new ushort[NumberOfEphysChannels, NumberOfSamples];
                var auxiliaryData = new ushort[NumberOfAuxChannels, NumberOfSamples];

                for (int i = 0; i < NumberOfSamples; i++)
                {
                    
                    for (int j = 0; j < NumberOfEphysChannels; j++)
                    {
                        ephysData[j, i] = frameBlock[i].Sample[EphysChannelMap[j]];
                    }

                    auxiliaryData[0, i] = frameBlock[i].Sample[AuxChannelMap[0]];
                    auxiliaryData[1, i] = frameBlock[i].Sample[AuxChannelMap[1]];
                    auxiliaryData[2, i] = frameBlock[i].Sample[AuxChannelMap[2]];
                }

                EphysData = GetEphysDataU16(ephysData);
                AuxiliaryData = GetAuxiliaryData(auxiliaryData);

            }
            else
            {
                var ephysData = new short[NumberOfEphysChannels, NumberOfSamples];
                var auxiliaryData = new ushort[NumberOfAuxChannels, NumberOfSamples];

                for (int i = 0; i < NumberOfSamples; i++)
                {

                    for (int j = 0; j < NumberOfEphysChannels; j++)
                    {
                        ephysData[j, i] = (short)frameBlock[i].Sample[EphysChannelMap[j]];
                    }

                    auxiliaryData[0, i] = frameBlock[i].Sample[AuxChannelMap[0]];
                    auxiliaryData[1, i] = frameBlock[i].Sample[AuxChannelMap[1]];
                    auxiliaryData[2, i] = frameBlock[i].Sample[AuxChannelMap[2]];
                }

                EphysData = EphysFormat == RHD2164Configuration.EphysDataFormat.TwosCompliment ? GetEphysDataS16(ephysData) : GetEphysDataF32(ephysData);
                AuxiliaryData = GetAuxiliaryData(auxiliaryData);
            }
        }

        private Mat GetEphysDataU16(ushort[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        private Mat GetEphysDataS16(short[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        private Mat GetEphysDataF32(short[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.F32, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.ConvertScale(header, output, 0.195);
            }

            return output;
        }

        private Mat GetAuxiliaryData(ushort[,] data)
        {
            using (var header = Mat.CreateMatHeader(data))
            {
                if (AuxFormat == RHD2164Configuration.AuxDataFormat.Volts)
                {
                    var output = new Mat(NumberOfAuxChannels, NumberOfSamples, Depth.F32, 1);
                    CV.ConvertScale(header, output, 0.0000374);
                    return output;
                }
                else
                {
                    var output = new Mat(NumberOfAuxChannels, NumberOfSamples, Depth.U16, 1);
                    CV.Convert(header, output);
                    return output;
                }
            }
        }

        public Mat EphysData { get; private set; }

        public Mat AuxiliaryData { get; private set; }
    }
}
