using System;
using System.Threading.Tasks;
using Utilities.Concurrency;

namespace Utilities.Logging
{
    public abstract class ThreadSafeLogger<T>
        : Logger<T>
    {
        private volatile bool _isFlushing = false;

        private AsyncSyncProvider _logSem = new AsyncSyncProvider(1, 1);

        protected ThreadSafeLogger(TimeSpan? maxLogWait, TimeSpan? sleepInterval, int maxItemsInBuffer)
            : base(maxLogWait, sleepInterval, maxItemsInBuffer)
        {
        }

        public async Task ThreadSafeFlushAsync(Func<Task> flush)
        {
            await Locking.OptimisticLock(_logSem,
                () => !_isFlushing,
                async () =>
                {
                    _isFlushing = true;
                    await flush();
                    _isFlushing = false;
                });
        }

        public async Task ThreadSafeAdd(Func<Task> add)
        {
            await Locking.PessimisticLock(_logSem, add);
        }
    }
}
