using System;
using System.Runtime.InteropServices;

namespace ActiveXDemoLib
{
    // регистрация сборки
    // C:\Windows\Microsoft.NET\Framework\v4.0.30319
    // RegAsm.exe ActiveXDemoLib.dll
    // RegAsm.exe ActiveXDemoLib.dll /u

    [ProgId("ActiveXDemoLib")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("227C0604-65BC-4150-9B43-E52B04F6D377")]
    [ComVisible(true)]
    public class MyFunction
    {
        [ComVisible(true)]
        public string FunctionName()
        {
            return "ActiveXDemoLib.Function";
        }

        [ComVisible(true)]
        public double MyFunctionValue(double y)
        {
            return 0;
        }
    }
}
