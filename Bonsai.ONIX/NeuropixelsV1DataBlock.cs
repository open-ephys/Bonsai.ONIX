using System;

namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides an low-level representation of a multi round-robin sample of a Neuropixels6B Chip
    /// </summary>
    public class NeuropixelsV1DataBlock
    {
        readonly int HYPERFRAMES_PER_BLOCK;
        public const int NUM_CHANNELS = 384;
        private const int SYNC_OFFSET = 4;
        private const int DATA_OFFSET = 5;
        private const int FRAMES_PER_SUPER = 13;
        private const int SUPERS_PER_HYPER = 12;
        private const int FRAME_WORDS = 36;
        //private const int block_idx = 0;

        // Frame hierarchy
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

        readonly ulong[] lfp_frame_clock;
        readonly ulong[] spike_frame_clock;
        readonly ulong[] lfp_data_clock;
        readonly ulong[] spike_data_clock;
        readonly int[] counter_data;
        readonly ushort[,] spike_data;
        readonly ushort[,] lfp_data;

        public NeuropixelsV1DataBlock(int hyper_frames_per_block = 1)
        {
            HYPERFRAMES_PER_BLOCK = hyper_frames_per_block;

            AllocateArray1D(ref spike_frame_clock, HYPERFRAMES_PER_BLOCK * 12);
            AllocateArray1D(ref lfp_frame_clock, HYPERFRAMES_PER_BLOCK);

            AllocateArray1D(ref spike_data_clock, HYPERFRAMES_PER_BLOCK * 12);
            AllocateArray1D(ref lfp_data_clock, HYPERFRAMES_PER_BLOCK);

            AllocateArray2D(ref spike_data, NUM_CHANNELS, HYPERFRAMES_PER_BLOCK * 12);
            AllocateArray2D(ref lfp_data, NUM_CHANNELS, HYPERFRAMES_PER_BLOCK);

            AllocateArray1D(ref counter_data, HYPERFRAMES_PER_BLOCK * 13 * 12);
        }

        // frame contains a single super frame
        public bool FillFromFrame(oni.Frame frame)
        {

            var data = frame.DataU16();

            // TODO: This should be done automatically by firmware, seems like buffers are not getting cleared during reset
            if (data[4] != 816)
            {
                return false;
            }

            spike_frame_clock[super_cnt] = frame.Clock();
            spike_data_clock[super_cnt] = ((ulong)data[0] << 48) | ((ulong)data[1] << 32) | ((ulong)data[2] << 16) | ((ulong)data[3] << 0);

            for (int i = 0; i < FRAMES_PER_SUPER; i++)
            {
                var count_offset = SYNC_OFFSET + i * FRAME_WORDS;
                counter_data[frame_cnt + i] = (data[count_offset + 21] << 10) | (data[count_offset + 28] << 0);

                if (i == 0) // This one is LFP data
                {
                    var super_cnt_circ = super_cnt % SUPERS_PER_HYPER;
                    if (super_cnt_circ == 0) // Use the first superframe in hyperframe as time of this lfp-data round robin
                    {
                        lfp_frame_clock[hyper_cnt] = frame.Clock();
                        lfp_data_clock[hyper_cnt] = ((ulong)data[0] << 48) | ((ulong)data[1] << 32) | ((ulong)data[2] << 16) | ((ulong)data[3] << 0);
                    }

                    for (int chan = 0; chan < 32; chan++)
                    {
                        lfp_data[chan + super_cnt_circ * 32, hyper_cnt] = data[chan_map[chan] + DATA_OFFSET]; // Start at index 6
                    }

                }
                else // Spike data
                {

                    var spike_frame_cnt = i - 1;
                    for (int chan = 0; chan < 32; chan++)
                    {
                        spike_data[chan + spike_frame_cnt * 32, super_cnt] = data[chan_map[chan] + DATA_OFFSET]; // Start at index 6
                    }

                }
            }

            super_cnt++;
            frame_cnt += FRAMES_PER_SUPER;

            if (super_cnt % SUPERS_PER_HYPER == 0)
            {
                hyper_cnt++;
            }

            return hyper_cnt == HYPERFRAMES_PER_BLOCK;
        }

        // Allocates memory for a 1D array of integers.
        void AllocateArray1D(ref int[] array1D, int xSize)
        {
            Array.Resize(ref array1D, xSize);
        }

        // Allocates memory for a 1D array of ulongs.
        void AllocateArray1D(ref ulong[] array1D, int xSize)
        {
            Array.Resize(ref array1D, xSize);
        }

        // Allocates memory for a 2D array of ushorts.
        void AllocateArray2D(ref ushort[,] array2D, int xSize, int ySize)
        {
            array2D = new ushort[xSize, ySize];
        }

        /// <summary>
        /// Gets the array of 64-bit unsigned frame clock for lfp round-robin samples
        /// </summary>
        public ulong[] LFPFrameClock
        {
            get { return lfp_frame_clock; }
        }

        /// <summary>
        /// Gets the array of 64-bit unsigned frame clock for spike round-robin samples
        /// </summary>
        public ulong[] SpikeFrameClock
        {
            get { return spike_frame_clock; }
        }

        /// <summary>
        /// Gets the array of 64-bit unsigned local hardware clock for lfp round-robin samples
        /// </summary>
        public ulong[] LFPDataClock
        {
            get { return lfp_data_clock; }
        }

        /// <summary>
        /// Gets the array of 64-bit unsigned local hardware clock for spike round-robin samples
        /// </summary>
        public ulong[] SpikeDataClock
        {
            get { return spike_data_clock; }
        }

        /// <summary>
        /// Gets the array of multidimensional spike-band data.
        /// </summary>
        public ushort[,] SpikeData
        {
            get { return spike_data; }
        }

        /// <summary>
        /// Gets the array of multidimensional lfp-band data.
        /// </summary>
        public ushort[,] LFPData
        {
            get { return lfp_data; }
        }

        /// <summary>
        /// Gets the array of frame-counter data.
        /// </summary>
        public int[] CounterData
        {
            get { return counter_data; }
        }

        public bool Valid
        {
            get; private set;
        }
    }
}
