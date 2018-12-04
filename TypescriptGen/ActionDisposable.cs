using System;
using System.Threading;

namespace TypescriptGen
{
    public sealed class ActionDisposable : IDisposable
    {
        private volatile Action _dispose;

        public ActionDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        public bool IsDisposed => _dispose == null;

        public void Dispose()
        {
            Interlocked.Exchange(ref _dispose, null)?.Invoke();
        }
    }
}