using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        public PointF ControlPoint { get; set; }

        public SvgQuadraticCurveSegment(PointF controlPoint, PointF end)
            : base(end)
        {
            ControlPoint = controlPoint;
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

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start)
        {
            var end = End;
            graphicsPath.AddBezier(start, CalculateFirstControlPoint(start, ControlPoint), CalculateSecondControlPoint(ControlPoint, end), end);
            return end;
        }

        public override string ToString()
        {
            return "Q" + ControlPoint.ToSvgString() + " " + End.ToSvgString();
        }
    }
}
