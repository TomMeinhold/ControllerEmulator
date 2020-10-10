namespace RawInput
{
    using Microsoft.Win32;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class Cursor
    {
        private static readonly string DefaultCursor = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", "");

        public static void ChangeCursor(string curFile)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", curFile);
            UpdateCursor();
        }

        public static void ChangeCursor(FileInfo file)
        {
            if (file is null)
            {
                throw new System.ArgumentNullException(nameof(file));
            }

            ChangeCursor(file.FullName);
        }

        public static void UpdateCursor()
        {
            NativeMethods.SystemParametersInfo(NativeMethods.SPI_SETCURSORS, 0, 0, NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDCHANGE);
        }

        public static void SetDefault()
        {
            ChangeCursor(DefaultCursor);
            UpdateCursor();
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
            public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

            public const int SPI_SETCURSORS = 0x0057;

            public const int SPIF_UPDATEINIFILE = 0x01;

            public const int SPIF_SENDCHANGE = 0x02;
        }
    }
}