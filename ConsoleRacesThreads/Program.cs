using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System;

namespace ConsoleRacesThreads
{
    public class ThreadRacer
    {
        public static Dictionary<int, string> RaceSymbols => new Dictionary<int, string>
        {
            { 1, "@" },
            { 2, "#" },
            { 3, "$" },
            { 4, "%" },
            { 5, "^" },
            { 6, "&" },
            { 7, "*" },
            { 8, "!" },
            { 9, "?" },
            { 10, ">" },
        };

        private static int SleepTime => 500;
        private static int Finish => 72;
        private static int RowOffset => 10;
        private static Mutex mutex = new Mutex();

        private readonly int _racer;

        public ThreadRacer(int racer)
        {
            _racer = racer;
        }

        public void Race()
        {
            var rnd = new Random();
            for (int position = 0; position < Finish; position++)
            {
                mutex.WaitOne();

                Console.SetCursorPosition(position, RowOffset + _racer);
                Console.Write(RaceSymbols[_racer]);

                mutex.ReleaseMutex();
                
                // var sleepThreadTime = rnd.Next(SleepTime);
                // Thread.Sleep(sleepThreadTime);
            }
        } 
    }

    public class Program
    {

        public static void Main(string[] args)
        {
            var threads = Enumerable.Range(1, ThreadRacer.RaceSymbols.Keys.Count)
                .Select(x => {
                    var racer = new ThreadRacer(x);
                    var thread = new Thread(new ThreadStart(racer.Race));
                    return thread;
                });

            foreach (var thread in threads)
                thread.Start();

            // foreach (var thread in threads)
            //     thread.Join();
        }
    }
}
