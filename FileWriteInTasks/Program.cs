using System.Text;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace FileWriteInTasks
{
    public class Program
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static async Task Main(string[] args)
        {
            var files = await GenerateFilesAsync(10, 100, 1000);
            foreach (var file in files)
                Console.WriteLine(file);

            var res = await MergeFilesAsync(files);
            var resFileInfo = new FileInfo(res);
            Console.WriteLine($"Result: {resFileInfo.FullName} {resFileInfo.Length}");
        }

        public static async Task<string> MergeFilesAsync(List<string> files)
        {
            var mergedFileName = Path.GetTempFileName();

            // simultaneously
            var tasks = files.Select(x => ReadFileAsync(x, mergedFileName));
            await Task.WhenAll(tasks);

            // consequentially
            // foreach (var filename in files)
            //     await ReadFileAsync(filename, mergedFileName);

            return mergedFileName;        
        }

        public static async Task<List<string>> GenerateFilesAsync(
            int N, 
            int rowCount,
            int stringLength)
        {
            var tasks = Enumerable.Range(1, N)
                .Select(x => WriteFileAsync(x, rowCount, stringLength));

            var files = await Task.WhenAll(tasks);
            return files.ToList();
        }

        public static async Task<string> WriteFileAsync(
            int fileIndex, 
            int rowCount,
            int stringLength)
        {
            Console.WriteLine($"{fileIndex} thread Id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Begin writing to {fileIndex} file {DateTime.Now.Minute} {DateTime.Now.Second} {DateTime.Now.Millisecond}");
            
            var fileName = $"{Path.GetTempFileName()}_{fileIndex}";
            await using var sw = new StreamWriter(fileName);
            var builder = new StringBuilder();
            for (int i = 0; i < rowCount; i++)
            {
                builder
                    .Append(fileIndex)
                    .Append(": ")
                    .Append((char)('A' + fileIndex), stringLength);
                await sw.WriteLineAsync(builder.ToString());
                builder.Clear();
            }
                
            sw.Close();
            
            Console.WriteLine($"End writing to {fileIndex} file {DateTime.Now.Minute} {DateTime.Now.Second} {DateTime.Now.Millisecond}");
            
            return fileName;
        }

        public static async Task ReadFileAsync(
            string sourceFile, 
            string destFile)
        {
            Console.WriteLine($"{sourceFile} thread Id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Begin read from {sourceFile} file {DateTime.Now.Minute} {DateTime.Now.Second} {DateTime.Now.Millisecond}");

            var lines = await File.ReadAllLinesAsync(sourceFile);
            foreach (var line in lines)
            {
                await _semaphore.WaitAsync();
                var newLine = $"{line}{Environment.NewLine}";
                await using var fs = new FileStream(destFile, FileMode.Append);
                await fs.WriteAsync(Encoding.UTF8.GetBytes(newLine));
                fs.Close();
                _semaphore.Release();
            }

            Console.WriteLine($"End read from {sourceFile} file {DateTime.Now.Minute} {DateTime.Now.Second} {DateTime.Now.Millisecond}");
        }
    }
}
