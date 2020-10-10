namespace RawInput.Events
{
    using System;
    using System.Windows.Forms;

    public class BaseRawInputEventArgs : EventArgs
    {
        public bool IsDown { get; internal set; }

        public Keys Key { get; internal set; }
    }
}