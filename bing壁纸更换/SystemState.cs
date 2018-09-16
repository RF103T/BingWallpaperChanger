using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace bing壁纸更换
{
    class SystemState
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

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_POWER_STATUS
        {
            public byte ACLineStatus;
            public byte BatteryFlag;
            public byte BatteryLifePercent;
            public byte Reserved1;
            public int BatteryLifeTime;
            public int BatteryFullLifeTime;
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
        //获得电源状态函数
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS systemPowerStatus);

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

        //判断当前是否是交流电接入
        static public bool isACLine()
        {
            SYSTEM_POWER_STATUS STATUS;
            if (GetSystemPowerStatus(out STATUS))
            {
                if (STATUS.ACLineStatus == 1)
                    return true;
            }
            return false;
        }
    }
}
