namespace RawInput.Events
{
    using RawInput.Native;
    using System.Windows.Forms;

    public class MouseRawInputEventArgs : BaseRawInputEventArgs
    {
        public MouseRawInputEventArgs(RawInputMouseData data)
        {
            X = data.Mouse.LastX;
            Y = data.Mouse.LastY;
            Flags = data.Mouse.Flags;
            Data = data;
            if (Data.Mouse.Buttons != RawMouseButtonFlags.None)
            {
                switch (Data.Mouse.Buttons)
                {
                    case RawMouseButtonFlags.LeftButtonDown:
                        Key = Keys.LButton;
                        IsDown = true;
                        break;

                    case RawMouseButtonFlags.LeftButtonUp:
                        Key = Keys.LButton;
                        IsDown = false;
                        break;

                    case RawMouseButtonFlags.RightButtonDown:
                        Key = Keys.RButton;
                        IsDown = true;
                        break;

                    case RawMouseButtonFlags.RightButtonUp:
                        Key = Keys.RButton;
                        IsDown = false;
                        break;

                    case RawMouseButtonFlags.MiddleButtonDown:
                        Key = Keys.MButton;
                        IsDown = true;
                        break;

                    case RawMouseButtonFlags.MiddleButtonUp:
                        Key = Keys.MButton;
                        IsDown = false;
                        break;
                }
            }
        }

        public RawInputMouseData Data { get; private set; }

        public int X { get; }

        public int Y { get; }

        public RawMouseFlags Flags { get; }
    }
}