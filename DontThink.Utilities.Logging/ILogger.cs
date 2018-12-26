using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace DontThink.Utilities.Logging
{
    public interface ILogger<T>
    {
        Task LogAsync(T item, LogLevel logLevel);

        Task LogManyAsync(IEnumerable<T> items, LogLevel logLevel);

        Task FlushAsync();
    }
}
