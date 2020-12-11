namespace Bonsai.ONIX
{
    /// <summary>
    /// Provides an low-level representation of a multi round-robin sample of an AD7617 Chip
    /// </summary>
    public class AnalogInputDataBlock : DataBlock
    {
        readonly int num_channels;
        short[,] raw;

        public AnalogInputDataBlock(int num_channels, int samples_per_block)
            : base(samples_per_block)
        {
            this.num_channels = num_channels;
            AllocateArray2D(ref raw, num_channels, samples_per_block);
        }

        protected override void FillFromData(ushort[] data)
        {
            for (int chan = 0; chan < num_channels; chan++)
            {
                raw[chan, index] = (short)data[chan + 4]; // Start at index 4
            }
        }

        /// <summary>
        /// Gets the array of multidimensional amplifier data samples, indexed by data stream.
        /// </summary>
        public short[,] Data
        {
            get { return raw; }
        }
    }
}
