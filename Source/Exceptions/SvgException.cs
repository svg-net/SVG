using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    public class SvgException : FormatException
    {
        public SvgException(string message) : base(message)
        {
        }
    }
}