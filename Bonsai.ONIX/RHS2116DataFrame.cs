using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class RHS2116DataFrame : U16DataBlockFrame
    {
        public const int NumberOfChannels = 16;
        public readonly int NumberOfSamples;
        public readonly RHS2116Configuration.DataFormat EphysFormat;

        public RHS2116DataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset,
                                RHS2116Configuration.DataFormat ephysFormat)
            : base(frameBlock, frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("RHD2164 frame buffer is empty.");
            }

            NumberOfSamples = frameBlock.Count;
            EphysFormat = ephysFormat;

            // Ephys data
            if (EphysFormat == RHS2116Configuration.DataFormat.Unsigned)
            {
                var ephysData = new ushort[NumberOfChannels, NumberOfSamples];

            for (int i = 0; i < NumberOfSamples; i++)
            {

                int chan = 4; // Ephys data starts at index 4

                for (int j = 0; j < NumberOfChannels; j++, chan++)
                {
                    ephysData[j, i] = frameBlock[i].Sample[chan];
                }
            }

                EphysData = GetDataU16(ephysData);

            }
            else
            {
                var ephysData = new short[NumberOfChannels, NumberOfSamples];

                for (int i = 0; i < NumberOfSamples; i++)
                {

                    int chan = 4; // Ephys data starts at index 4

                    for (int j = 0; j < NumberOfChannels; j++, chan++)
                    {
                        ephysData[j, i] = (short)frameBlock[i].Sample[chan];
                    }
                }

                EphysData = EphysFormat == RHS2116Configuration.DataFormat.TwosCompliment ? GetDataS16(ephysData) : GetEphysDataF32(ephysData);
            }

            // DC Data
            if (EphysFormat == RHS2116Configuration.DataFormat.Unsigned)
            {
                var dcData = new ushort[NumberOfChannels, NumberOfSamples];

                for (int i = 0; i < NumberOfSamples; i++)
                {
                    int chan = 20; // DC data starts at index 20

                    for (int j = 0; j < NumberOfChannels; j++, chan++)
                    {
                        // Amplitude inverted around 512
                        dcData[j, i] = (ushort)(1024 - frameBlock[i].Sample[chan]);
                    }
                }

                DCData = GetDataU16(dcData);

            }
            // TODO: For some reason, when I set the AC data to 2s compelment, raw DC data is different
            // it looks like its sitting at high voltage for some reason, no clue why.
            else
            {
                var dcData = new short[NumberOfChannels, NumberOfSamples];

                for (int i = 0; i < NumberOfSamples; i++)
                {

                    int chan = 20; // DC data starts at index 20

                    for (int j = 0; j < NumberOfChannels; j++, chan++)
                    {
                        dcData[j, i] = (short)(512 - frameBlock[i].Sample[chan]); // NB: DC channels are always unsigned
                    }
                }

                DCData = EphysFormat == RHS2116Configuration.DataFormat.TwosCompliment ? GetDataS16(dcData) : GetDCDataF32(dcData);
            }
        }

        private Mat GetDataU16(ushort[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        private Mat GetDataS16(short[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        private Mat GetEphysDataF32(short[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.F32, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.ConvertScale(header, output, 0.195); // NB: 0.195 uV/LSB
            }

            return output;
        }

        // NB: DC amplifier range: -5.8 –+6.4. Values beyond this can be clipped
        private Mat GetDCDataF32(short[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.F32, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.ConvertScale(header, output, 0.01923); // NB: 19.23 mV/LSB
            }

            return output;
        }

        public Mat EphysData { get; private set; }

        public Mat DCData { get; private set; }
    }
}
