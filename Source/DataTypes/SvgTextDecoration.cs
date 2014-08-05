using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgTextDecorationConverter))]
    public enum SvgTextDecoration
    {
        inherit, 
        none,
        underline,
        overline,
        lineThrough,
        blink
    }
}
