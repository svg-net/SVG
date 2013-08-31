using System;

namespace Svg
{
    public class SvgException : FormatException
    {
        public SvgException(string message) : base(message)
        {
        }
    }
}