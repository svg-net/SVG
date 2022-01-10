using System;
namespace Svg
{
    public class SvgGdiPlusCannotBeLoadedException : Exception
    {
        public SvgGdiPlusCannotBeLoadedException(Exception inner) : base("Cannot initialize gdi+ libraries. This is likely to be caused by running on a non-Windows OS without proper gdi+ replacement. Please refer to the documentation for more details.", inner) {}
    }
}