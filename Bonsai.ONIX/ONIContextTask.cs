using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    public class ONIContextTask : IDisposable
    {
        readonly oni.Context ctx;

        Task CollectFrames;

        // TODO: Multi-writer, thread safe FIFO for oni_write()'s
        private Task WriteData;
        System.Collections.Concurrent.BlockingCollection<oni.Frame> write_queue = new System.Collections.Concurrent.BlockingCollection<oni.Frame>();

        CancellationTokenSource TokenSource;
        CancellationToken CollectFramesToken;

        internal event EventHandler<FrameReceivedEventArgs> FrameReceived;

        public readonly uint SystemClockHz;
        public readonly uint AcquisitionClockHz;
        public readonly uint MaxReadFrameSize;
        public readonly uint MaxWriteFrameSize;
        public readonly Dictionary<uint, oni.device_t> DeviceTable;

        public static readonly string DefaultDriver = "riffa";
        public static readonly int DefaultIndex = 0;

        public ONIContextTask(string driver, int index)
        {
            ctx = new oni.Context(driver, index);
            SystemClockHz = ctx.SystemClockHz;
            AcquisitionClockHz = ctx.AcquisitionClockHz;
            MaxReadFrameSize = ctx.MaxReadFrameSize;
            MaxWriteFrameSize = ctx.MaxWriteFrameSize;
            DeviceTable = ctx.DeviceTable;
        }

        internal void Start()
        {
            if (CollectFrames == null || CollectFrames.Status == TaskStatus.RanToCompletion)
            {
                ctx.Start(true);
                TokenSource = new CancellationTokenSource();
                CollectFramesToken = TokenSource.Token;

                CollectFrames = Task.Factory.StartNew(() =>
                {
                    while (!CollectFramesToken.IsCancellationRequested)
                    {
                        OnFrameReceived(new FrameReceivedEventArgs(ctx.ReadFrame()));
                    }
                },
                CollectFramesToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            }
        }

        internal void Stop()
        {
            if (CollectFrames != null && !CollectFrames.IsCanceled)
            {
                TokenSource.Cancel();
                Task.WaitAll(CollectFrames);
                ctx.Stop();
            }
        }

        #region oni.Context delegates
        internal Action<int, int> SetCustomOption => ctx.SetCustomOption;
        internal Func<int, int> GetCustomOption => ctx.GetCustomOption;
        internal Action ResetFrameClock => ctx.ResetFrameClock;

        public bool Running
        {
            get
            {
                return ctx.Running;
            }
        }

        public int HardwareAddress
        {
            get
            {
                return ctx.HardwareAddress;
            }
            set
            {
                ctx.HardwareAddress = value;
            }
        }

        public int BlockReadSize
        {
            get
            {
                return ctx.BlockReadSize;
            }
            set
            {
                ctx.BlockReadSize = value;
            }
        }

        public int BlockWriteSize
        {
            get
            {
                return ctx.BlockWriteSize;
            }
            set
            {
                ctx.BlockWriteSize = value;
            }
        }

        public int HubState
        {
            get
            {
                return ctx.GetCustomOption((int)oni.ONIXOption.PORTFUNC);
            }
            set
            {
                ctx.SetCustomOption((int)oni.ONIXOption.PORTFUNC, value);
            }
        }


        private readonly object reg_lock = new object();

        internal uint ReadRegister(uint? dev_index, uint register_address)
        {
            lock (reg_lock)
            {
                return ctx.ReadRegister(dev_index, register_address);
            }
        }

        internal void WriteRegister(uint? dev_index, uint register_address, uint value)
        {
            lock (reg_lock)
            {
                ctx.WriteRegister(dev_index, register_address, value);
            }
        }

        private readonly object write_lock = new object();

        public void Write<T>(uint dev_idx, T data) where T : struct
        {
            lock (write_lock)
            {
                ctx.Write(dev_idx, data);
            }
        }
        public void Write<T>(uint dev_idx, T[] data) where T : struct
        {
            lock (write_lock)
            {
                ctx.Write(dev_idx, data);
            }
        }
        public void Write(uint dev_idx, IntPtr data, int data_size)
        {
            lock (write_lock)
            {
                ctx.Write(dev_idx, data, data_size);
            }
        }

        internal Func<uint, uint?> HubDataClock => ctx.HubDataClock;
        #endregion

        void OnFrameReceived(FrameReceivedEventArgs e)
        {
            FrameReceived?.Invoke(this, e);
        }

        public void Dispose()
        {
            ctx?.Dispose();

        }
    }
}
