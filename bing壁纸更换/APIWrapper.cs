using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace bing壁纸更换
{
    class APIWrapper
    {
        //声明结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        //取得Shell窗口句柄函数 
        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();
        //取得桌面窗口句柄函数 
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        //取得前台窗口句柄函数 
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        //取得窗口大小函数 
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowRect(IntPtr hwnd, out RECT rc);

        private static IntPtr shellHandle;
        private static IntPtr desktopHandle;

        //判断是否有应用全屏
        static public bool isFullScreen()
        {
            shellHandle = GetShellWindow();
            desktopHandle = GetDesktopWindow();
            IntPtr foregroundHandle = GetForegroundWindow();
            if (foregroundHandle != null && !foregroundHandle.Equals(IntPtr.Zero))
            {
                if (foregroundHandle != shellHandle && foregroundHandle != desktopHandle)
                {
                    int screenHeight = Screen.PrimaryScreen.Bounds.Height;
                    int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                    RECT foregroundWindowRect;
                    GetWindowRect(foregroundHandle, out foregroundWindowRect);
                    if (Math.Abs(foregroundWindowRect.Right - foregroundWindowRect.Left) == screenWidth && Math.Abs(foregroundWindowRect.Bottom - foregroundWindowRect.Top) == screenHeight)
                        return true;
                }
            }
            return false;
        }
    }
}
