using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed partial class SvgCubicCurveSegment : SvgPathSegment
    {
        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            var firstControlPoint = FirstControlPoint;
            if (float.IsNaN(firstControlPoint.X) || float.IsNaN(firstControlPoint.Y))
            {
                var prev = parent.IndexOf(this) - 1;
                if (prev >= 0 && parent[prev] is SvgCubicCurveSegment)
                {
                    var prevSecondControlPoint = graphicsPath.PathPoints[graphicsPath.PointCount - 2];
                    firstControlPoint = Reflect(prevSecondControlPoint, start);
                }
                else
                    firstControlPoint = start;
            }
            else
                firstControlPoint = ToAbsolute(firstControlPoint, IsRelative, start);

            var end = ToAbsolute(End, IsRelative, start);
            graphicsPath.AddBezier(start, firstControlPoint, ToAbsolute(SecondControlPoint, IsRelative, start), end);
            return end;
        }

        [System.Obsolete("Use new AddToPath.")]
        public override void AddToPath(GraphicsPath graphicsPath)
        {
            AddToPath(graphicsPath, Start, null);
        }
    }
}
