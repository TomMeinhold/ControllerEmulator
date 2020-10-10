namespace RawInput.Events
{
    using RawInput.Native;
    using System.Windows.Forms;

    public class KeyboardRawInputEventArgs : BaseRawInputEventArgs
    {
        public KeyboardRawInputEventArgs(RawInputKeyboardData data)
        {
            Data = data;
            IsDown = Data.Keyboard.Flags == RawKeyboardFlags.Down;
            Key = (Keys)Data.Keyboard.VirutalKey;
        }

        public RawInputKeyboardData Data { get; private set; }
    }
}