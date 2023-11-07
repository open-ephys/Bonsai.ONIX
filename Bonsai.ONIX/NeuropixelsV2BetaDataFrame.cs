using OpenCV.Net;
using System.Collections.Generic;

namespace Bonsai.ONIX
{
    /// <summary>
    /// One or more Neuropixels 2.0 "super-frames" each of which
    /// contains a single sample of all 384 channels @ 30kHz.
    /// </summary>
    public class NeuropixelsV2BetaDataFrame : U16DataBlockFrame
    {

        private const int FramesPerSuperFrame = 16;
        private const int NumberOfChannels = 384;
        private const int ADCsPerProbe = 24;
        private const int NPDataStartOffset = 1; // Account for index
        private const int ADCDataStartOffset = 9;
        private const int FrameWords = 28; // sync + !sync + counter[0] + counter[1] + 24 ADCs

        // ADC & frame-index to channel mapping
        // First dimension: data index
        // Second dimension: frame index within super frame
        private static readonly int[,] RawToChannel = {
            { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 },                          // Data Index 9, ADC 1  
            { 96, 98, 100, 102, 104, 106, 108, 110, 112, 114, 116, 118, 120, 122, 124, 126 },       // Data Index 10, ADC 7 
            { 192, 194, 196, 198, 200, 202, 204, 206, 208, 210, 212, 214, 216, 218, 220, 222 },     // Data Index 11, ADC 13 
            { 288, 290, 292, 294, 296, 298, 300, 302, 304, 306, 308, 310, 312, 314, 316, 318 },     // Data Index 12, ADC 19 
            { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31 },                          // Data Index 13, ADC 2 
            { 97, 99, 101, 103, 105, 107, 109, 111, 113, 115, 117, 119, 121, 123, 125, 127 },       // Data Index 14, ADC 8 
            { 193, 195, 197, 199, 201, 203, 205, 207, 209, 211, 213, 215, 217, 219, 221, 223 },     // Data Index 15, ADC 14 
            { 289, 291, 293, 295, 297, 299, 301, 303, 305, 307, 309, 311, 313, 315, 317, 319 },     // Data Index 16, ADC 20 
            { 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52, 54, 56, 58, 60, 62 },                     // Data Index 17, ADC 3 
            { 128, 130, 132, 134, 136, 138, 140, 142, 144, 146, 148, 150, 152, 154, 156, 158 },     // Data Index 18, ADC 9 
            { 224, 226, 228, 230, 232, 234, 236, 238, 240, 242, 244, 246, 248, 250, 252, 254 },     // Data Index 19, ADC 15 
            { 320, 322, 324, 326, 328, 330, 332, 334, 336, 338, 340, 342, 344, 346, 348, 350 },     // Data Index 20, ADC 21 
            { 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53, 55, 57, 59, 61, 63 },                     // Data Index 21, ADC 4 
            { 129, 131, 133, 135, 137, 139, 141, 143, 145, 147, 149, 151, 153, 155, 157, 159 },     // Data Index 22, ADC 10 
            { 225, 227, 229, 231, 233, 235, 237, 239, 241, 243, 245, 247, 249, 251, 253, 255 },     // Data Index 23, ADC 16 
            { 321, 323, 325, 327, 329, 331, 333, 335, 337, 339, 341, 343, 345, 347, 349, 351 },     // Data Index 24, ADC 22 
            { 64, 66, 68, 70, 72, 74, 76, 78, 80, 82, 84, 86, 88, 90, 92, 94 },                     // Data Index 25, ADC 5 
            { 160, 162, 164, 166, 168, 170, 172, 174, 176, 178, 180, 182, 184, 186, 188, 190 },     // Data Index 26, ADC 11 
            { 256, 258, 260, 262, 264, 266, 268, 270, 272, 274, 276, 278, 280, 282, 284, 286 },     // Data Index 27, ADC 17 
            { 352, 354, 356, 358, 360, 362, 364, 366, 368, 370, 372, 374, 376, 378, 380, 382 },     // Data Index 28, ADC 23 
            { 65, 67, 69, 71, 73, 75, 77, 79, 81, 83, 85, 87, 89, 91, 93, 95 },                     // Data Index 29, ADC 6 
            { 161, 163, 165, 167, 169, 171, 173, 175, 177, 179, 181, 183, 185, 187, 189, 191 },     // Data Index 30, ADC 12 
            { 257, 259, 261, 263, 265, 267, 269, 271, 273, 275, 277, 279, 281, 283, 285, 287 },     // Data Index 31, ADC 18 
            { 353, 355, 357, 359, 361, 363, 365, 367, 369, 371, 373, 375, 377, 379, 381, 383 }      // Data Index 32, ADC 24 
         };


        public NeuropixelsV2BetaDataFrame(IList<ONIManagedFrame<ushort>> frameBlock, ulong frameOffset) //, double[] gains)
            : base(frameBlock, frameOffset)
        {
            if (frameBlock.Count == 0)
            {
                throw new WorkflowRuntimeException("Neuropixels V2 frame buffer is empty.");
            }

            Index = frameBlock[0].Sample[4]; // Assumed to already be grouped by Index

            var frameCounter = new int[FramesPerSuperFrame * frameBlock.Count];
            var spikeData = new ushort[NumberOfChannels, frameBlock.Count];

            // Total frame count
            var frameCount = 0;

            // Loop over 384-channel "super-frames"
            for (var i = 0; i < frameBlock.Count; i++)
            {

                var superFrame = frameBlock[i].Sample;

                // Loop over 16 "frames" within each "super frame"
                for (var j = 0; j < FramesPerSuperFrame; j++)
                {
                    var dataCount = NPDataStartOffset + j * FrameWords;
                    frameCounter[frameCount++] = (superFrame[dataCount + 2 + 4] << 14) | (superFrame[dataCount + 3 + 4] << 0);

                    // ADC data starts at index 9 in the data array
                    // The period of ADC data within data array is 28 words (24 ADCs, 2 Syncs, 2 counters)
                    var adcDataOffset = ADCDataStartOffset + j * FrameWords;
                    
                    // Loop over ADC samples within each "frame" and map to channel position
                    for (var k = 0; k < ADCsPerProbe; k++)
                    {
                        spikeData[RawToChannel[k, j], i] = superFrame[adcDataOffset + k];
                    }
                }
            }

            // Project into Mats
            // TODO: gain
            Ephys = GetEphysData(spikeData, 1.0); // gains[Index]);
            FrameCounter = GetCounter(frameCounter);
        }

        private static Mat GetEphysData(ushort[,] data, double gain)
        {
            var numChannels = data.GetLength(0);
            var numSamples = data.GetLength(1);

            var output = new Mat(numChannels, numSamples, Depth.U16, 1);
            using (var header = Mat.CreateMatHeader(data))
            {
                CV.ConvertScale(header, output, gain);
            }

            return output;
        }

        private static Mat GetCounter(int[] data)
        {
            return Mat.FromArray(data, 1, data.Length, Depth.S32, 1);
        }

        public int Index { get; private set; }

        public Mat Ephys { get; private set; }

        public Mat FrameCounter { get; private set; }
    }
}
