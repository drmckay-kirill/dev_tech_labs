using System.Runtime.InteropServices;
using System;

namespace DllImportLab
{
    class Program
    {
        private const double Step = 0.5;
        private const double Low = -2;
        private const double High = 2;
        private const string DllPath = "Lib2-1.dll";
        private const string PlotName = "Plot.png";

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern double TheFunc(string name, double x);

        private static double MakeDllCall(double x)
        {
            return TheFunc("Ponomarev", x);
        }

        private static double MyFunction(double x)
        {
            return 6.8 * x * x - 9.1 * x + 3.1;
        }

        static void Main(string[] args)
        {
            var yData = new List<double>();
            var xData = new List<double>();

            var (xmin, ymin) = (1000.0, 1000.0);
            
            for (var x = Low; x <= High; x += Step)
            {
                var res = MakeDllCall(x);
                var myResult = MyFunction(x);
                Console.WriteLine($"{x} {res} {myResult}");
                xData.Add(x);
                yData.Add(res);

                if (res < ymin)
                {
                    ymin = res;
                    xmin = x;
                }
            }

            //Console.WriteLine($"VERTEX {xmin} {ymin}");

            var plt = new ScottPlot.Plot(500, 500);
            plt.PlotScatter(xData.ToArray(), yData.ToArray());
            plt.SaveFig(PlotName);
        }
    }
}
