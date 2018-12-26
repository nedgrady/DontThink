using System;
using DontThink.Utilities.Logging;
using System.Threading.Tasks;
using System.Threading;
using DontThink.Utilities.Database;
using DontThink.Utilities.Concurrency;
using DontThink.Utilities.IO;
using System.Data.SqlClient;
using System.Data;
using DontThink.Utilities.Debugging;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace DontThink.Test
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
            Console.WriteLine("Starting");
            Log("Log");
            Console.WriteLine("Ending");
            Console.ReadLine();
        }

        static async void Log(string s)
        {
            await Task.Delay(1000);
            Console.WriteLine(s);
        }
    }
}
