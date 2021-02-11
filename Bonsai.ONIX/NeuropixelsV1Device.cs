using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [Description("Acquires a stream of \"ultra-frames\" from a single Neuropixels v1.0 probe.")]
    [DefaultProperty("Configuration")]
    public class NeuropixelsV1Device : ONIFrameReader<NeuropixelsV1DataFrame>
    {
        public NeuropixelsV1Device() : base(ONIXDevices.ID.NEUROPIX1R0)
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

        protected override IObservable<NeuropixelsV1DataFrame> Process(IObservable<oni.Frame> source)
        {

            var data_block = new NeuropixelsV1DataBlock(BlockSize);

            return Observable.Concat(

                // First sequence is Empty but starts the probe after context reset so that data is aligned
                Observable.Create<NeuropixelsV1DataFrame>(observer =>
                {
                    using (var probe = new NeuropixelsV1Probe(DeviceAddress))
                    {
                        probe.Reset();
                        probe.WriteConfiguration(Configuration, PerformReadCheck);
                        probe.Start();
                    }
                    observer.OnCompleted();
                    return Disposable.Empty;
                }),

                // Second sequence filters resulting frame stream
                source.Where(f =>
                {
                    return data_block.FillFromFrame(f);
                })
                .Select(f =>
                {
                    var sample = new NeuropixelsV1DataFrame(data_block);
                    data_block = new NeuropixelsV1DataBlock(BlockSize);
                    return sample;
                })
                .Finally(() =>
                {
                    using (var probe = new NeuropixelsV1Probe(DeviceAddress))
                    {
                        probe.Reset();
                    }
                })
            );
        }

        protected override void DeviceIndexChanged()
        {
            using (var flex = new NeuropixelsV1Flex(DeviceAddress))
            {
                Configuration.ProbeSN = flex.ProbeSN;
                Configuration.ProbePartNo = flex.ProbePartNo;

                Configuration.FlexPartNo = flex.PartNo;
                Configuration.FlexVersion = flex.Version;
            }
        }

        [Category("Configuration")]
        [Description("The probe serial number.")]
        [System.Xml.Serialization.XmlIgnore]
        public ulong? ProbeSN => Configuration.ProbeSN;

        [Category("Configuration")]
        [Description("The probe part number.")]
        [System.Xml.Serialization.XmlIgnore]
        public string ProbePartNo => Configuration.ProbePartNo;

        [Category("Configuration")]
        [Description("The flex cable version.")]
        [System.Xml.Serialization.XmlIgnore]
        public string FlexVersion => Configuration.FlexVersion;

        [Category("Configuration")]
        [Description("The probe part number.")]
        [System.Xml.Serialization.XmlIgnore]
        public string FlexPartNo => Configuration.FlexPartNo;

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
        [Description("The size of data blocks, in units of \"ultra-frames\", that are propagated in the observable sequence.")]
        public int BlockSize { get; set; } = 1;

    }
}
