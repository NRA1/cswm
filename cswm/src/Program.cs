using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using X11;

namespace cswm
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowManager wm = new WindowManager(Logger.LogLevel.Debug);
            wm.Run();
        }
    }
}