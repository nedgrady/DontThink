using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Utilities
{
    namespace Logging
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

            public override Task FlushAsync()
            {
                throw new NotImplementedException();
            }

            public override Task LogAsync(string item)
            {
                throw new NotImplementedException();
            }

            public override Task LogManyAsync(IEnumerable<string> items)
            {
                throw new NotImplementedException();
            }

            protected override IEnumerable<string> GetCollection()
            {
                throw new NotImplementedException();
            }
        }
    }
}
