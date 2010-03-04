using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public abstract class SvgTransform
    {
        public abstract Matrix Matrix { get; }
        public abstract string WriteToString();
    }
}