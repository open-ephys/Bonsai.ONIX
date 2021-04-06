using OpenCV.Net;

namespace Bonsai.ONIX
{
    /// <summary>
    /// One or more Neuropixels 1.0 "ultra-frames" each of which
    /// contains 12, 30kHz, spike samples and 1, 2.5 kHz LFP sample from each
    /// of the 384 electrodes.
    /// </summary>
    public class NeuropixelsV1DataFrame
    {
        public NeuropixelsV1DataFrame(NeuropixelsV1DataBlock dataBlock)
        {
            SpikeFrameClock = GetClock(dataBlock.SpikeDataClock);
            LFPFrameClock = GetClock(dataBlock.LFPDataClock);

            SpikeDataClock = GetClock(dataBlock.SpikeDataClock);
            LFPDataClock = GetClock(dataBlock.LFPDataClock);

            SpikeData = GetEphysData(dataBlock.SpikeData);
            LFPData = GetEphysData(dataBlock.LFPData);

            FrameCounter = GetCounter(dataBlock.CounterData);
        }

        Mat GetClock(ulong[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1); // TODO: abusing double to fit uint64_t
        }

        Mat GetEphysData(ushort[,] data)
        {
            if (data.Length == 0)
            {
                return null;
            }

            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        Mat GetCounter(int[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.S32, 1); // Abusing S32 to fit U20
        }

        public Mat SpikeFrameClock { get; private set; }

        public Mat LFPFrameClock { get; private set; }

        public Mat SpikeDataClock { get; private set; }

        public Mat LFPDataClock { get; private set; }

        public Mat SpikeData { get; private set; }

        public Mat LFPData { get; private set; }

        public Mat FrameCounter { get; private set; }

    }
}
