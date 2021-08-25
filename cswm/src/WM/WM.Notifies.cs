using X11;

namespace cswm
{
    public partial class WindowManager
    {
        void OnMapNotify(X11.XMapEvent ev)
        {
            Log.Debug($"(MapNotifyEvent) Window {ev.window} has been mapped.");
        }
        
        void OnUnmapNotify(X11.XUnmapEvent ev)
        {
            if (ev.@event == this.root)
            {
                Log.Debug($"(OnUnmapNotify) Window {ev.window} has been reparented to root");
                return;
            }
            if (!this.WindowIndexByClient.ContainsKey(ev.window))
                return; // Don't unmap a window we don't own.

            RemoveFrame(ev.window);
            
            ReTile();
            
            SetKeysTrap(root);
        }
        
        void OnDestroyNotify(X11.XDestroyWindowEvent ev)
        {
            if (WindowIndexByClient.ContainsKey(ev.window))
            {
                WindowIndexByClient.Remove(ev.window);
            }
            else if (WindowIndexByFrame.ContainsKey(ev.window))
                WindowIndexByFrame.Remove(ev.window);
            Log.Debug($"(OnDestroyNotify) Destroyed {ev.window}");
        }
        
        void OnReparentNotify(X11.XReparentEvent ev)
        {
            return; // Never seems to be interesting and is often duplicated.
        }

        void OnCreateNotify(X11.XCreateWindowEvent ev)
        {
            Log.Debug($"(OnCreateNotify) Created event {ev.window}, parent {ev.parent}");
        }
        
        void OnMotionNotify(X11.XMotionEvent ev)
        {
            Log.Info($"Pointer moved on window {ev.window}");
        }

        void OnEnterNotify(X11.XCrossingEvent ev)
        {
            Window win = ev.window;
            if (WindowIndexByClient.ContainsKey(win))
            {
                Window frame = WindowIndexByClient[win].frame;
                Xlib.XSetInputFocus(display, frame, RevertFocus.RevertToParent, 0);
                Xlib.XRaiseWindow(display, frame);
                Log.Info("Changed frame color");
            }
        }
    }
}