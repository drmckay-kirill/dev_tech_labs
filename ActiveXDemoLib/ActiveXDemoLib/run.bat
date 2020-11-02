dotnet build ActiveXDemoLib
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" ActiveXDemoLib/bin/Debug/ActiveXDemoLibNew.dll /codebase
dotnet build ActiveXDemoConsole
"ActiveXDemoConsole/bin/Debug/ActiveXDemoConsole.exe"
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe" ActiveXDemoLib/bin/Debug/ActiveXDemoLibNew.dll /u