using System;
using System.Runtime.InteropServices;

namespace ActiveXDemoLibNew
{
    // регистрация сборки
    // C:\Windows\Microsoft.NET\Framework\v4.0.30319
    // RegAsm.exe ActiveXDemoLib.dll /codebase
    // RegAsm.exe ActiveXDemoLib.dll /u

    [ProgId("ActiveXDemoLibNewNew")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("39406057-8F51-47A3-A410-733D4A5BD61D")]
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
