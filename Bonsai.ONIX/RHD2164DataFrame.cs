using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class RHD2164DataFrame : U16DataBlockFrame
    {
        public const int NumberOfEphysChannels = 64;
        public const int NumberOfAuxChannels = 3;
        public readonly int NumberOfSamples;
        public readonly RHD2164Configuration.EphysDataFormat EphysFormat;
        public readonly RHD2164Configuration.AuxDataFormat AuxFormat;

        public RHD2164DataFrame(IList<ONIManagedFrame<ushort>> frameBlock,
                                RHD2164Configuration.EphysDataFormat ephysFormat,
                                RHD2164Configuration.AuxDataFormat auxFormat)
            : base(frameBlock)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("RHD2164 frame buffer is empty.");
            }

            NumberOfSamples = frameBlock.Count;
            EphysFormat = ephysFormat;
            AuxFormat = auxFormat;


            if (EphysFormat == RHD2164Configuration.EphysDataFormat.Unsigned)
            {
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

                EphysData = GetEphysDataU16(ephysData);
                AuxiliaryData = GetAuxiliaryData(auxiliaryData);

            }
            else
            {
                var ephysData = new short[NumberOfEphysChannels, NumberOfSamples];
                var auxiliaryData = new int[NumberOfAuxChannels, NumberOfSamples];

                for (int i = 0; i < NumberOfSamples; i++)
                {
                    int chan = 4; // Data starts at index 4

                    for (int j = 0; j < NumberOfEphysChannels; j++, chan++)
                    {
                        ephysData[j, i] = (short)frameBlock[i].Sample[chan];
                    }

                    for (int j = 0; j < NumberOfAuxChannels; j++, chan++)
                    {
                        auxiliaryData[j, i] = frameBlock[i].Sample[chan];
                    }
                }

                EphysData = EphysFormat == RHD2164Configuration.EphysDataFormat.TwosCompliment ? GetEphysDataS16(ephysData) : GetEphysDataF32(ephysData);
                AuxiliaryData = GetAuxiliaryData(auxiliaryData);
            }
        }


        Mat GetEphysDataU16(ushort[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        Mat GetEphysDataS16(short[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        Mat GetEphysDataF32(short[,] data)
        {
            var output = new Mat(NumberOfEphysChannels, NumberOfSamples, Depth.F32, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.ConvertScale(header, output, 0.195);
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

            if (AuxFormat == RHD2164Configuration.AuxDataFormat.Volts)
            {
                var scaled = new Mat(NumberOfAuxChannels, NumberOfSamples, Depth.F32, 1);
                CV.ConvertScale(output, scaled, 0.0000374);
                return scaled;
            }
            else
            {
                return output;
            }
        }

        public Mat EphysData { get; private set; }

        public Mat AuxiliaryData { get; private set; }
    }
}
