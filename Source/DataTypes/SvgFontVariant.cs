using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg.DataTypes
{
    [TypeConverter(typeof(SvgFontVariantConverter))]
    public enum SvgFontVariant
    {
        normal,
        smallcaps,
        inherit
    }
}
