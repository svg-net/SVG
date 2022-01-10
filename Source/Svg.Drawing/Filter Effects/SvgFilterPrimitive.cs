using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
#if !NO_SDC
using System.Drawing.Drawing2D;
#endif

namespace Svg.FilterEffects
{
    public abstract partial class SvgFilterPrimitive : SvgElement
    {
#if !NO_SDC
        public abstract void Process(ImageBuffer buffer);
#endif
    }
}
