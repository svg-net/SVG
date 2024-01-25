using System;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Svg
{
    [Serializable]
    public class SvgGdiPlusCannotBeLoadedException : Exception
    {
        const string gdiErrorMsg = "Cannot initialize gdi+ libraries. This is likely to be caused by running on a non-Windows OS without proper gdi+ replacement. Please refer to the documentation for more details.";

        public SvgGdiPlusCannotBeLoadedException() : base(gdiErrorMsg) { }
        public SvgGdiPlusCannotBeLoadedException(string message) : base(message) { }
        public SvgGdiPlusCannotBeLoadedException(Exception inner) : base(gdiErrorMsg, inner) {}
        public SvgGdiPlusCannotBeLoadedException(string message, Exception inner) : base(message, inner) { }

#if !NET8_0_OR_GREATER
        protected SvgGdiPlusCannotBeLoadedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }
}
