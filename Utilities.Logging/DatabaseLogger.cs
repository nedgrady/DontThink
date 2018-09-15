using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Utilities.Logging
{
    partial class DatabaseLogger
        : ThreadSafeLogger<string>
    {
        protected DatabaseLogger(
            string connectionString,
            TimeSpan? maxLogWait = null,
            TimeSpan? sleepInterval = null,
            int maxItemsInBuffer = 20,
            Func<string, string> transformFunc = null)
            : base(maxLogWait, sleepInterval, maxItemsInBuffer)
        {
        }

        protected override IEnumerable<string> Buffer { get => throw new NotImplementedException(); }

        public override Task FlushAsync()
        {
            throw new NotImplementedException();
        }

        public override Task LogAsync(string item, LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public override Task LogManyAsync(IEnumerable<string> items, LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

    }
}
