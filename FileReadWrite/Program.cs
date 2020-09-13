using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
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

            // Cache vs *step-by-step* writing vs blocks
            //BenchmarkRunner.Run<WriteToFile_MethodVersion>();

            // *Sync* vs async
            //BenchmarkRunner.Run<WriteToFile_AsyncVersion>();

            // *String builder* (with clear) vs string interpolation
            //BenchmarkRunner.Run<WriteToFile_InterpolationVersion>();

            // *Stream writer* vs File stream
            //BenchmarkRunner.Run<WriteToFile_FileStreamVersion>();

            // periodic Flush ~
            //BenchmarkRunner.Run<WriteToFile_FlushVersion>();

            // final: choose block size
            //BenchmarkRunner.Run<WriteToFile_BlockVersion>();

            #endregion

            #region ReadFromFile
            
            BenchmarkRunner.Run<ReadFromFile_MethodsVersion>();

            #endregion
        }

        public class ReadFromFile_MethodsVersion
        {
            public string FilePath => "/tmp/tmptofWVk.tmp";

            public int SearchIndex => 100000 - 2;

            [Benchmark(Baseline=true)]
            public string SimpleForeach()
            {
                var lines = File.ReadLines(FilePath);
                var index = 0;
                foreach(var line in lines)
                {
                    if (index == SearchIndex)
                        return line;
                    index++;
                }
                return "";
            }

            [Benchmark]
            public string SimpleReadAll()
            {
                var lines = File.ReadAllLines(FilePath);
                return lines[SearchIndex];
            }

            [Benchmark]
            public string StreamReaderForeach()
            {
                using var sr = new StreamReader(FilePath);
                var index = 0;
                var line = "";
                while((line = sr.ReadLine()) != null)
                {
                    if (index == SearchIndex)
                        return line;
                    index++;
                }
                sr.Close();
                return "";
            }
        }

        public class WriteToFile_BlockVersion
        {
            public int LinesCount => 100000;

            [Benchmark(Baseline=true)]
            public void WriteToFile()
            {
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();

                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                {                
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    sw.WriteLine(sb.ToString());
                    sb.Clear();
                }
                   
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            [Arguments(2000)]
            [Arguments(5000)]
            [Arguments(10000)]
            public void WriteToFile_Blocks(int blockSize)
            {
                var blocksNumber = LinesCount / blockSize;
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();
                using var sw = File.AppendText(filePath);

                for(int block = 0; block < blocksNumber; block++)
                {
                    for (int i = 0; i < blockSize; i++)                    
                        sb
                            .Append(Guid.NewGuid().ToString())
                            .Append(";")
                            .Append(Guid.NewGuid().ToString())
                            .Append(Environment.NewLine);
                    
                    sw.Write(sb.ToString());
                    sb.Clear();               
                }
                
                sw.Close();

                PrintFileInfoAndDelete(filePath);
            }
        }

        public class WriteToFile_FlushVersion
        {
            [Params(100000)]
            public int LinesCount { get; set; }

            [Benchmark(Baseline=true)]
            public void WriteToFile()
            {
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();

                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                {                
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    sw.WriteLine(sb.ToString());
                    sb.Clear();
                }
                   
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public void WriteToFile_Flush()
            {
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();

                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                {                
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    sw.WriteLine(sb.ToString());
                    sb.Clear();

                    if (i % 100 == 0)
                        sw.Flush();
                }
                   
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }    
        }

        public class WriteToFile_FileStreamVersion
        {
            [Params(100000)]
            public int LinesCount { get; set; }

            [Benchmark(Baseline=true)]
            public void WriteToFile()
            {
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();

                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                {                
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    sw.WriteLine(sb.ToString());
                    sb.Clear();
                }
                   
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public void WriteToFile_FileStream()
            {
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();

                using var fs = new FileStream(filePath, FileMode.Append);

                for (int i = 0; i < LinesCount; i++)
                {                
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    fs.Write(Encoding.UTF8.GetBytes(sb.ToString()));
                    sb.Clear();
                }
                   
                fs.Close();
                    
                PrintFileInfoAndDelete(filePath);
            } 
        }

        public class WriteToFile_InterpolationVersion
        {
            [Params(100000)]
            public int LinesCount { get; set; }

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

            [Benchmark]
            public void WriteToFile_StringBuilder()
            {
                var filePath = Path.GetTempFileName();
                
                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                {
                    var sb = new StringBuilder();
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    sw.WriteLine(sb.ToString());
                }
                   
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public void WriteToFile_StringBuilder_Clear()
            {
                var filePath = Path.GetTempFileName();
                var sb = new StringBuilder();

                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                {                
                    sb
                        .Append(Guid.NewGuid().ToString())
                        .Append(";")
                        .Append(Guid.NewGuid().ToString());
                    sw.WriteLine(sb.ToString());
                    sb.Clear();
                }
                   
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }             
        }

        public class WriteToFile_AsyncVersion
        {
            [Params(100000)]
            public int LinesCount { get; set; }
            
            public int BlockSize => 1000;
            
            [Benchmark(Baseline=true)]
            public void WriteToFile_Sync()
            {
                var filePath = Path.GetTempFileName();
                
                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                    sw.Write($"{Guid.NewGuid()};{Guid.NewGuid()}{Environment.NewLine}");
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public async Task WriteToFile_Async()
            {
                var filePath = Path.GetTempFileName();
                
                using var sw = File.AppendText(filePath);
                for (int i = 0; i < LinesCount; i++)
                    await sw.WriteAsync($"{Guid.NewGuid()};{Guid.NewGuid()}{Environment.NewLine}");
                sw.Close();
                    
                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public void WriteToFile_Blocks()
            {
                var blocksNumber = LinesCount / BlockSize;
                var filePath = Path.GetTempFileName();
                using var sw = File.AppendText(filePath);

                for(int block = 0; block < blocksNumber; block++)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < BlockSize; i++)
                        sb.Append($"{Guid.NewGuid()};{Guid.NewGuid()}{Environment.NewLine}");
                    sw.Write(sb.ToString());                
                }
                
                sw.Close();

                PrintFileInfoAndDelete(filePath);
            }

            [Benchmark]
            public async Task WriteToFile_BlocksAsync()
            {
                var blocksNumber = LinesCount / BlockSize;
                var filePath = Path.GetTempFileName();
                using var sw = File.AppendText(filePath);

                for(int block = 0; block < blocksNumber; block++)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < BlockSize; i++)
                        sb.Append($"{Guid.NewGuid()};{Guid.NewGuid()}{Environment.NewLine}");
                    await sw.WriteAsync(sb.ToString());                
                }
                
                sw.Close();

                PrintFileInfoAndDelete(filePath);
            }
        }

        public class WriteToFile_MethodVersion
        {
            [Params(100000, 1000000)]
            public int LinesCount { get; set; }

            public int BlockSize => 1000;

            [Benchmark]
            public void WriteToFile_StringBuilder()
            {
                var sb = new StringBuilder();
                for (int i = 0; i < LinesCount; i++)
                    sb.Append($"{Guid.NewGuid()};{Guid.NewGuid()}{Environment.NewLine}");

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

            [Benchmark]
            public void WriteToFile_Blocks()
            {
                var blocksNumber = LinesCount / BlockSize;
                var filePath = Path.GetTempFileName();
                using var sw = File.AppendText(filePath);

                for(int block = 0; block < blocksNumber; block++)
                {
                    var sb = new StringBuilder();
                    for (int i = 0; i < BlockSize; i++)
                        sb.Append($"{Guid.NewGuid()};{Guid.NewGuid()}{Environment.NewLine}");
                    //File.AppendAllText(filePath, sb.ToString());
                    sw.Write(sb.ToString());                
                }
                
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
