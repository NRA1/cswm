using System.Diagnostics;
using X11;

namespace cswm
{
    public partial class WindowManager
    {
        private void OnKeyPressEvent(XKeyEvent ev)
        {
            Log.Info($"Recieved keypress {ev.keycode}");
            if ((X11.KeyCode) ev.keycode == Xlib.XKeysymToKeycode(this.display, (X11.KeySym) CswmKeySym.XK_Return) && ev.state == (uint)KeyButtonMask.Mod1Mask)
            {
                Process.Start("alacritty");
            }
            else if ((X11.KeyCode) ev.keycode == Xlib.XKeysymToKeycode(display, (X11.KeySym) CswmKeySym.XK_space) && ev.state == (uint) KeyButtonMask.Mod1Mask)
            {
                Process.Start("rofi", " -show run");
            }
            else if ((X11.KeyCode) ev.keycode == Xlib.XKeysymToKeycode(display, (X11.KeySym) CswmKeySym.XK_BackSpace) && ev.state == (uint) KeyButtonMask.Mod1Mask)
            {
                // Log.Debug($"Killing window {ev.window}");
                //
                // Xlib.XKillClient(display, ev.window);
            }
        }
    }
}