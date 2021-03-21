using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Bonsai.ONIX
{
    public class ONIContextTask : IDisposable
    {
        readonly oni.Context ctx;

        Task CollectFrames;
        Task ReadFrames;

        /// <summary>
        /// Maximum amount of frames the reading queue will hold.
        /// If the queue fills, frame reading will throttle, filling host memory instead of 
        /// userspace memory.
        /// </summary>
        private const Int32 MaxQueuedFrames = 2000000;
        /// <summary>
        /// Timeout in ms for queue reads. This should not be critical as the 
        /// read operation will cancel if the task is stopped
        /// </summary>
        private const int QueueTimeout = 200;
        BlockingCollection<oni.Frame> FrameQueue;

        // TODO: Multi-writer, thread safe FIFO for oni_write()'s
        // private Task WriteData;
        // System.Collections.Concurrent.BlockingCollection<oni.Frame> write_queue = new System.Collections.Concurrent.BlockingCollection<oni.Frame>();

        CancellationTokenSource TokenSource;
        CancellationToken CollectFramesToken;

        internal event EventHandler<FrameReceivedEventArgs> FrameReceived;

        public readonly uint SystemClockHz;
        public readonly uint AcquisitionClockHz;
        public readonly uint MaxReadFrameSize;
        public readonly uint MaxWriteFrameSize;
        public readonly Dictionary<uint, oni.Device> DeviceTable;

        public static readonly string DefaultDriver = "riffa";
        public static readonly int DefaultIndex = 0;

        private readonly object hw_lock = new object();

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
            ctx.Start(true);
            TokenSource = new CancellationTokenSource();
            CollectFramesToken = TokenSource.Token;

            FrameQueue = new BlockingCollection<oni.Frame>(MaxQueuedFrames);

            ReadFrames = Task.Factory.StartNew(() =>
            {
                while (!CollectFramesToken.IsCancellationRequested)
                {
                    oni.Frame frame = ReadFrame();
                    //This should not be needed since we are calling Dispose()
                    //But somehow it seems to improve performance (coupled with GC.RemovePressure)
                    //More investigation might be needed
                    GC.AddMemoryPressure(frame.DataSize);
                    try
                    {
                        FrameQueue.Add(frame, CollectFramesToken);
                    }
                    catch (OperationCanceledException) 
                    {
                        DisposeFrame(frame);
                    };
                }
            },
            CollectFramesToken,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

            CollectFrames = Task.Factory.StartNew(() =>
            {
                while (!CollectFramesToken.IsCancellationRequested)
                {
                    oni.Frame frame;

                    try
                    {
                        FrameQueue.TryTake(out frame, QueueTimeout, CollectFramesToken);
                        OnFrameReceived(new FrameReceivedEventArgs(frame));
                    }
                    catch (OperationCanceledException)
                    {
                        //If the thread stops no frame has been collected
                    }
                }
            },
            CollectFramesToken,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }

        internal void Stop()
        {
            if ((CollectFrames != null || ReadFrames != null) && !CollectFrames.IsCanceled)
            {
                TokenSource.Cancel();
                Task.WaitAll(new Task[] { CollectFrames, ReadFrames });
            }
            TokenSource?.Dispose();
            TokenSource = null;
            //clear queue and free memory
            while (FrameQueue?.Count > 0)
            {
                oni.Frame frame;
                frame = FrameQueue.Take();
                DisposeFrame(frame);
            }
            FrameQueue?.Dispose();
            FrameQueue = null;
            ctx.Stop();
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

        internal uint ReadRegister(uint dev_index, uint register_address)
        {
            lock (hw_lock)
            {
                return ctx.ReadRegister(dev_index, register_address);
            }
        }

        internal void WriteRegister(uint dev_index, uint register_address, uint value)
        {
            lock (hw_lock)
            {
                ctx.WriteRegister(dev_index, register_address, value);
            }
        }

        public oni.Frame ReadFrame()
        {
            lock (hw_lock)
            {
                return ctx.ReadFrame();
            }
        }

        public void Write<T>(uint dev_idx, T data) where T : unmanaged
        {
            lock (hw_lock)
            {
                ctx.Write(dev_idx, data);
            }
        }
        public void Write<T>(uint dev_idx, T[] data) where T : unmanaged
        {
            lock (hw_lock)
            {
                ctx.Write(dev_idx, data);
            }
        }
        public void Write(uint dev_idx, IntPtr data, int data_size)
        {
            lock (hw_lock)
            {
                ctx.Write(dev_idx, data, data_size);
            }
        }

        public Func<uint, oni.Hub> GetHub => ctx.GetHub;
        #endregion

        void OnFrameReceived(FrameReceivedEventArgs e)
        {
            FrameReceived?.Invoke(this, e);
            DisposeFrame(e.Frame);
        }

        void DisposeFrame(oni.Frame frame)
        {
            long dataSize = frame.DataSize;
            frame.Dispose();
            GC.RemoveMemoryPressure(dataSize);
        }

        public void Dispose()
        {
            Stop();
            lock (hw_lock)
            {
                ctx?.Dispose();
            }
        }
    }
}
