using System;
using X11;

namespace cswm
{
    public partial class WindowManager
    {
        private void AddFrame(Window child)
        {
            const int frame_width = 3;
            const int inner_border = 1;

            if (this.WindowIndexByClient.ContainsKey(child))
                return; // Window has already been framed.

            string Name = String.Empty;
            Xlib.XFetchName(this.display, child, ref Name);
            Log.Debug($"Framing {Name}");

            Xlib.XGetWindowAttributes(this.display, child, out XWindowAttributes attr);
            
            // int adjusted_x_loc = WindowIndexByClient.Count * 300;
            // int adjusted_y_loc = WindowIndexByClient.Count * 300;
            
            Window frame = Xlib.XCreateSimpleWindow(this.display, this.root, 0, 0, 2, 2, 
                3, this.Colours.InactiveFrameColor, this.Colours.WindowBackground);
            

            Xlib.XSelectInput(this.display, frame, EventMask.FocusChangeMask | 
                                                   EventMask.SubstructureRedirectMask | EventMask.SubstructureNotifyMask);
            Xlib.XSelectInput(this.display, child, EventMask.EnterWindowMask);

            
            Xlib.XDefineCursor(this.display, frame, this.Cursors.FrameCursor);
            
            WindowGroup wg = new WindowGroup { child = child, frame = frame };
            this.WindowIndexByClient[child] = wg;
            this.WindowIndexByFrame[frame] = wg;
            windowGroups.Add(wg);

            ReTile();
            
            Xlib.XReparentWindow(this.display, child, frame, 0, 0); 
            Xlib.XMapWindow(this.display, frame);
            // Ensure the child window survives the untimely death of the window manager.
            Xlib.XAddToSaveSet(this.display, child);

            // Grab window manager keybindings
            SetKeysTrap(frame);


        }

        private void RemoveFrame(Window child)
        {

            if (!this.WindowIndexByClient.ContainsKey(child))
            {
                return; // Do not attempt to unframe a window we have not framed.
            }
            Window frame = WindowIndexByClient[child].frame;

            Xlib.XUnmapWindow(this.display, frame);
            Xlib.XDestroyWindow(this.display, frame);
            
            
            windowGroups.Remove(WindowIndexByClient[child]);
            this.WindowIndexByClient.Remove(child); // Cease tracking the window/frame pair.
        }
    }
}