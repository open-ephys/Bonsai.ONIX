using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.ONI
{
    /// <summary>
    /// Provides an low-level representation of a multi round-robin sample of a Neuropixels6B Chip
    /// </summary>
    public class Neuropixels1R0DataBlock
    {
        private int HyperFramesPerBlock;
        public const int NumChannels = 384;
        private const int data_offset = 5;

        private readonly int block_idx = 0;

        // Total frames
        private int total_frame_cnt = 0;
        private int total_super_cnt = 0;

        // Frame hierarchy
        private int spike_frame_cnt = 0;
        private int frame_cnt = 0;
        private int super_cnt = 0;
        private int hyper_cnt = 0;

        // TODO: Would rather used the top map, but its wrong because of 
        // to vs downto somewhere in firmware
        //public static readonly int[] chan_map = {0 , 7 , 14, 21, 28,
        //                                         1 , 8 , 15, 22, 29,
        //                                         2 , 9 , 16, 23, 30,
        //                                         3 , 10, 17, 24, 31,
        //                                         4 , 11, 18, 25, 32,
        //                                         5 , 12, 19, 26, 33,
        //                                         6 , 13 };

        public static readonly int[] chan_map = {6, 13, 20, 27, 34,
                                                 5, 12, 19, 26, 33,
                                                 4, 11, 18, 25, 32,
                                                 3, 10, 17, 24, 31,
                                                 2,  9, 16, 23, 30,
                                                 1,  8, 15, 22, 29,
                                                 0,  7 };

        readonly ulong[] lfpClock;
        readonly ulong[] spikeClock;
        readonly uint[] counterData;
        readonly ushort[] frameTypeData;
        readonly ushort[,] spikeData;
        readonly ushort[,] lfpData;

        public Neuropixels1R0DataBlock(int hyper_frames_per_block = 1)
        {
            HyperFramesPerBlock = hyper_frames_per_block;

            AllocateArray1D(ref spikeClock, HyperFramesPerBlock * 12);
            AllocateArray1D(ref lfpClock, HyperFramesPerBlock);

            AllocateArray1D(ref counterData, HyperFramesPerBlock * 13 * 12);
            AllocateArray1D(ref frameTypeData, HyperFramesPerBlock * 13 * 12);

            AllocateArray2D(ref spikeData, NumChannels, HyperFramesPerBlock * 12);
            AllocateArray2D(ref lfpData, NumChannels, HyperFramesPerBlock);
        }

        public bool FillFromFrame(oni.Frame frame)
        {
            if (block_idx >= HyperFramesPerBlock)
                throw new IndexOutOfRangeException();

            // [uint64_t local_clock, uint16_t frame_type, ephys1, uint16_t ephys2, ... , uint16_t aux1, uint16_t aux2, ...]
            var data = frame.Data<ushort>();

            // TODO: This should be done automatically by firmware, seems like buffers are not getting cleared during reset
            if (total_frame_cnt == 0 && data[4] == 207)
                return false;

            // Frame type and counter
            frameTypeData[total_frame_cnt] = data[4];
            //counterData[total_frame_cnt] = ((uint)data[27] << 16) | ((uint)data[34] << 0);
            counterData[total_frame_cnt] = ((uint)data[21 + data_offset] << 16) | ((uint)data[28 + data_offset] << 0);

            if (frame_cnt == 0) // This one is LFP data
            {
                if (super_cnt == 0) // Use the first superframe in hyperframe as time of this spike-data round robin
                {
                    lfpClock[hyper_cnt] = ((ulong)data[0] << 48) | ((ulong)data[1] << 32) | ((ulong)data[2] << 16) | ((ulong)data[3] << 0);
                }

                for (int chan = 0; chan < 32; chan++)
                {
                    lfpData[chan + super_cnt * 32, hyper_cnt] = data[chan_map[chan] + data_offset]; // Start at index 6
                }

            } else { // Spike data

                if (frame_cnt == 1) // Use the first frame in superframe as time of this spike-data round robin
                {
                    spikeClock[super_cnt] = ((ulong)data[0] << 48) | ((ulong)data[1] << 32) | ((ulong)data[2] << 16) | ((ulong)data[3] << 0);
                }

                //spikeClock[block_idx] = ((ulong)data[0] << 48) | ((ulong)data[1] << 32) | ((ulong)data[2] << 16) | ((ulong)data[3] << 0);

                for (int chan = 0; chan < 32; chan++)
                {
                    spikeData[chan + spike_frame_cnt * 32, total_super_cnt] = data[chan_map[chan] + data_offset]; // Start at index 6
                }

                spike_frame_cnt++;
            }

            if (frame_cnt == 12)
            {
                total_super_cnt++;
                if (++super_cnt == 12)
                {
                    hyper_cnt++;
                    super_cnt = 0;
                }

                spike_frame_cnt = 0;
                frame_cnt = 0;
            }
            else
            {
                frame_cnt++;
            }
            total_frame_cnt++;
            return hyper_cnt == HyperFramesPerBlock;
        }

        // Allocates memory for a 1-D array of integers.
        void AllocateArray1D(ref ushort[] array1D, int xSize)
        {
            Array.Resize(ref array1D, xSize);
        }

        // Allocates memory for a 1-D array of integers.
        void AllocateArray1D(ref uint[] array1D, int xSize)
        {
            Array.Resize(ref array1D, xSize);
        }

        // Allocates memory for a 1-D array of integers.
        void AllocateArray1D(ref ulong[] array1D, int xSize)
        {
            Array.Resize(ref array1D, xSize);
        }

        // Allocates memory for a 2-D array of ushorts.
        void AllocateArray2D(ref ushort[,] array2D, int xSize, int ySize)
        {
            array2D = new ushort[xSize, ySize];
        }

        /// <summary>
        /// Gets the array of 64-bit unsigned local hardware clock for lfp round-robin samples
        /// </summary>
        public ulong[] LFPClock
        {
            get { return lfpClock; }
        }

        /// <summary>
        /// Gets the array of 64-bit unsigned local hardware clock for spike round-robin samples
        /// </summary>
        public ulong[] SpikeClock
        {
            get { return spikeClock; }
        }

        /// <summary>
        /// Gets the array of multidimensional spike-band data.
        /// </summary>
        public ushort[,] SpikeData
        {
            get { return spikeData; }
        }

        /// <summary>
        /// Gets the array of multidimensional lfp-band data.
        /// </summary>
        public ushort[,] LFPData
        {
            get { return lfpData; }
        }

        /// <summary>
        /// Gets the array of frame-counter data.
        /// </summary>
        public uint[] CounterData
        {
            get { return counterData; }
        }

        /// <summary>
        /// Gets the array of frame-type data.
        /// </summary>
        public ushort[] FrameType
        {
            get { return frameTypeData; }
        }

    }
}
