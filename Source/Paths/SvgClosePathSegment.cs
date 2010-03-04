using System;
using System.Collections.Generic;
using System.Text;

namespace Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            graphicsPath.CloseFigure();
        }
    }
}
