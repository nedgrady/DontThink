using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Debugging;
using System.Linq;
using Utilities.IO;
#if DEBUG
using System.Diagnostics;
#endif
namespace Utilities.Logging
{
    /// <summary>
    /// Generic base class of loggers.
    /// Maintains an in-memory buffer of any items and periodically flushes these items.
    /// We have this class to abstract the logic of spawning the logging thread and sleeping/flushing where appropriate
    /// from any concrete implementations.
    /// </summary>
    /// <typeparam name="T">The type of object to log, probably only ever going to be a string in reality...</typeparam>
    public abstract class Logger<T>
            : ILogger<T>,
            IDisposable
    {
        protected readonly TimeSpan _maxLogWait;
        protected readonly TimeSpan _sleepInterval;
        protected readonly int _maxItemsInBuffer;
        protected readonly LogLevel _minLogLevel;

        protected readonly List<T> _buffer;

        const string FALLBACK_FILE = "LogDump.txt";

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
            int maxItemsInBuffer = 20,
            LogLevel minLogLevel = LogLevel.Error)
        {
            _maxLogWait = maxLogWait ?? new TimeSpan(0, 0, 30);
            _sleepInterval = sleepInterval ?? new TimeSpan(0, 0, 5);
            _maxItemsInBuffer = maxItemsInBuffer;
            _minLogLevel = minLogLevel;
             _buffer = new List<T>(_maxItemsInBuffer);
            Thread workerThread = new Thread(async () => { await LoggerWorker(); });
            workerThread.Start();
        }

        private async Task LoggerWorker()
        {
            await LogStart();
            while (true)
            {
                if (Buffer.Count() > _maxItemsInBuffer ||
                    _lastFlush + _maxLogWait < DateTime.Now)
                {
#if DEBUG
                    Console.WriteLine($"{DateTime.Now} : flushing {_buffer.Count} items");
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

        protected async Task DefaultLog(Func<Task> logFunc, LogLevel logLevel)
        {
            if (logFunc == null)
            {
#if DEBUG
                throw new ArgumentNullException("logFunc");
#pragma warning disable CS0162 // Unreachable code detected
#endif

                LogLevel level = logLevel;
                await FileUtilities.WriteToCurrentDirectory($"{LogLevel.Critical}\t{DateTime.Now}\tnull logFunc at {Debugging.Debugging.CurrentCodeInfo()}");
                return;
#if DEBUG
#pragma warning restore CS0162 // Unreachable code detected
#endif
            }

            if ((logLevel) >= _minLogLevel)
                await logFunc();
        }

        /// <summary>
        /// Provides the base logger class access to its concrete implementation's in memory buffer.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<T> Buffer { get; }

        ///<summary>
        /// Method called as soon as the logging thread spins up
        /// Optionally override this in any child classes to perform initial actions on the logging thread.
        ///</summary>
        protected async virtual Task LogStart()
        {
#if DEBUG
            Console.WriteLine($"Started Logging at {DateTime.Now} PID: {Process.GetCurrentProcess().Id} Managed Thead ID: {Thread.CurrentThread.ManagedThreadId}");
#endif
        }

        #region ILogger
        public abstract Task FlushAsync();
        public abstract Task LogAsync(T item, LogLevel logLevel);
        public abstract Task LogManyAsync(IEnumerable<T> items, LogLevel logLevel);
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected async virtual Task Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    await FlushAsync();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Logger() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public async void Dispose()
        {
#if DEBUG
            Console.WriteLine("Disposing");
#endif
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            await Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        ~Logger()
        {
            Dispose();
        }
    }
}
