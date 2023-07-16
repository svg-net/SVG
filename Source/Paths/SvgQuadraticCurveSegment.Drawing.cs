using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed partial class SvgQuadraticCurveSegment : SvgPathSegment
    {
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

        private static PointF CalculateControlPoint(PointF start, PointF firstControlPoint)
        {
            var x1 = (firstControlPoint.X * 3 - start.X) / 2;
            var y1 = (firstControlPoint.Y * 3 - start.Y) / 2;

            return new PointF(x1, y1);
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            var controlPoint = ControlPoint;
            if (float.IsNaN(controlPoint.X) || float.IsNaN(controlPoint.Y))
            {
                var prev = parent.IndexOf(this) - 1;
                if (prev >= 0 && parent[prev] is SvgQuadraticCurveSegment)
                {
                    var prevStart = graphicsPath.PathPoints[graphicsPath.PointCount - 4];
                    var prevFirstControlPoint = graphicsPath.PathPoints[graphicsPath.PointCount - 3];
                    var prevControlPoint = CalculateControlPoint(prevStart, prevFirstControlPoint);
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

        [System.Obsolete("Use new AddToPath.")]
        public override void AddToPath(GraphicsPath graphicsPath)
        {
            AddToPath(graphicsPath, Start, null);
        }
    }
}
