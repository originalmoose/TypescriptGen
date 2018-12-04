using System;

namespace TypeGen
{
    public static class Disposable
    {
        public static IDisposable Create(Action dispose)
        {
            return new ActionDisposable(dispose);
        }
    }
}