using OpenCV.Net;
using System;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    /// TODO: This thing is _horribly_ inefficient. We are going to wait for the refactor which doesn't m
    /// make such heavy use of Mats for internal processing and has more mechanisms for zero copy.

    /// <summary>
    /// One or more Neuropixels 1.0 "ultra-frames" each of which
    /// contains 12, 30kHz, spike samples and 1, 2.5 kHz LFP sample from each
    /// of the 384 electrodes.
    /// </summary>
    
    public class NeuropixelsV1eDataFrame : U16DataBlockFrame
    {
        private readonly int NumberofUltraFrames;
        private readonly int NumberOfSuperFrames;
        internal const int SuperframesPerUltraFrame = 12;
        private const int FramesPerSuperFrame = 13;
        private const int FramesPerUltraFrame = SuperframesPerUltraFrame * FramesPerSuperFrame;
        private const int NumberOfChannels = NeuropixelsV1Probe.CHANNEL_COUNT;
        private const int DataOffset = 5;
        private const int FrameWords = 40; // 1 Sync + 4 Heartbeats + 1 reserved + 2 counters + 32 ADCs = 40

        // ADC number to frame index map
        private static readonly int[] adcToFrameIndex = {1, 9 , 17, 25, 33,
                                                         2, 10, 18, 26, 34,
                                                         3, 11, 19, 27, 35,
                                                         4, 12, 20, 28, 36,
                                                         5, 13, 21, 29, 37,
                                                         6, 14, 22, 30, 38,
                                                         7, 15 };

        // ADC number to starting channel number
        private static readonly int[] adcToChannel = {0, 1, 24, 25, 48, 49, 72, 73, 96, 97, 120,
            121, 144, 145, 168, 169, 192, 193, 216, 217, 240, 241, 264, 265, 288, 289, 312, 313,
            336, 337, 360, 361};

        public NeuropixelsV1eDataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset, ushort apGain, ushort lfpGain, ushort[] threshold, ushort[] offset) 
            : base(frameBlock, frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new WorkflowRuntimeException("Neuropixels V1 frame buffer is empty.");
            }

            if (frameBlock.Count % SuperframesPerUltraFrame != 0)
            {
                throw new WorkflowRuntimeException("Neuropixels V1 frame buffer is not a multiple of ultraframe size.");
            }

            NumberofUltraFrames = frameBlock.Count / SuperframesPerUltraFrame;
            NumberOfSuperFrames = frameBlock.Count;

            // Temporary storage
            var lfpClock = new ulong[NumberofUltraFrames];
            var frameCounter = new int[NumberofUltraFrames * FramesPerUltraFrame];
            var lfpData = new ushort[NumberOfChannels, NumberofUltraFrames];
            var spikeData = new ushort[NumberOfChannels, NumberofUltraFrames * SuperframesPerUltraFrame];

            // Frame hierarchy
            int frameCount = 0;
            int superCount = 0;
            int ultraCount = 0;

            // Generate ultra-frames
            while (superCount < NumberOfSuperFrames) // Create ultra-frames
            {
                var data = frameBlock[superCount].Sample;

                for (int i = 0; i < FramesPerSuperFrame; i++) // Parse super-frames
                {
                    var dataOffset = DataOffset + i * FrameWords;
                    
                    frameCounter[frameCount + i] = (data[dataOffset + 31] << 10) | (data[dataOffset + 39] << 0);

                    if (i == 0) // LFP data
                    {
                        var circularSuperCount = superCount % SuperframesPerUltraFrame;

                        // TODO: My God
                        if (circularSuperCount == 0)
                        {
                            lfpClock[ultraCount] = BitConverter.ToUInt64(BitConverter.GetBytes(Clock[superCount].Val0), 0);
                        }

                        for (int adc = 0; adc < 32; adc++)
                        {
                            var d = data[adcToFrameIndex[adc] + dataOffset];

                            if (d >= threshold[adc])
                            {
                                d -= offset[adc];
                            }

                            // Gain correction
                            lfpData[adcToChannel[adc] + circularSuperCount * 2, ultraCount] = (ushort)((d * lfpGain) >> 14);  // Q10.0 * Q1.14 -> Q10.0
                        }

                    }
                    else // Spike data
                    {

                        var channelOffset = 2 * (i - 1);
                        for (int adc = 0; adc < 32; adc++)
                        {

                            var d = data[adcToFrameIndex[adc] + dataOffset];

                            if (d >= threshold[adc])
                            {
                                d -= offset[adc];
                            }
                  
                            // Gain correction
                            spikeData[adcToChannel[adc] + channelOffset, superCount] = (ushort)((d * lfpGain) >> 14); // Q10.0 * Q1.14 -> Q10.0
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
            LFPClock = GetClock(lfpClock);
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

        public Mat LFPClock { get; private set; }

        public Mat SpikeData { get; private set; }

        public Mat LFPData { get; private set; }

        public Mat FrameCounter { get; private set; }
    }
}
