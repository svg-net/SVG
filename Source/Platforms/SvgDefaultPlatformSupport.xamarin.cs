using System;
using System.Collections.Generic;
using System.Text;

namespace Svg.Platforms
{
    internal class SvgDefaultPlatformSupport : ISvgPlatformSupport
    {
        /// <summary> Get System Dpi </summary>
        public int GetSystemDpi()
        {
            return Convert.ToInt32(Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density * 96.0);
        }
    }
}
