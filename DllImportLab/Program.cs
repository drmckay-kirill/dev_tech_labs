using System.Runtime.InteropServices;
using System;

namespace DllImportLab
{
    class Program
    {
        private const string DllPath = "/home/drmckay/projects/dev_tech_labs/DllImportLab/Lib2-1.dll";

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern double TheFunc(char[] name, double x);

        static void Main(string[] args)
        {
            var value = TheFunc("Ponomarev".ToCharArray(), 0);
        }
    }
}
