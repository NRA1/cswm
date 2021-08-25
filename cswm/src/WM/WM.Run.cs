using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using X11;

namespace cswm
{
    public partial class WindowManager
    {
        
        public int Run()
        {
            IntPtr ev = Marshal.AllocHGlobal(24 * sizeof(long));

            Window ReturnedParent = 0, ReturnedRoot = 0;


            Xlib.XGrabServer(this.display); // Lock the server during initialization
            int r = Xlib.XQueryTree(this.display, this.root, ref ReturnedRoot, ref ReturnedParent,
                out List<Window> ChildWindows);

            Log.Debug($"Reparenting and framing pre-existing child windows: {ChildWindows.Count}");
            for (int i = 0; i < ChildWindows.Count; i++)
            {
                Log.Debug($"Framing child {i}, {ChildWindows[i]}");
                AddFrame(ChildWindows[i]);
            }
            Xlib.XUngrabServer(this.display); // Release the lock on the server.


            while (true)
            {
                Xlib.XNextEvent(this.display, ev);
                XAnyEvent xevent = Marshal.PtrToStructure<X11.XAnyEvent>(ev);

                switch (xevent.type)
                {
                    case (int)Event.DestroyNotify:
                        XDestroyWindowEvent destroyEvent = Marshal.PtrToStructure<X11.XDestroyWindowEvent>(ev);
                        OnDestroyNotify(destroyEvent);
                        break;
                    case (int)Event.CreateNotify:
                        XCreateWindowEvent createEvent = Marshal.PtrToStructure<X11.XCreateWindowEvent>(ev);
                        OnCreateNotify(createEvent);
                        break;
                    case (int)Event.MapNotify:
                        XMapEvent mapNotify = Marshal.PtrToStructure<X11.XMapEvent>(ev);
                        OnMapNotify(mapNotify);
                        break;
                    case (int)Event.MapRequest:
                        XMapRequestEvent mapEvent = Marshal.PtrToStructure<X11.XMapRequestEvent>(ev);
                        OnMapRequest(mapEvent);
                        break;
                    case (int)Event.ConfigureRequest:
                        XConfigureRequestEvent cfgEvent = Marshal.PtrToStructure<X11.XConfigureRequestEvent>(ev);
                        OnConfigureRequest(cfgEvent);
                        break;
                    case (int)Event.UnmapNotify:
                        XUnmapEvent unmapEvent = Marshal.PtrToStructure<X11.XUnmapEvent>(ev);
                        OnUnmapNotify(unmapEvent);
                        break;
                    case (int)Event.ReparentNotify:
                        XReparentEvent reparentEvent = Marshal.PtrToStructure<X11.XReparentEvent>(ev);
                        OnReparentNotify(reparentEvent);
                        break;
                    case (int)Event.ButtonPress:
                        XButtonEvent buttonPressEvent = Marshal.PtrToStructure<X11.XButtonEvent>(ev);
                        OnButtonPressEvent(buttonPressEvent);
                        break;
                    case (int)Event.ButtonRelease:
                        this.MouseMovement = null;
                        break;
                    case (int)Event.MotionNotify:
                        // We only want the newest motion event in order to reduce perceived lag
                        while (Xlib.XCheckMaskEvent(this.display, EventMask.Button1MotionMask, ev)) { /* skip over */ }
                        XMotionEvent motionEvent = Marshal.PtrToStructure<X11.XMotionEvent>(ev);
                        OnMotionNotify(motionEvent);
                        break;
                    case (int)Event.FocusOut:
                        XFocusChangeEvent focusOutEvent = Marshal.PtrToStructure<X11.XFocusChangeEvent>(ev);
                        OnFocusOutEvent(focusOutEvent);
                        break;
                    case (int)Event.FocusIn:
                        XFocusChangeEvent focusInEvent = Marshal.PtrToStructure<X11.XFocusChangeEvent>(ev);
                        OnFocusInEvent(focusInEvent);
                        break;
                    case (int)Event.ConfigureNotify:
                        break;
                    case (int)Event.Expose:
                        XExposeEvent exposeEvent = Marshal.PtrToStructure<X11.XExposeEvent>(ev);
                        OnExposeEvent(exposeEvent);
                        break;
                    case (int)Event.KeyPress:
                        XKeyEvent keyPressEvent = Marshal.PtrToStructure<X11.XKeyEvent>(ev);
                        OnKeyPressEvent(keyPressEvent);
                        break;
                    case (int)Event.EnterNotify:
                        XCrossingEvent enterEvent = Marshal.PtrToStructure<X11.XCrossingEvent>(ev);
                        OnEnterNotify(enterEvent);
                        break;
                    default:
                        this.Log.Debug($"Event type: { Enum.GetName(typeof(Event), xevent.type)}");
                        break;
                }
            }
            Marshal.FreeHGlobal(ev);
        }
    }
}