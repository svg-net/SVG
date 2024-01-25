using System;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Svg
{
    [Serializable]
    public class SvgException : FormatException
    {
        public SvgException() { }
        public SvgException(string message) : base(message) { }
        public SvgException(string message, Exception inner) : base(message, inner) { }

#if !NET8_0_OR_GREATER
        protected SvgException(SerializationInfo info, StreamingContext context)
            : base (info, context) { }
#endif
    }

    [Serializable]
    public class SvgIDException : FormatException
    {
        public SvgIDException() { }
        public SvgIDException(string message) : base(message) { }
        public SvgIDException(string message, Exception inner) : base(message, inner) { }

#if !NET8_0_OR_GREATER
        protected SvgIDException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }

    [Serializable]
    public class SvgIDExistsException : SvgIDException
    {
        public SvgIDExistsException() { }
        public SvgIDExistsException(string message) : base(message) { }
        public SvgIDExistsException(string message, Exception inner) : base(message, inner) { }

#if !NET8_0_OR_GREATER
        protected SvgIDExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }

    [Serializable]
    public class SvgIDWrongFormatException : SvgIDException
    {
        public SvgIDWrongFormatException() { }
        public SvgIDWrongFormatException(string message) : base(message) { }
        public SvgIDWrongFormatException(string message, Exception inner) : base(message, inner) { }

#if !NET8_0_OR_GREATER
        protected SvgIDWrongFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }
}
