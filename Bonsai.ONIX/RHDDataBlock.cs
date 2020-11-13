namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides an low-level representation of a multi round-robbin sample of an RHD Chip
    /// </summary>
    public class RHDDataBlock : DataBlock
    {
        public readonly int NumChannels;
        public readonly int NumAuxInChannels;

        ushort[,] ephysData;
        int[,] auxiliaryData;

        public RHDDataBlock(int num_ephys_channels,
                            int samples_per_block,
                            int num_aux_in_channels = 3) : base(samples_per_block)
        {
            NumChannels = num_ephys_channels;
            NumAuxInChannels = num_aux_in_channels;

            AllocateArray2D(ref ephysData, num_ephys_channels, samples_per_block);
            AllocateArray2D(ref auxiliaryData, num_aux_in_channels, samples_per_block);
        }

        protected override void FillFromData(ushort[] data)
        {
            int chan = 0; 
            for (; chan < NumChannels; chan++)
            {
                ephysData[chan, index] = data[chan + 4]; // Start at index 4
            }
            for (int k = 0; k < NumAuxInChannels; k++)
            {
                auxiliaryData[k, index] = data[4 + chan++]; 
            }
        }

        /// <summary>
        /// Gets the array of multidimensional amplifier data samples, indexed by data stream.
        /// </summary>
        public ushort[,] EphysData
        {
            get { return ephysData; }
        }

        /// <summary>
        /// Gets the array of multidimensional auxiliary data samples, indexed by data stream.
        /// </summary>
        public int[,] AuxiliaryData
        {
            get { return auxiliaryData; }
        }
    }
}
