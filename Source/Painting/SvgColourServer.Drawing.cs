#if !NO_SDC
using System;
using System.Drawing;

namespace Svg
{
    public partial class SvgColourServer : SvgPaintServer
    {
        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            // is none?
            if (this == None) return new SolidBrush(System.Drawing.Color.Transparent);

            // default fill color is black, default stroke color is none
            if (this == NotSet && forStroke) return new SolidBrush(System.Drawing.Color.Transparent);

            int alpha = (int)Math.Round((opacity * (this.Colour.A / 255.0)) * 255);
            Color colour = System.Drawing.Color.FromArgb(alpha, this.Colour);

            return new SolidBrush(colour);
        }
    }
}
#endif
