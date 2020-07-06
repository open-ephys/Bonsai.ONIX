using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Drawing.Design;

namespace Bonsai.ONI
{
    [Description("Acquires data from a single Neuropixels-1.0 chip.")]
    public class Neuropixels1R0Device : ONIFrameReaderDeviceBuilder<Neuropixels1R0DataFrame>
    {
        // Control registers (see oedevices.h)
        //enum Register
        //{
        //      TODO
        //}

        public Neuropixels1R0Device() : base(oni.Device.DeviceID.NEUROPIX1R0) { }

        public override IObservable<Neuropixels1R0DataFrame> Process(IObservable<oni.Frame> source)
        {
            var data_block = new Neuropixels1R0DataBlock(BlockSize);

            return source
                .Where(f => f.DeviceIndex() == DeviceIndex.SelectedIndex)
                .Where(f =>
                {
                    return data_block.FillFromFrame(f);
                })
                .Select(f =>
                {
                    var sample = new Neuropixels1R0DataFrame(data_block, ClockHz);
                    data_block = new Neuropixels1R0DataBlock(BlockSize);
                    return sample;
                });
        }

        [Category("Acquisition")]
        [Range(0, 100)]
        [Description("The size of data blocks, in round-robin samples, that are propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 1;

        [Category("Configuration")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        [Description("Gain Calibration CSV")]
        public string GainCalCSV { get; set; }

        [Category("Configuration")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        [Description("ADC Calibration CSV")]
        public string ADCCalCSV { get; set; }

        [Category("Configuration")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        [Description("Active Channels CSV")]
        public string ChannelsCSV { get; set; }

        [Category("Testing")]
        [Description("Enable TEST mode (generate sine wave on specified channel)")]
        public bool TestMode { get; set; }

    }
}
