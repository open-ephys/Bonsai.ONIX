﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace Bonsai.ONIX
{
    [ONIXDeviceID(DeviceID.HARPSyncInput)]
    [Description("Receives time input from a HARP synchronization source")]
    public class HARPSyncInputDevice : ONIFrameReader<HARPSyncInputDataFrame, ushort>
    {
        private enum Register
        {
            ENABLE = 0
        }

        protected override IObservable<HARPSyncInputDataFrame> Process(IObservable<ONIManagedFrame<ushort>> source, ulong frameOffset)
        {
            return source.Select(f => { return new HARPSyncInputDataFrame(f, frameOffset); });
        }

        public override ONIDeviceAddress DeviceAddress { get; set; } = new ONIDeviceAddress();

        [Category("Configuration")]
        [Description("Enable the device data stream.")]
        public bool EnableStream
        {
            get
            {
                return ReadRegister((uint)Register.ENABLE) > 0;
            }
            set
            {
                WriteRegister((uint)Register.ENABLE, value ? (uint)1 : 0);
            }
        }

    }
}
