using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        public PointF ControlPoint { get; set; }

        public SvgQuadraticCurveSegment(bool isRelative, PointF controlPoint, PointF end)
            : base(isRelative, end)
        {
            ControlPoint = controlPoint;
        }

        public SvgQuadraticCurveSegment(bool isRelative, PointF end)
            : this(isRelative, NaN, end)
        {
        }

        private static PointF CalculateFirstControlPoint(PointF start, PointF controlPoint)
        {
            var x1 = start.X + (controlPoint.X - start.X) * 2 / 3;
            var y1 = start.Y + (controlPoint.Y - start.Y) * 2 / 3;

            return new PointF(x1, y1);
        }

        private static PointF CalculateSecondControlPoint(PointF controlPoint, PointF end)
        {
            var x2 = controlPoint.X + (end.X - controlPoint.X) / 3;
            var y2 = controlPoint.Y + (end.Y - controlPoint.Y) / 3;

            return new PointF(x2, y2);
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            var controlPoint = ControlPoint;
            if (float.IsNaN(controlPoint.X) || float.IsNaN(controlPoint.Y))
            {
                var prev = parent.IndexOf(this) - 1;
                if (prev >= 0 && parent[prev] is SvgQuadraticCurveSegment prevSegment)
                {
                    var prevStart = graphicsPath.PathPoints[graphicsPath.PointCount - 4];
                    var prevControlPoint = ToAbsolute(prevSegment.ControlPoint, prevSegment.IsRelative, prevStart);
                    controlPoint = Reflect(prevControlPoint, start);
                }
                else
                    controlPoint = start;
            }
            else
                controlPoint = ToAbsolute(controlPoint, IsRelative, start);

            var end = ToAbsolute(End, IsRelative, start);
            graphicsPath.AddBezier(start, CalculateFirstControlPoint(start, controlPoint), CalculateSecondControlPoint(controlPoint, end), end);
            return end;
        }

        public override string ToString()
        {
            if (float.IsNaN(ControlPoint.X) || float.IsNaN(ControlPoint.Y))
                return (IsRelative ? "t" : "T") + End.ToSvgString();
            else
                return (IsRelative ? "q" : "Q") + ControlPoint.ToSvgString() + " " + End.ToSvgString();
        }
    }
}
