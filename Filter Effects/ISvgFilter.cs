using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public interface ISvgFilter
    {
        void ApplyFilter(Bitmap sourceGraphic, Graphics renderer);
        List<SvgFilterPrimitive> Primitives { get; }
        Dictionary<string, Bitmap> Results { get; }
        SvgUnit Width { get; set; }
        SvgUnit Height { get; set; }
    }
}