using System;
using System.Runtime.InteropServices;

namespace PdfMerge.Utils
{
    public static class WindowsShell
    {
        private static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        static WindowsShell()
        {
            SHGetKnownFolderPath(WindowsShell.Downloads, 0, IntPtr.Zero, out DOWNLOADSFOLDER);
        }

        public static string DOWNLOADSFOLDER;

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);
    }
}
