using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontThink.Utilities.Logging
{
    abstract class ThreadSafeSaucyLogger <T>
        : ThreadSafeLogger<T>
    {
        protected ThreadSafeSaucyLogger(
            TimeSpan? maxLogWait,
            TimeSpan? sleepInterval,
            int maxItemsInBuffer,
            LogLevel minLogLevel = LogLevel.Error)
            : base(maxLogWait, sleepInterval, maxItemsInBuffer, minLogLevel)
        {
        }

        protected abstract Task LogSauce();
    }
}
