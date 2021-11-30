using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(bool isRelative, PointF moveTo)
            : base(isRelative, moveTo)
        {
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            graphicsPath.StartFigure();
            return ToAbsolute(End, IsRelative, start);
        }

        public override string ToString()
        {
            return (IsRelative ? "m" : "M") + End.ToSvgString();
        }
    }
}
