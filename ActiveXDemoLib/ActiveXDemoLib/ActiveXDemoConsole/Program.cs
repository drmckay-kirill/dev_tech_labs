using System;

namespace ActiveXDemoConsole
{
    class Program
    {
        private static string ProgId => "ActiveXDemoLibNewNew";

        static void Main(string[] args)
        {
            var activeXLibType = Type.GetTypeFromProgID(ProgId)
                ?? throw new ArgumentException($"Не удалось загрузить ActiveX объект с ProgId {ProgId}");
            dynamic activeXObject = Activator.CreateInstance(activeXLibType);
            
            Console.WriteLine(activeXObject.FunctionName());
            Console.WriteLine(activeXObject.MyFunctionValue(1));
            Console.WriteLine(activeXObject.MyFunctionValue(2));
        }
    }
}
