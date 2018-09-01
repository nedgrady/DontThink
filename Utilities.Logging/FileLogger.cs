using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Utilities
{
    namespace Logging
    {
        public class FileLogger
            : ThreadSafeLogger<string>
        {

            private readonly string _filePath;
            private readonly object _fileLock = new object();

            private readonly List<string> _strings = new List<string>();

            private readonly Func<string, string> _transformFunc;


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
                Func<string, string> transformFunc = null)
                : base(maxLogWait, sleepInterval, maxItemsInBuffer)
            {
                _filePath = filePath ?? throw new ArgumentNullException("filePath", "filePath parameter null when initializing a Logger object.");
                _transformFunc = transformFunc;

                //TODO - find a better way of checking if the file is valid...
#pragma warning disable CS0642 // Possible mistaken empty statement
                //We're trying to open the file here so that the Logger throws an exception as soon as it is initialized, rather than when the first log entry is made.
                using (var f = File.Open(filePath, FileMode.OpenOrCreate)) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
            }

            public async override Task FlushAsync() => await ThreadSafeFlushAsync(WriteBufferToFile);

            public async override Task LogAsync(string str)
            {
#if DEBUG
                Console.WriteLine($"Logging: {str}");
#endif
                await ThreadSafeAdd(async () => _strings.Add(_transformFunc?.Invoke(str) ?? str));
                await MaybeFlush();
            }

            public async override Task LogManyAsync(IEnumerable<string> strings)
            {
                await ThreadSafeAdd(async () => _strings.AddRange(strings.Select(s => _transformFunc?.Invoke(s) ?? s)));
                await MaybeFlush();
            }

            protected async override Task LogStart()
            {
                await LogAsync($"Initialised Logging with Max Log Wait: {_maxLogWait}, Sleep Interval: {_sleepInterval}, Max Buffer Size: {_maxItemsInBuffer}");
                await base.LogStart();
            }

            protected override IEnumerable<string> GetCollection() => _strings;

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
        }
    }
}
