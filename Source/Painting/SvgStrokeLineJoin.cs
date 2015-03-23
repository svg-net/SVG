using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgStrokeLineJoinConverter))]
    public enum SvgStrokeLineJoin
    {
        Miter,
        Round,
        Bevel
    }
}
