using System;
using Utilities.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Test
{
    class Program
    {
        static FileLogger f = new FileLogger(
            filePath: @"logs.txt",
            maxLogWait: new TimeSpan(0, 0, 1),
            maxItemsInBuffer: 1,
            transformFunc: s => ($"{DateTime.Now}:\t" + s));

        static async Task Main(string[] args)
        {
            Thread[] threads = new Thread[20];

            for(int i=0;i<threads.Length; i++)
            {
                threads[i] = new Thread(async () => await DoWork());
                threads[i].Start();
            }
            Console.ReadLine();
        }

        static void Start()
        {

        }

        static async Task DoWork()
        {
            for(int i=0;i<20;i++)
            {
                await f.LogAsync($"{i} {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(150);
            }
        }
    }
}
