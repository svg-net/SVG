using System;
using System.Collections.Generic;
using System.Text;

namespace Svg.FilterEffects
{
    public interface ISvgFilterable
    {
        ISvgFilter Filter { get; set; }
    }
}
