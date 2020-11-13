using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.ONIX
{
    [Description("Acquires data from a single AD7617 analog to digital converter.")]
    public class AD7617Device : ONIFrameReaderDeviceBuilder<AnalogInputDataFrame>
    {
        public AD7617Device() : base(oni.Device.DeviceID.AD7617) { }

        public override IObservable<AnalogInputDataFrame> Process(IObservable<oni.Frame> source)
        {
            var data_block = new AnalogInputDataBlock(NumChannels, BlockSize);

            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Where(f =>
                {
                    return data_block.FillFromFrame(f);
                })
                .Select(f =>
                {
                    var sample = new AnalogInputDataFrame(data_block, FrameClockHz, DataClockHz);
                    data_block = new AnalogInputDataBlock(NumChannels, BlockSize);
                    return sample;
                });
        }

        [Category("Acquisition")]
        [Range(10, 10000)]
        [Description("The size of data blocks, in samples, that are propagated as events in the observable sequence.")]
        public int BlockSize { get; set; } = 250;

        // For now, this is not settable
        [System.Xml.Serialization.XmlIgnore]
        [Category("Configuration")]
        [Description("Number of channels begin used.")]
        public int NumChannels { get; private set; } = 12; 
    }
}
