using X11;

namespace cswm
{
    public partial class WindowManager
    {
        private void OnMapRequest(X11.XMapRequestEvent ev)
        {
            AddFrame(ev.window);
            Xlib.XMapWindow(this.display, ev.window);
        }
        
        void OnConfigureRequest(X11.XConfigureRequestEvent ev)
        {
            XWindowChanges changes = new X11.XWindowChanges
            {
                x = ev.x,
                y = ev.y,
                width = ev.width,
                height = ev.height,
                border_width = ev.border_width,
                sibling = ev.above,
                stack_mode = ev.detail
            };

            if (this.WindowIndexByClient.ContainsKey(ev.window))
            {
                // Resize the frame
                Xlib.XConfigureWindow(this.display, this.WindowIndexByClient[ev.window].frame, ev.value_mask, ref changes);
            }
            // Resize the window
            Xlib.XConfigureWindow(this.display, ev.window, ev.value_mask, ref changes);
        }
    }
}