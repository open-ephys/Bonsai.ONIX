using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.ONIX
{
    public sealed class ONIContextDisposable : ICancelable, IDisposable
    {
        IDisposable resource;
        object lockObject;

        public ONIContextDisposable(ONIContextTask contextTask, IDisposable disposable, object ctx_lock)
        {
            Context = contextTask ?? throw new ArgumentNullException(nameof(contextTask));
            resource = disposable ?? throw new ArgumentNullException(nameof(disposable));
            lockObject = ctx_lock ?? throw new ArgumentNullException(nameof(ctx_lock));
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
                //disposable.Dispose();
                // NB: Persist the context for some time to keep UI performance
                // so that the whole hardware stack does not have be set up and torn
                // down for every register IO
                _ = DelayDisposeAsync(disposable, lockObject);
            }
        }

        private async Task DelayDisposeAsync(IDisposable disposable, object ctx_lock)
        {
            // TODO: This may be causing big issues when the workflow is restarted and somehow prevents Context.Stop() from being called because this is part of the Dispose() procedure.
            await Task.Delay(100);
            lock (ctx_lock)
            {
                disposable.Dispose();
            }
        }
    }
}