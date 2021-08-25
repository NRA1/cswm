using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using X11;

namespace cswm
{
    public partial class WindowManager
    {
        private Logger Log;
        private IntPtr display;
        private Window root;
        private WMCursors Cursors = new WMCursors();
        private WMColours Colours = new WMColours();
        private readonly Dictionary<Window, WindowGroup> WindowIndexByClient = new Dictionary<Window, WindowGroup>();
        private readonly Dictionary<Window, WindowGroup> WindowIndexByFrame = new Dictionary<Window, WindowGroup>();
        private List<WindowGroup> windowGroups = new List<WindowGroup>();
        private MouseMovement MouseMovement;

        public XErrorHandlerDelegate OnError;

        public WindowManager(Logger.LogLevel level)
        {
            this.Log = new Logger(level);
            IntPtr pDisplayText = Xlib.XDisplayName(null);
            string? DisplayText = Marshal.PtrToStringAnsi(pDisplayText);
            if (DisplayText == String.Empty)
            {
                Log.Error("No display configured for X11; check the value of the DISPLAY variable is set correctly");
                Environment.Exit(1);
            }

            Log.Info($"Connecting to X11 Display {DisplayText}");
            this.display = Xlib.XOpenDisplay(null);

            if (display == IntPtr.Zero)
            {
                Log.Error("Unable to open the default X display");
                Environment.Exit(1);
            }

            this.root = Xlib.XDefaultRootWindow(display);
            OnError = this.ErrorHandler;

            // Grab keypresses from root
            SetKeysTrap(this.root);
            
            // Status stat = Xlib.XGrabKey(this.display, Xlib.XKeysymToKeycode(display, (X11.KeySym) CswmKeySym.XK_Return),
            //     KeyButtonMask.Mod1Mask, root, true, GrabMode.Async, GrabMode.Async);
            
            Xlib.XSetErrorHandler(OnError);
            // This will trigger a bad access error if another window manager is already running
            Xlib.XSelectInput(this.display, this.root,
                EventMask.SubstructureRedirectMask | EventMask.SubstructureNotifyMask | EventMask.EnterWindowMask);

            Xlib.XSync(this.display, false);

            // Setup cursors
            this.Cursors.DefaultCursor = Xlib.XCreateFontCursor(this.display, FontCursor.XC_left_ptr);
            this.Cursors.TitleCursor = Xlib.XCreateFontCursor(this.display, FontCursor.XC_fleur);
            this.Cursors.FrameCursor = Xlib.XCreateFontCursor(this.display, FontCursor.XC_sizing);
            Xlib.XDefineCursor(this.display, this.root, this.Cursors.DefaultCursor);

            // Setup colours
            this.Colours.DesktopBackground = GetPixelByName("black");
            this.Colours.WindowBackground = GetPixelByName("white");
            this.Colours.InactiveTitleBorder = GetPixelByName("light slate grey");
            this.Colours.InactiveTitleColor = GetPixelByName("slate grey");
            this.Colours.InactiveFrameColor = GetPixelByName("dark slate grey");
            this.Colours.ActiveFrameColor = GetPixelByName("dark goldenrod");
            this.Colours.ActiveTitleColor = GetPixelByName("gold");
            this.Colours.ActiveTitleBorder = GetPixelByName("saddle brown");

            Xlib.XSetWindowBackground(this.display, this.root, this.Colours.DesktopBackground);
            Xlib.XClearWindow(this.display, this.root); // force a redraw with the new background color
        }

    }
}