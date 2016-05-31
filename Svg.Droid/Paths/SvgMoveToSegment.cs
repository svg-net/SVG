using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Svg.Pathing
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(PointF moveTo)
        {
            this.Start = moveTo;
            this.End = moveTo;
        }

        public override void AddToPath(GraphicsPath graphicsPath)
        {
            //graphicsPath.StartFigure();
            graphicsPath.MoveTo(this.Start);
        }
        
        public override string ToString()
		{
        	return "M" + this.Start.ToSvgString();
		}

    }
}
