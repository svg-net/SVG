using System;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Svg.Exceptions
{
    [Serializable]
    public class SvgMemoryException : Exception
    {
        public SvgMemoryException() { }
        public SvgMemoryException(string message) : base(message) { }
        public SvgMemoryException(string message, Exception inner) : base(message, inner) { }

#if !NET8_0_OR_GREATER
        protected SvgMemoryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }
}
