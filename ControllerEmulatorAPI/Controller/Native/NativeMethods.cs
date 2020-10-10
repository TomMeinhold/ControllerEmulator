namespace ControllerEmulatorAPI.Controller.Native
{
    using System.Runtime.InteropServices;

    public static class NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
    }
}