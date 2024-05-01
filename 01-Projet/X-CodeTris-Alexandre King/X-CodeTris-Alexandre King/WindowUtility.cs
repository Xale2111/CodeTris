using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

//This code comes from : https://stackoverflow.com/questions/67008500/how-to-move-c-sharp-console-application-window-to-the-center-of-the-screen
//It was litely modifed to move to window to the top left corner


namespace X_CodeTris_Alexandre_King
{
    static class WindowUtility
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;            

        public static void MoveWindowToTopLeftCorner()
        {
            IntPtr window = Process.GetCurrentProcess().MainWindowHandle;

            if (window == IntPtr.Zero)
                throw new Exception("Couldn't find a window to center!");            

            SetWindowPos(window, IntPtr.Zero, -10, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
        }
    }
}
