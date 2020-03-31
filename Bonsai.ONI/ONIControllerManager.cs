using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

// Behavior
// 1. Provide a global (static) storage of open ONIControllers
// 2. Only create a new one if it has a unique ID that is different than currently open and referenced context (defined by string and index)
// 3. Provide the ability to refresh the device map on an open context (this is more the context itself no?)

namespace Bonsai.ONI
{
    public static class ONIContextManager
    {
        public const string DefaultConfigurationFile = "ONI.config";

        static readonly Dictionary<string, Tuple<ONIController, RefCountDisposable>> openContexts = new Dictionary<string, Tuple<ONIController, RefCountDisposable>>();
        static readonly object openContextsLock = new object();

        //public static DeviceMapT FindMachingDevices(DeviceMapT map, oni.Device.DeviceID dev_id)
        //{
        //    // Find all matching devices
        //    return map.Where(
        //        pair => pair.Value.id == (uint)dev_id
        //    ).ToDictionary(x => x.Key, x => x.Value);
        //}

        //public static ONIContext ReserveContext(string context_name)
        //{
        //    return ReserveContext(context_name, ONIConfiguration.Default);
        //}


        public static ONIController ReserveContext(string driver, int index) //ONIConfiguration config)
        {
            var controller = default(Tuple<ONIController, RefCountDisposable>);

            lock (openContextsLock)
            {
                if (string.IsNullOrEmpty(driver))
                {
                    throw new ArgumentException("A driver name must be specified.", "driver");
                }

                // Create key
                var controller_key = driver + index.ToString();

                if (!openContexts.TryGetValue(controller_key, out controller)) // Controller not opened
                {

                    var new_controller = new ONIController();

                    var dispose = Disposable.Create(() =>
                    {
                        openContexts.Remove(controller_key);
                    });

                    var ref_count = new RefCountDisposable(dispose);
                    controller = Tuple.Create(new_controller, ref_count);
                    openContexts.Add(controller_key, controller);

                    return new ONIContext(new_controller, ref_count); // Disposable controller
                }
            }

            return new ONIContext(controller.Item1, controller.Item2.GetDisposable()); // New reference
        }
    }
}