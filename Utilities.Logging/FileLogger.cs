using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Utilities.Logging
{
    public class FileLogger
        : ThreadSafeLogger<string>
    {

        private readonly string _filePath;
        private readonly object _fileLock = new object();

        private readonly List<string> _strings = new List<string>();

        private readonly Func<string, LogLevel, string> _transformFunc;

        protected override IEnumerable<string> Buffer => _strings;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="maxLogWait"></param>
        /// <param name="sleepInterval"></param>
        /// <param name="maxItemsInBuffer"></param>
        /// <param name="transformFunc"></param>
        public FileLogger(
            string filePath,
            TimeSpan? maxLogWait = null,
            TimeSpan? sleepInterval = null,
            int maxItemsInBuffer = 20,
            Func<string, LogLevel, string> transformFunc = null,
            LogLevel minLogLevel = LogLevel.Error)
            : base(maxLogWait, sleepInterval, maxItemsInBuffer, minLogLevel)
        {
            _filePath = filePath ?? throw new ArgumentNullException("filePath", "filePath parameter null when initializing a Logger object.");

            _transformFunc = transformFunc ?? DefaultTransformFunction;

            //TODO - find a better way of checking if the file is valid...
#pragma warning disable CS0642 // Possible mistaken empty statement
            //We're trying to open the file here so that the Logger throws an exception as soon as it is initialized, rather than when the first log entry is made.
            using (var f = File.Open(filePath, FileMode.OpenOrCreate)) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
        }

        public async override Task FlushAsync() => await ThreadSafeFlushAsync(WriteBufferToFile);

        public async override Task LogAsync(string str, LogLevel logLevel)
        {
#if DEBUG
            Console.WriteLine($"Logging: {str}");
#endif
            await ThreadSafeLog(async () => _strings.Add(_transformFunc?.Invoke(str, logLevel) ?? str), logLevel);

            if (logLevel >= LogLevel.Error)
                await FlushAsync();
            else
                await MaybeFlush();
        }

        public async override Task LogManyAsync(IEnumerable<string> strings, LogLevel logLevel)
        {
            await ThreadSafeLog(async () => _strings.AddRange(strings.Select(s => _transformFunc?.Invoke(s, logLevel) ?? s)), logLevel);
            await MaybeFlush();
        }

        protected async override Task LogStart()
        {
            await LogAsync($"Initialised Logging with Max Log Wait: {_maxLogWait}, Sleep Interval: {_sleepInterval}, Max Buffer Size: {_maxItemsInBuffer}", LogLevel.Information);
            await base.LogStart();
        }

        private async Task WriteBufferToFile()
        {
            using (StreamWriter writer = new StreamWriter(path: _filePath, append: true))
            {
                foreach (string logEntry in _strings)
                {
                    await writer.WriteLineAsync(logEntry);
                }
            }
            _strings.Clear();
        }

        private async Task MaybeFlush()
        {
            if (_strings.Count >= _maxItemsInBuffer)
                await FlushAsync();
        }

        private static string DefaultTransformFunction(string logEntry, LogLevel level)
        {
            return $"{level.ToUserFriendlyName()}\t{DateTime.Now}\t{logEntry}";
        }
    }

}
