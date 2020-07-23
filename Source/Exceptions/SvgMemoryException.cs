using System;
using System.Runtime.Serialization;

namespace Svg.Exceptions
{
    [Serializable]
    public class SvgMemoryException : Exception
    {
        public SvgMemoryException() { }
        public SvgMemoryException(string message) : base(message) { }
        public SvgMemoryException(string message, Exception inner) : base(message, inner) { }

        protected SvgMemoryException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
