using X11;

namespace cswm
{
    public partial class WindowManager
    {
        private void OnExposeEvent(X11.XExposeEvent ev)
        {
            Log.Info($"Exposed window {ev.window}");

        }
        
        // Annoyingly, this event fires when an application quits itself, resuling in some bad window errors.
        void OnFocusOutEvent(X11.XFocusChangeEvent ev)
        {
            Window frame = ev.window;
            if (Status.Failure == Xlib.XSetWindowBorder(this.display, frame, this.Colours.InactiveTitleBorder))
                return; // If the windows have been destroyed asynchronously, cut this short.
        }

        void OnFocusInEvent(X11.XFocusChangeEvent ev)
        {
            Window frame = ev.window;
            Xlib.XSetWindowBorder(this.display, frame, this.Colours.ActiveFrameColor);
            Log.Info($"Focused window {ev.window}");
        }
    }
}