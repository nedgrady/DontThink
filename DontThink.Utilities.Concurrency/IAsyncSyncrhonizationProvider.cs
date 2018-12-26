using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontThink.Utilities.Concurrency
{
    public interface IAsyncSyncrhonizationProvider
    {
        Task WaitAsync();

        void Release();
    }
}
