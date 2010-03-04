using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg.Transforms
{
    [TypeConverter(typeof(SvgTransformConverter))]
    public class SvgTransformCollection : List<SvgTransform>
    {
    }
}
