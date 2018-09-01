using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Utilities
{
    namespace Logging
    {
        public interface ILogger<T>
        {
            Task LogAsync(T item);

            Task LogManyAsync(IEnumerable<T> items);

            Task FlushAsync();
        }
    }
}
