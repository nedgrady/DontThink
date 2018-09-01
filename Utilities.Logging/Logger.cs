using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
#if DEBUG
using System.Diagnostics;
#endif
namespace Utilities
{
    namespace Logging
    {
        /// <summary>
        /// Generic base class of loggers.
        /// Maintains an in-memory buffer of any items and periodically flushes these items.
        /// We have this class to abstract the logic of spawning the logging thread and sleeping/flushing where appropriate
        /// from any concrete implementations.
        /// </summary>
        /// <typeparam name="T">The type of object to log, probably only ever going to be a string in reality...</typeparam>
        public abstract class Logger<T>
             : ILogger<T>
        {
            protected readonly TimeSpan _maxLogWait;
            protected readonly TimeSpan _sleepInterval;
            protected readonly int _maxItemsInBuffer;

            private DateTime _lastFlush = DateTime.Now;
            /// <summary>
            /// Initializes an instance of the <cref ref="Logger">Logger</cref>
            /// </summary>
            /// <param name="maxLogWait">The maximum time the logging thread will wait before flushing the logs in memory. Defaults to 30 seconds if ommitted.</param>
            /// <param name="sleepInterval">The interval of time that the logger thread will sleep between polling the count of the in memory logs and taking according action. Defaults to 5 seconds if ommitted.</param>
            /// <param name="maxItemsInBuffer">The maximum number of items to be kept in memory before being flushed. Defaults to 20 if ommitted. </param>
            public Logger(
                TimeSpan? maxLogWait = null,
                TimeSpan? sleepInterval = null,
                int maxItemsInBuffer = 20)
            {
                _maxLogWait = maxLogWait ?? new TimeSpan(0, 0, 30);
                _sleepInterval = sleepInterval ?? new TimeSpan(0, 0, 5);
                _maxItemsInBuffer = maxItemsInBuffer;

                Thread workerThread = new Thread(async () => { await LoggerWorker(); });
                workerThread.Start();
            }

            private async Task LoggerWorker()
            {
                await LogStart();
                while (true)
                {
                    if (GetCollection().Count() > _maxItemsInBuffer ||
                        _lastFlush + _maxLogWait < DateTime.Now)
                    {
#if DEBUG
                        Console.WriteLine($"{DateTime.Now} : flushing {GetCollection().Count()} items");
#endif
                        await FlushAsync();
                        _lastFlush = DateTime.Now;
                    }
#if DEBUG
                    Console.WriteLine($"{DateTime.Now} sleeping for {_sleepInterval.TotalMilliseconds}");
#endif
                    await Task.Delay(_sleepInterval);
                }
            }


            /// <summary>
            /// Provides the base logger class access to its concrete implementation's in memory buffer.
            /// </summary>
            /// <returns></returns>
            protected abstract IEnumerable<T> GetCollection();

            ///<summary>
            /// Method called as soon as the logging thread spins up
            /// Optionally override this in any child classes to perform initial actions on the logging thread.
            ///</summary>
            protected virtual Task LogStart()
            {
#if DEBUG
                Console.WriteLine($"Started Logging at {DateTime.Now} PID: {Process.GetCurrentProcess().Id} Managed Thead ID: {Thread.CurrentThread.ManagedThreadId}");
#endif
                return Task.CompletedTask;
            }

            #region ILogger
            public abstract Task FlushAsync();
            public abstract Task LogAsync(T item);
            public abstract Task LogManyAsync(IEnumerable<T> items);
            #endregion
        }
    }
}
