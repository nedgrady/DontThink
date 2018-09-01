using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    namespace Concurrency
    {
        public static class Locking
        {
            public static async Task<bool> OptimisticLock(IAsyncSyncrhonizationProvider sync, Func<bool> check, Func<Task> action)
            {
                if (check())
                {
                    await sync.WaitAsync();
                    try
                    {
                        if (check())
                            await action();
                        return true;
                    }
                    finally
                    {
                        sync.Release();
                    }
                }
                return false;
            }

            public static async Task PessimisticLock(IAsyncSyncrhonizationProvider sync, Func<Task> action)
            {
                await sync.WaitAsync();
                try
                {
                    await action();
                }
                finally
                {
                    sync.Release();
                }
            }
        }
    }
}
