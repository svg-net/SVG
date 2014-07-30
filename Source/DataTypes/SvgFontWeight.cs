using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgFontWeightConverter))]
    public enum SvgFontWeight
    {
        inherit,
        normal,
        bold,
        bolder,
        lighter,
        w100,
        w200,
        w300,
        w400, // same as normal
        w500,
        w600,
        w700, // same as bold
        w800,
        w900
    }
}
