using System;

namespace ComDemoLibConsole
{
    class Program
    {
        private static Guid TypeGuid => Guid.Parse("46385161-4C92-4F59-9867-EA02227F9C51");

        static void Main(string[] args)
        {
            var objectType = Type.GetTypeFromCLSID(TypeGuid)
                ?? throw new ArgumentException($"Сборка с идентификатором {TypeGuid} не обнаружена");
            dynamic @object = Activator.CreateInstance(objectType);

            Console.WriteLine(@object.FuncName());
            Console.WriteLine(@object.FuncValue(1));
            Console.WriteLine(@object.FuncValue(2));
        }
    }
}
