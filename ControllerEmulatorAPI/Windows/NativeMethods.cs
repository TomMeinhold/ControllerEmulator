namespace ControllerEmulatorAPI.Windows
{
    using Microsoft.Win32;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class NativeMethods
    {
        /// <summary>
        /// @"%SystemRoot%\cursors\aero_arrow.cur"
        /// </summary>
        private static string cursorBefore;

        private static void ChangeCursor(string curFile)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", curFile);
            SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

        private const int SPI_SETCURSORS = 0x0057;

        private const int SPIF_UPDATEINIFILE = 0x01;

        private const int SPIF_SENDCHANGE = 0x02;

        public static void SetDefault()
        {
            ChangeCursor(cursorBefore);
        }

        public static void SetTransparent()
        {
            cursorBefore = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", "");
            ChangeCursor(new FileInfo("invisible.cur").FullName);
        }

        public static void Toggle(bool b)
        {
            if (b)
            {
                SetDefault();
            }
            else
            {
                SetTransparent();
            }
        }
    }
}