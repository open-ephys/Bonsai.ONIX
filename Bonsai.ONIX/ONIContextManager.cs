using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
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

        // Mirrors open_contexts but can be created by non-opening reservation calls (ReserveOpenContextAsync)
        static readonly Dictionary<string, EventWaitHandle> contex_wait_handles
            = new Dictionary<string, EventWaitHandle>();

        static readonly object open_ctx_lock = new object();

        /// <summary>
        /// Reserve an ONI Context after it has already been opened by the appropriate call to
        /// <see cref="ReserveContext(ONIHardwareSlot, bool)"/>
        /// </summary>
        /// <param name="slot">Hardware slot to reserve.</param>
        /// <returns>Returns a <see cref="ONIContextDisposable"/> asynchronously after it has already been opened.</returns>
        public static async Task<ONIContextDisposable> ReserveOpenContextAsync(ONIHardwareSlot slot)
        {
            lock (open_ctx_lock)
            {
                if (!contex_wait_handles.TryGetValue(slot.MakeKey(), out EventWaitHandle wait_handle))
                {
                    contex_wait_handles.Add(slot.MakeKey(), new EventWaitHandle(false, EventResetMode.ManualReset));
                }
            }

            return await Task.Run(() =>
            {
                contex_wait_handles[slot.MakeKey()].WaitOne();
                return ReserveContext(slot);
            });
        }

        public static async Task<ONIContextDisposable> ReserveContextAsync(ONIHardwareSlot slot, bool releaseWaiting = false, CancellationToken ct = default)
        {
            return await Task.Run(() => ReserveContext(slot, releaseWaiting), ct);
        }

        public static ONIContextDisposable ReserveContext(ONIHardwareSlot slot, bool release_waiting = false)
        {
            var ctx_counted = default(Tuple<ONIContextTask, RefCountDisposable>);

            lock (open_ctx_lock)
            {
                if (string.IsNullOrEmpty(slot.Driver))
                {
                    if (open_contexts.Count == 1) ctx_counted = open_contexts.Values.Single();
                    else throw new ArgumentException("An ONI hardware slot must be specified.", nameof(slot));
                }

                if (!open_contexts.TryGetValue(slot.MakeKey(), out ctx_counted))
                {

                    var configuration = LoadConfiguration();
                    if (configuration.Contains(slot.MakeKey()))
                    {
                        slot = configuration[slot.MakeKey()];
                    }

                    // TODO: context open timeout. Is this reasonable??
                    var generateContext = Task.Run(() => new ONIContextTask(slot.Driver, slot.Index));
                    if (!generateContext.Wait(1000))
                    {
                        throw new TimeoutException("ONI aquisition context creation timed out.");
                    }
                    var ctx = generateContext.Result;

                    var dispose = Disposable.Create(() =>
                        {
                            ctx.Dispose();
                            contex_wait_handles[slot.MakeKey()].Reset();

                            // Context and wait handles are removed from dictionaries together
                            open_contexts.Remove(slot.MakeKey());
                            contex_wait_handles.Remove(slot.MakeKey());
                        });

                    if (!contex_wait_handles.TryGetValue(slot.MakeKey(), out EventWaitHandle wait_handle))
                    {
                        contex_wait_handles.Add(slot.MakeKey(), new EventWaitHandle(false, EventResetMode.ManualReset));
                    }

                    if (release_waiting)
                    {
                        contex_wait_handles[slot.MakeKey()].Set();
                    }

                    var ref_count = new RefCountDisposable(dispose);
                    ctx_counted = Tuple.Create(ctx, ref_count);
                    open_contexts.Add(slot.MakeKey(), ctx_counted);
                    return new ONIContextDisposable(ctx, ref_count);

                }

                if (release_waiting)
                {
                    // Will already be created if we in this portion of code.
                    contex_wait_handles[slot.MakeKey()].Set();
                }

                return new ONIContextDisposable(ctx_counted.Item1, ctx_counted.Item2.GetDisposable());
            }

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