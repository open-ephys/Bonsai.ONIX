using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    public class AnalogInputDataFrame : U16DataBlockFrame
    {
        public const int NumberOfChannels = 12;
        internal readonly int NumberOfSamples;
        internal readonly AnalogIODevice.AnalogDataType Format;

        public AnalogInputDataFrame(IList<ONIManagedFrame<short>> frameBlock, ulong frameOffset,
                                    float[] scale,
                                    AnalogIODevice.AnalogDataType format = AnalogIODevice.AnalogDataType.S16)
            : base(frameBlock, frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("Analog input frame buffer is empty.");
            }

            NumberOfSamples = frameBlock.Count;
            Format = format;

            // TODO: Could make a diagonal matrix out of scale and do Volts
            // conversion using OpenCV but not sure if that would be beneficial.
            if (Format == AnalogIODevice.AnalogDataType.S16)
            {
                var data = new short[NumberOfChannels, NumberOfSamples];
                for (int i = 0; i < NumberOfSamples; i++)
                {
                    int chan = 4; // Data starts at index 4

                    for (int j = 0; j < NumberOfChannels; j++, chan++)
                    {
                        data[j, i] = frameBlock[i].Sample[chan];
                    }
                }

                Data = GetData(data);

            }
            else
            {
                var data = new float[NumberOfChannels, NumberOfSamples];
                for (int i = 0; i < NumberOfSamples; i++)
                {
                    int chan = 4; // Data starts at index 4

                    for (int j = 0; j < NumberOfChannels; j++, chan++)
                    {
                        data[j, i] = scale[j] * frameBlock[i].Sample[chan];
                    }
                }

                Data = GetData(data);
            }
        }

        private Mat GetData(short[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.S16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        private Mat GetData(float[,] data)
        {
            var output = new Mat(NumberOfChannels, NumberOfSamples, Depth.F32, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        public Mat Data { get; private set; }

    }
}
