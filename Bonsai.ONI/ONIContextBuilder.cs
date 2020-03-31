using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuilderPackage
{
    class ONIContextBuilder : ZeroArgumentExpressionBuilder // TODO: for output streams SingleArgumentExpressionBuilder
    {
        // Public state
        [System.Xml.Serialization.XmlIgnore] // Must be recreated
        [Description("The hardware context associated with this node.")]
        [Editor("Bonsai.ONI.Design.ONIControllerEditor, Bonsai.ONI.Design", typeof(UITypeEditor))]
        public oni.Context AcqContext { get; private set; }

        [System.Xml.Serialization.XmlIgnore] // Temporary state
        public bool Connected { get; private set; }

        public string Driver { get; set; } = "riffa";
        public int Index { get; set; } = 0;
        public int BlockReadSize { get; set; } = 2048;

        // Internal state
        private Task CollectFrames;
        private CancellationTokenSource TokenSource;
        private CancellationToken CollectFramesToken;

        public void AttemptToConnect()
        {
            try
            {
                AcqContext = new oni.Context(Driver, Index);
            }
            catch (oni.ONIException)
            {
                Connected = false;
                return;
            }

            Connected = true;
        }

        public bool Running()
        {
            return CollectFrames.Status == TaskStatus.Running;
        }

        public void Start()
        {
            if (CollectFrames == null || CollectFrames.Status == TaskStatus.RanToCompletion)
            {

                AcqContext.Reset();
                AcqContext.SetBlockReadSize(BlockReadSize);

                TokenSource = new CancellationTokenSource();
                CollectFramesToken = TokenSource.Token;

                CollectFrames = Task.Factory.StartNew(() =>
                {
                    while (!CollectFramesToken.IsCancellationRequested)
                    {
                        OnFrameReceived(new FrameReceivedEventArgs(AcqContext.ReadFrame()));
                    }
                },
                CollectFramesToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            }
        }

        public void Stop()
        {
            if (CollectFrames != null && !CollectFrames.IsCanceled)
            {
                // NB: Unfortunately, in order to completely stop the hardware, I need to destroy the context.
                // This is obviously not ideal and I'm not actually sure why this is the case.

                TokenSource.Cancel();
                //Task.WaitAll(CollectFrames, 200); //, (int)100); // Wait for theads to exit, useful when I implement writer
                CollectFrames.Wait(200);
                AcqContext.Stop();
                AcqContext.Destroy();
            }
        }

    }
}
