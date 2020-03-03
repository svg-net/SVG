using System;
using System.Runtime.InteropServices;

namespace Svg.Platforms
{
    internal class SvgDefaultPlatformSupport : ISvgPlatformSupport
    {
        public int GetSystemDpi()
        {
            bool isWindows;

#if NETCORE
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
            var platform = Environment.OSVersion.Platform;
            isWindows = platform == PlatformID.Win32NT; 
#endif

            if (isWindows)
            {
                // NOTE: starting with Windows 8.1, the DPI is no longer system-wide but screen-specific
                IntPtr hDC = GetDC(IntPtr.Zero);
                const int LOGPIXELSY = 90;
                int result = GetDeviceCaps(hDC, LOGPIXELSY);
                ReleaseDC(IntPtr.Zero, hDC);
                return result;
            }
            else
            {
                // hack for macOS and Linux
                return 96;
            }
        }

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        // isn't allowed in UWP Store Apps
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}
