﻿using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgCubicCurveSegment : SvgPathSegment
    {
        public PointF FirstControlPoint { get; set; }
        public PointF SecondControlPoint { get; set; }

        public SvgCubicCurveSegment(PointF firstControlPoint, PointF secondControlPoint, PointF end)
            : base(end)
        {
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start)
        {
            var end = End;
            graphicsPath.AddBezier(start, FirstControlPoint, SecondControlPoint, end);
            return end;
        }

        public override string ToString()
        {
            return "C" + FirstControlPoint.ToSvgString() + " " + SecondControlPoint.ToSvgString() + " " + End.ToSvgString();
        }
    }
}
