using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgStrokeLineCapConverter))]
    public enum SvgStrokeLineCap
    {
        Butt,
        Round,
        Square,
        Inherit
    }
}
