using System;
using System.Reactive.Disposables;
using System.Threading;

namespace Bonsai.ONIX
{
    public sealed class ONIContextDisposable : ICancelable, IDisposable
    {
        IDisposable resource;

        public ONIContextDisposable(ONIContextTask ctx_task, IDisposable disposable)
        {

            Context = ctx_task ?? throw new ArgumentNullException("controller");
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
                disposable.Dispose();
            }
        }
    }
}