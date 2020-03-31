using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.ONI
{
    [Description("Acquires data from a single RHDxxxx bioamplifier chip.")]
    public class AD7617Device : ONIFrameReaderDeviceBuilder<AD7617DataFrame>
    {
        public AD7617Device() : base(oni.Device.DeviceID.AD7617) { }

        public override IObservable<AD7617DataFrame> Process(IObservable<oni.Frame> source)
        {
            var data_block = new AD7617DataBlock(NumChannels, BlockSize);

            return source
                .Where(f => f.DeviceIndices.Contains(DeviceIndex.SelectedIndex))
                .Where(f =>
                {
                    return data_block.FillFromFrame(f, DeviceIndex.SelectedIndex);
                })
                .Select(f =>
                {
                    var sample = new AD7617DataFrame(data_block, Controller.AcqContext.SystemClockHz);
                    data_block = new AD7617DataBlock(NumChannels, BlockSize);
                    return sample;
                });
        }

        [Range(10, 10000)]
        [Description("The size of data blocks, in samples, that are propogated in the observable sequence.")]
        public int BlockSize { get; set; } = 250;

        // For now, this is not settable
        [System.Xml.Serialization.XmlIgnore] 
        [Description("Number of channels.")]
        public int NumChannels { get; private set; } = 12; 
    }
}
