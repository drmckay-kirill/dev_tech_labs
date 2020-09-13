using System.Text;
using System.Net;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FileReadWrite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region WriteToFile

            // Cache vs *immediate writing*
            //BenchmarkRunner.Run<WriteToFile_CacheVersion>();

            // Sync vs async

            // String builder vs string interpolation

            // Stream writer vs File stream

            // File stream without using operator vs normal File stream

            // normal File stream vs periodic Flush

            // Final version vs version with chunks

            #endregion
        }

        public class WriteToFile_AsyncVersion()
        {
            
        }

        public class WriteToFile_CacheVersion
        {
            [Params(10000, 100000)]
            public int LinesCount { get; set; }

            [Benchmark]
            public void WriteToFile_ListCache()
            {
                var cache = new List<string>();
                for (int i = 0; i < LinesCount; i++)
                    cache.Add($"{Guid.NewGuid()};{Guid.NewGuid()}");

                var filePath = Path.GetTempFileName();
                File.AppendAllLines(filePath, cache);

                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public void WriteToFile_StringBuilder()
            {
                var sb = new StringBuilder();
                for (int i = 0; i < LinesCount; i++)
                    sb.Append($"{Guid.NewGuid()};{Guid.NewGuid()}");

                var filePath = Path.GetTempFileName();
                File.AppendAllText(filePath, sb.ToString());

                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark(Baseline=true)]
            public void WriteToFile()
            {
                var filePath = Path.GetTempFileName();
                
                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                    sw.WriteLine($"{Guid.NewGuid()};{Guid.NewGuid()}");
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }
        }

        public static void PrintFileInfoAndDelete(string filePath)
        {
            //var fileInfo = new FileInfo(filePath);
            //Console.WriteLine($"{fileInfo.FullName} ----- {fileInfo.Length}");
            File.Delete(filePath);
        }
    }
}
