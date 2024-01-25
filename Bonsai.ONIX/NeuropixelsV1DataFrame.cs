using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    /// TODO: Frames produced by Neuropixesl V1 headstages do not conform to
    /// ONI spec because the have the hub counter at the end of the frame.
    /// That is why this does not derive from U16DataBlockFrame

    /// <summary>
    /// One or more Neuropixels 1.0 "ultra-frames" each of which
    /// contains 12, 30kHz, spike samples and 1, 2.5 kHz LFP sample from each
    /// of the 384 electrodes.
    /// </summary>
    public class NeuropixelsV1DataFrame
    {
        public readonly int NumberofUltraFrames;
        private readonly int NumberOfSuperFrames;
        internal const int SuperframesPerUltraFrame = 12;
        private const int FramesPerSuperFrame = 13;
        private const int FramesPerUltraFrame = SuperframesPerUltraFrame * FramesPerSuperFrame;
        private const int NumberOfChannels = NeuropixelsV1Probe.CHANNEL_COUNT;
        private const int DataOffset = 1;
        private const int FrameWords = 36; // 32 ADCs + type + 2 counters

        // ADC number to frame index map
        private static readonly int[] adcToFrameIndex = {0, 7 , 14, 21, 28,
                                                         1, 8 , 15, 22, 29,
                                                         2, 9 , 16, 23, 30,
                                                         3, 10, 17, 24, 31,
                                                         4, 11, 18, 25, 32,
                                                         5, 12, 19, 26, 33,
                                                         6, 13 };

        // ADC number to starting channel number
        private static readonly int[] adcToChannel = {0, 1, 24, 25, 48, 49, 72, 73, 96, 97, 120,
            121, 144, 145, 168, 169, 192, 193, 216, 217, 240, 241, 264, 265, 288, 289, 312, 313,
            336, 337, 360, 361};

        public NeuropixelsV1DataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new Bonsai.WorkflowRuntimeException("Neuropixels V1 frame buffer is empty.");
            }

            if (frameBlock.Count % SuperframesPerUltraFrame != 0)
            {
                throw new WorkflowRuntimeException("Neuropixels V1 frame buffer is not a multiple of ultraframe size.");
            }

            var frameClock = new ulong[frameBlock.Count];
            var dataClock = new ulong[frameBlock.Count];

            for (int i = 0; i < frameBlock.Count; i++)
            {
                frameClock[i] = frameBlock[i].FrameClock - frameOffset;
                dataClock[i] = ((ulong)frameBlock[i].Sample[468] << 48) |
                               ((ulong)frameBlock[i].Sample[469] << 32) |
                               ((ulong)frameBlock[i].Sample[470] << 16) |
                               ((ulong)frameBlock[i].Sample[471] << 0);
            }

            Clock = GetClock(frameClock);
            HubSyncCounter = GetClock(dataClock);

            NumberofUltraFrames = frameBlock.Count / SuperframesPerUltraFrame;
            NumberOfSuperFrames = frameBlock.Count;
            var lfpFrameClock = new ulong[NumberofUltraFrames];
            var spikeFrameClock = new ulong[NumberofUltraFrames * SuperframesPerUltraFrame];
            var lfpDataClock = new ulong[NumberofUltraFrames];
            var spikeDataClock = new ulong[NumberofUltraFrames * SuperframesPerUltraFrame];
            var frameCounter = new int[NumberofUltraFrames * FramesPerUltraFrame];
            var lfpData = new ushort[NumberOfChannels, NumberofUltraFrames];
            var spikeData = new ushort[NumberOfChannels, NumberofUltraFrames * SuperframesPerUltraFrame];

            // Frame hierarchy
            int frameCount = 0;
            int superCount = 0;
            int ultraCount = 0;


            // Generate ultra-frames
            while (superCount < NumberOfSuperFrames)
            {

                var data = frameBlock[superCount].Sample;

                spikeFrameClock[superCount] = frameBlock[superCount].FrameClock;
                spikeDataClock[superCount] = ((ulong)data[468] << 48) | ((ulong)data[469] << 32) | ((ulong)data[470] << 16) | ((ulong)data[471] << 0);

                for (int i = 0; i < FramesPerSuperFrame; i++)
                {
                    var data_offset = DataOffset + i * FrameWords;
                    frameCounter[frameCount + i] = (data[data_offset + 27] << 10) | (data[data_offset + 34] << 0);

                    if (i == 0) // LFP data
                    {
                        var super_cnt_circ = superCount % SuperframesPerUltraFrame;

                        if (super_cnt_circ == 0) // Use the first superframe in ultraframe as time of this lfp-data round robin
                        {
                            lfpFrameClock[ultraCount] = frameBlock[superCount].FrameClock;
                            lfpDataClock[ultraCount] = spikeDataClock[superCount];
                        }

                        for (int adc = 0; adc < 32; adc++)
                        {
                            lfpData[adcToChannel[adc] + super_cnt_circ * 2, ultraCount] = (ushort)(data[adcToFrameIndex[adc] + data_offset] >> 5); // Q11.5 -> Q11.0
                        }

                    }
                    else // Spike data
                    {

                        var channelOffset = 2 * (i - 1);
                        for (int adc = 0; adc < 32; adc++)
                        {
                            spikeData[adcToChannel[adc] + channelOffset, superCount] = (ushort)(data[adcToFrameIndex[adc] + data_offset] >> 5); // Q11.5 -> Q11.0
                        }

                    }
                }

                superCount++;
                frameCount += FramesPerSuperFrame;

                if (superCount % SuperframesPerUltraFrame == 0)
                {
                    ultraCount++;
                }
            }

            // Project into Mats
            SpikeFrameClock = GetClock(spikeFrameClock);
            LFPFrameClock = GetClock(lfpFrameClock);
            SpikeDataClock = GetClock(spikeDataClock);
            LFPDataClock = GetClock(lfpDataClock);
            SpikeData = GetEphysData(spikeData);
            LFPData = GetEphysData(lfpData);
            FrameCounter = GetCounter(frameCounter);
        }


        // TODO: This copies!
        private static Mat GetClock(ulong[] data)
        {
            // TODO: abusing double to fit ulong
            // NB: OpenCV does not have a Depth.U64 which would allow to use of Mat.Header and Mat.Convert for zero-copy
            return Mat.FromArray(data, 1, data.Length, Depth.F64, 1);
        }


        private static Mat GetEphysData(ushort[,] data)
        {
            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.Convert(header, output);
            }

            return output;
        }

        private static Mat GetCounter(int[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.S32, 1);
        }


        /// <summary>
        /// The frame clock. Created by the host when receiving the sample from the device.
        /// </summary>
        public Mat Clock { get; private set; }

        /// <summary>
        /// The sample clock, create locally alongside the source device.
        /// </summary>
        public Mat HubSyncCounter { get; private set; }


        public Mat SpikeFrameClock { get; private set; }

        public Mat LFPFrameClock { get; private set; }

        public Mat SpikeDataClock { get; private set; }

        public Mat LFPDataClock { get; private set; }

        public Mat SpikeData { get; private set; }

        public Mat LFPData { get; private set; }

        public Mat FrameCounter { get; private set; }
    }
}
