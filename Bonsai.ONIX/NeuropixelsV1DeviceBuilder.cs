using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Acquires data from a single Neuropixels v1.0 probe.")]
    [DefaultProperty("Configuration")]
    public class NeuropixelsV1DeviceBuilder : ONIFrameReaderDeviceBuilder<NeuropixelsV1DataFrame>
    {
        // Flex configuration reader
        NeuropixelsV1Flex flex;

        public NeuropixelsV1DeviceBuilder() : base(ONIXDevices.ID.NEUROPIX1R0)
        {
            Configuration = new NeuropixelsConfiguration
            {
                Channels = new NeuropixelsChannel[NeuropixelsV1Probe.CHANNEL_COUNT],
                ADCs = new NeuropixelsADC[NeuropixelsV1Probe.ADC_COUNT],
                Electrodes = new NeuropixelsElectrode[NeuropixelsV1Probe.ELECTRODE_COUNT]
            };

            for (int i = 0; i < Configuration.Channels.Length; i++)
            {
                Configuration.Channels[i] = new NeuropixelsChannel();
            }

            Configuration.InternalReferenceChannels = new int[] { NeuropixelsV1Probe.INTERNAL_REF_CHANNEL };

            for (int i = 0; i < Configuration.ADCs.Length; i++)
            {
                Configuration.ADCs[i] = new NeuropixelsADC();
            }

            for (int i = 0; i < Configuration.Electrodes.Length; i++)
            {
                Configuration.Electrodes[i] = new NeuropixelsElectrode();
            }
        }

        public override IObservable<NeuropixelsV1DataFrame> Process(IObservable<oni.Frame> source)
        {
            // Configure probe
            var probe = new NeuropixelsV1Probe(HardwareSlot, DeviceIndex.SelectedIndex);
            probe.WriteConfiguration(Configuration, PerformReadCheck);

            var data_block = new NeuropixelsV1DataBlock(BlockSize);

            return source.Where(f =>
                {
                    return data_block.FillFromFrame(f);
                })
                .Select(f =>
                {
                    var sample = new NeuropixelsV1DataFrame(data_block);
                    data_block = new NeuropixelsV1DataBlock(BlockSize);
                    return sample;
                });
        }

        public override Expression Build(IEnumerable<Expression> arguments)
        {
            var r = base.Build(arguments);

            flex = new NeuropixelsV1Flex(HardwareSlot, DeviceIndex.SelectedIndex);
            Configuration.ProbeSN = flex.ProbeSN;
            Configuration.ProbeType = flex.ProbePartNo;

            DeviceIndex.IndexChanged += flex.Update;

            return r;
        }

        [Category("Metadata")]
        [Description("The probe serial number.")]
        [System.Xml.Serialization.XmlIgnore]
        public ulong? ProbeSN => flex?.ProbeSN;

        [Category("Metadata")]
        [Description("The flex cable version.")]
        [System.Xml.Serialization.XmlIgnore]
        public string FlexVersion => flex?.Version;

        [Category("Metadata")]
        [Description("The probe part number.")]
        [System.Xml.Serialization.XmlIgnore]
        public string FlexPartNo => flex?.PartNo;

        [Category("Metadata")]
        [Description("The probe part number.")]
        [System.Xml.Serialization.XmlIgnore]
        public string ProbePartNo => flex?.ProbePartNo;

        [Category("Configuration")]
        [Description("Neuropixels probe hardware configuration.")]
        [Editor("Bonsai.ONIX.Design.NeuropixelsEditor, Bonsai.ONIX.Design", typeof(UITypeEditor))]
        [Externalizable(false)]
        public NeuropixelsConfiguration Configuration { get; set; }

        [Category("Configuration")]
        [Description("Write configuration twice to perform a bit-wise confirmation of configuration values on the probe.")]
        public bool PerformReadCheck { get; set; } = false;

        [Category("Acquisition")]
        [Range(1, 1e6)]
        [Description("The size of data blocks, in units of \"hyper-frames\", that are propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 100;

    }
}
