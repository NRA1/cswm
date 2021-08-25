using X11;

namespace cswm
{
    public partial class WindowManager
    {
        void ReTile()
        {
            int winCount = windowGroups.Count;
            int slaveCount = winCount - 1;
            Xlib.XGetWindowAttributes(display, root, out XWindowAttributes rootAttr);

            if (winCount == 1)
            {
                Xlib.XGetWindowAttributes(display, windowGroups[0].frame, out XWindowAttributes frameAttr);
                Xlib.XMoveResizeWindow(display, windowGroups[0].frame, 0, 0,
                    (uint) (rootAttr.width - (frameAttr.border_width * 2)),
                    (uint) (rootAttr.height - (frameAttr.border_width * 2)));
                Xlib.XMoveResizeWindow(display, windowGroups[0].child, 0, 0,
                    (uint) (rootAttr.width - (frameAttr.border_width * 2)),
                    (uint) (rootAttr.height - (frameAttr.border_width * 2)));
            }
            else if (winCount > 1)
            {
               for (int i = 0; i < winCount; i++)
               {
                   Xlib.XGetWindowAttributes(display, windowGroups[i].frame, out XWindowAttributes frameAttr);
                   int borders = frameAttr.border_width * 2;
                   if (i == 0)
                   {
                       Xlib.XMoveResizeWindow(display, windowGroups[0].frame, 0, 0, (uint)((rootAttr.width / 2) - borders),
                           (uint)(rootAttr.height - borders));
                       Xlib.XMoveResizeWindow(display, windowGroups[0].child, 0, 0, (uint)((rootAttr.width / 2) - borders),
                           (uint)(rootAttr.height - borders));
                   }
                   else
                   {
                       Xlib.XMoveResizeWindow(display, windowGroups[i].frame, (int) (rootAttr.width / 2),
                           (int) (rootAttr.height / slaveCount) * (i - 1), (uint)((rootAttr.width / 2) - borders),
                           (uint) ((rootAttr.height / slaveCount) - borders));
                       Xlib.XMoveResizeWindow(display, windowGroups[i].child, 0, 0, (uint)((rootAttr.width / 2) - borders),
                           (uint) ((rootAttr.height / slaveCount) - borders));
                   }
               }               
            }

        }
    }
}