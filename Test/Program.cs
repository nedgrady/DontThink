using System;
using Utilities.Logging;
using System.Threading.Tasks;
using System.Threading;
using Utilities.Database;
using Utilities.Concurrency;
using Utilities.IO;
using System.Data.SqlClient;
using System.Data;
using Utilities.Debugging;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Test
{
    class Program
    {
        /// <summary>
        /// Do things here
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine(Debugging.CurrentCodeInfo());
            Console.ReadKey();
            
        }
    }
}
