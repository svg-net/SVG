using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgColourMatrixTypeConverter))]
    public enum SvgColourMatrixType
    {
        Matrix,
        Saturate,
        HueRotate,
        LuminanceToAlpha
    }
}
