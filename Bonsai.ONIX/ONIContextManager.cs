using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Bonsai.ONIX
{
    public static class ONIContextManager
    {
        public const string DefaultConfigurationFile = "ONIX.config";
        static readonly Dictionary<string, Tuple<ONIContextTask, RefCountDisposable>> open_contexts
            = new Dictionary<string, Tuple<ONIContextTask, RefCountDisposable>>();
        static readonly object open_ctx_lock = new object();

        public static async Task<ONIContextDisposable> ReserveContextAsync(ONIHardwareSlot slot)
        {
            return await Task.Run(() => ReserveContext(slot));
        }

        public static ONIContextDisposable ReserveContext(ONIHardwareSlot slot)
        {
            var ctx_counted = default(Tuple<ONIContextTask, RefCountDisposable>);

            lock (open_ctx_lock)
            {
                if (!open_contexts.TryGetValue(slot.MakeKey(), out ctx_counted))
                {

                    var configuration = LoadConfiguration();
                    if (configuration.Contains(slot.MakeKey()))
                    {
                        slot = configuration[slot.MakeKey()];
                    }

                    var ctx = new ONIContextTask(slot.Driver, slot.Index);

                    var dispose = Disposable.Create(() =>
                    {
                        ctx.Dispose();
                        open_contexts.Remove(slot.MakeKey());
                    });

                    var ref_count = new RefCountDisposable(dispose);
                    ctx_counted = Tuple.Create(ctx, ref_count);
                    open_contexts.Add(slot.MakeKey(), ctx_counted);
                    return new ONIContextDisposable(ctx, ref_count);
                }
            }

            return new ONIContextDisposable(ctx_counted.Item1, ctx_counted.Item2.GetDisposable());
        }

        public static ONIHardwareSlotCollection LoadConfiguration()
        {
            if (!File.Exists(DefaultConfigurationFile))
            {
                return new ONIHardwareSlotCollection();
            }

            var serializer = new XmlSerializer(typeof(ONIHardwareSlotCollection));
            using (var reader = XmlReader.Create(DefaultConfigurationFile))
            {
                return (ONIHardwareSlotCollection)serializer.Deserialize(reader);
            }
        }

        public static void SaveConfiguration(ONIHardwareSlotCollection configuration)
        {
            var serializer = new XmlSerializer(typeof(ONIHardwareSlotCollection));
            using (var writer = XmlWriter.Create(DefaultConfigurationFile, new XmlWriterSettings { Indent = true }))
            {
                serializer.Serialize(writer, configuration);
            }
        }

    }
}