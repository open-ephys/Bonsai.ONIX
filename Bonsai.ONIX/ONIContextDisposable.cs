using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    public sealed class ONIContextDisposable : ICancelable, IDisposable
    {
        IDisposable resource;

        public ONIContextDisposable(ONIContextTask ctx_task, IDisposable disposable)
        {
            Context = ctx_task ?? throw new ArgumentNullException("ctx_task");
            resource = disposable ?? throw new ArgumentNullException("disposable");
        }

        public ONIContextTask Context { get; private set; }

        public bool IsDisposed
        {
            get { return resource == null; }
        }

        public void Dispose()
        {
            var disposable = Interlocked.Exchange(ref resource, null);
            if (disposable != null)
            {
                // NB: Persist the context for soem time to keep UI performance
                // so that the whole PCIe stack does not have be set up and torn
                // down for every register IO
                _ = DelayDisposeAsync(disposable);

                //disposable.Dispose();
            }
        }

        private async Task DelayDisposeAsync(IDisposable disposable)
        {
            await Task.Delay(300);
            disposable.Dispose();
        }
    }
}