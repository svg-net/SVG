using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(PointF start, PointF end)
        {
            this.Start = start;
            this.End = end;
        }

        public override void AddToPath(GraphicsPath graphicsPath)
        {
            graphicsPath.AddLine(this.Start, this.End);
        }
        
        public override string ToString()
		{
        	return "L" + this.End.ToSvgString();
		}

    }
}