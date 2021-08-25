using X11;

namespace cswm
{
    public partial class WindowManager
    {
        
        private void SetKeysTrap(Window child)
        {
            Xlib.XGrabKey(this.display, Xlib.XKeysymToKeycode(display, (X11.KeySym) CswmKeySym.XK_Return),
                KeyButtonMask.Mod1Mask, child, false, GrabMode.Async, GrabMode.Async);
            
            Xlib.XGrabKey(this.display, Xlib.XKeysymToKeycode(display, (X11.KeySym) CswmKeySym.XK_space),
                KeyButtonMask.Mod1Mask, child, false, GrabMode.Async, GrabMode.Async);
            
            Xlib.XGrabKey(this.display, Xlib.XKeysymToKeycode(display, (X11.KeySym) CswmKeySym.XK_BackSpace),
                KeyButtonMask.Mod1Mask, child, false, GrabMode.Async, GrabMode.Async);
        }
    }
}