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
        private static readonly Dictionary<string, Tuple<ONIContextTask, RefCountDisposable>> openContexts
            = new Dictionary<string, Tuple<ONIContextTask, RefCountDisposable>>();

        // Mirrors open_contexts but can be created by non-opening reservation calls (ReserveOpenContextAsync)
        private static readonly Dictionary<string, EventWaitHandle> contextWaitHandles
            = new Dictionary<string, EventWaitHandle>();
        private static readonly object openContextLock = new object();

        /// <summary>
        /// Reserve an ONI Context after it has already been opened by the appropriate call to
        /// <see cref="ReserveContext(ONIHardwareSlot, bool)"/>
        /// </summary>
        /// <param name="slot">Hardware slot to reserve.</param>
        /// <returns>Returns a <see cref="ONIContextDisposable"/> asynchronously after it has already been opened.</returns>
        public static async Task<ONIContextDisposable> ReserveOpenContextAsync(ONIHardwareSlot slot)
        {
#if DEBUG
            Console.WriteLine("Open context async slot " + slot + " reserved by " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().DeclaringType);
#endif
            lock (openContextLock)
            {
                if (!contextWaitHandles.TryGetValue(slot.MakeKey(), out EventWaitHandle waitHandle))
                {
                    contextWaitHandles.Add(slot.MakeKey(), new EventWaitHandle(false, EventResetMode.ManualReset));
                }
            }

            return await Task.Run(() =>
            {
                contextWaitHandles[slot.MakeKey()].WaitOne();
                return ReserveContext(slot);
            });
        }

        public static async Task<ONIContextDisposable> ReserveContextAsync(ONIHardwareSlot slot, bool releaseWaiting = false, CancellationToken ct = default)
        {
#if DEBUG
            Console.WriteLine("Context async slot " + slot + " reserved by " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().DeclaringType);
#endif
            return await Task.Run(() => ReserveContext(slot, releaseWaiting), ct);
        }

        public static ONIContextDisposable ReserveContext(ONIHardwareSlot slot, bool releaseWaiting = false, bool resetRunning = false)
        {
#if DEBUG
            Console.WriteLine("Context slot " + slot + " reserved by " + (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().DeclaringType);
#endif
            var contextCounted = default(Tuple<ONIContextTask, RefCountDisposable>);

            lock (openContextLock)
            {
                if (string.IsNullOrEmpty(slot.Driver))
                {
                    if (openContexts.Count == 1) contextCounted = openContexts.Values.Single();
                    else throw new ArgumentException("An ONI hardware slot must be specified.", nameof(slot));
                }

                if (!openContexts.TryGetValue(slot.MakeKey(), out contextCounted))
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
                            contextWaitHandles[slot.MakeKey()].Reset();

                            // Context and wait handles are removed from dictionaries together
                            openContexts.Remove(slot.MakeKey());
                            contextWaitHandles.Remove(slot.MakeKey());
                        });

                    if (!contextWaitHandles.TryGetValue(slot.MakeKey(), out EventWaitHandle waitHandle))
                    {
                        contextWaitHandles.Add(slot.MakeKey(), new EventWaitHandle(false, EventResetMode.ManualReset));
                    }

                    if (releaseWaiting)
                    {
                        contextWaitHandles[slot.MakeKey()].Set();
                    }

                    var referenenceCount = new RefCountDisposable(dispose);
                    contextCounted = Tuple.Create(ctx, referenenceCount);
                    openContexts.Add(slot.MakeKey(), contextCounted);
                    return new ONIContextDisposable(ctx, referenenceCount, openContextLock);

                }

                if (resetRunning)
                {
                    contextCounted.Item1.Reset();
                }
                if (releaseWaiting)
                {
                    // Will already be created if we in this portion of code.
                    contextWaitHandles[slot.MakeKey()].Set();
                }

                return new ONIContextDisposable(contextCounted.Item1, contextCounted.Item2.GetDisposable(), openContextLock);
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