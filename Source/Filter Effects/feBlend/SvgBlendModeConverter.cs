using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg.FilterEffects
{
    public sealed class SvgBlendModeConverter : EnumBaseConverter<SvgBlendMode>
    {
        public SvgBlendModeConverter()
            : base(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.DashedLowerCase)
        {
        }
    }
}