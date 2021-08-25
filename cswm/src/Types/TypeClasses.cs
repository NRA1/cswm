using X11;

namespace cswm
{
    public class WindowGroup
    {
        public Window child;
        public Window frame;
    }

    public class WMCursors
    {
        public Cursor DefaultCursor;
        public Cursor FrameCursor;
        public Cursor TitleCursor;
    }

    public class WMColours
    {
        public ulong ActiveFrameColor;
        public ulong ActiveTitleColor;
        public ulong ActiveTitleBorder;
        public ulong InactiveFrameColor;
        public ulong InactiveTitleColor;
        public ulong InactiveTitleBorder;
        public ulong DesktopBackground;
        public ulong WindowBackground;
    }

    public enum MouseMoveType
    {
        TitleDrag,
        TopLeftFrameDrag,
        TopRightFrameDrag,
        BottomLeftFrameDrag,
        BottomRightFrameDrag,
        RightFrameDrag,
        TopFrameDrag,
        LeftFrameDrag,
        BottomFrameDrag,
    }
    
    public class MouseMovement
    {
        public MouseMoveType Type { get; private set; }
        public int MotionStartX { get; set; } = 0;
        public int MotionStartY { get; set; } = 0;
        public int WindowOriginPointX { get; private set; } = 0;
        public int WindowOriginPointY { get; private set; } = 0;

        public MouseMovement(MouseMoveType type, int Motion_X, int Motion_Y, int Window_X, int Window_Y)
        {
            Type = type;
            MotionStartX = Motion_X;
            MotionStartY = Motion_Y;
            WindowOriginPointX = Window_X;
            WindowOriginPointY = Window_Y;
        }
    }
}