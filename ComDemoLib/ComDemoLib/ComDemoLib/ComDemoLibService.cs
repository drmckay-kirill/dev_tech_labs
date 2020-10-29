using System.Runtime.InteropServices;

namespace ComDemoLib
{
    [Guid("94D7E3E6-1E2A-4B91-9C11-CB4BFE5C4CCD"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IComDemoLibService
    {
        [DispId(1)]
        string FuncName();

        [DispId(2)]
        double FuncValue(double x);
    }

    [Guid("46385161-4C92-4F59-9867-EA02227F9C51"), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class ComDemoLibService : IComDemoLibService
    {
        public string FuncName()
        {
            return "ComDemoLibService";
        }

        public double FuncValue(double x)
        {
            return 65536;
        }
    }
}
