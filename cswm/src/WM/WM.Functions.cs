using System;
using System.Runtime.InteropServices;
using X11;

namespace cswm
{
    public partial class WindowManager
    {
        public int ErrorHandler(IntPtr display, ref XErrorEvent ev)
        {
            if (ev.error_code == 10) // BadAccess, i.e. another window manager has already claimed those privileges.
            {
                Log.Error("X11 denied access to window manager resources - another window manager is already running");
                Environment.Exit(1);
            }

            // Other runtime errors and warnings.
            IntPtr description = Marshal.AllocHGlobal(1024);
            Xlib.XGetErrorText(this.display, ev.error_code, description, 1024);
            string? desc = Marshal.PtrToStringAnsi(description);
            Log.Warn($"X11 Error: {desc}");
            Marshal.FreeHGlobal(description);
            return 0;
        }
        
        private ulong GetPixelByName(string name)
        {
            int screen = Xlib.XDefaultScreen(this.display);
            XColor color = new XColor();
            if (0 == Xlib.XParseColor(this.display, Xlib.XDefaultColormap(this.display, screen), name, ref color))
            {
                Log.Error($"Invalid Color {name}");
            }

            if (0 == Xlib.XAllocColor(this.display, Xlib.XDefaultColormap(this.display, screen), ref color))
            {
                Log.Error($"Failed to allocate color {name}");
            }

            return color.pixel;
        }
    }
}