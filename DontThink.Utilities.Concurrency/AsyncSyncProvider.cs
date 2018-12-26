using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DontThink.Utilities.Concurrency
{
    public class AsyncSyncProvider
        : SemaphoreSlim,
        IAsyncSyncrhonizationProvider
    {
#if DEBUG
        private int _count = 0;
#endif

        public AsyncSyncProvider(int initialCount) : base(initialCount)
        {
        }

        public AsyncSyncProvider(int initialCount, int maxCount) : base(initialCount, maxCount)
        {
        }

        void IAsyncSyncrhonizationProvider.Release()
        {
#if DEBUG
            Interlocked.Increment(ref _count);
#endif
            base.Release();
        }

        async Task IAsyncSyncrhonizationProvider.WaitAsync() => await base.WaitAsync();
    }
}
