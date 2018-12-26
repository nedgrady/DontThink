using System;
using System.Threading.Tasks;
using DontThink.Utilities.Concurrency;

namespace DontThink.Utilities.Logging
{
    public abstract class ThreadSafeLogger<T>
        : Logger<T>
    {
        private volatile bool _isFlushing = false;

        private AsyncSyncProvider _logSem = new AsyncSyncProvider(1, 1);

        protected ThreadSafeLogger(TimeSpan? maxLogWait, TimeSpan? sleepInterval, int maxItemsInBuffer, LogLevel minLogLevel = LogLevel.Error)
            : base(maxLogWait, sleepInterval, maxItemsInBuffer, minLogLevel)
        {
        }

        protected async Task ThreadSafeFlushAsync(Func<Task> flush)
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

        protected async Task ThreadSafeLog(Func<Task> logFunc, LogLevel logLevel)
        {
            await DefaultLog(async () => 
            {
                await Locking.PessimisticLock(_logSem, logFunc);
            }, logLevel);
        }
    }
}
