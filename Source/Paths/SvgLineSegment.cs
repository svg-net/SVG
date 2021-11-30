using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(bool isRelative, PointF end)
            : base(isRelative, end)
        {
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            var end = ToAbsolute(End, IsRelative, start);
            graphicsPath.AddLine(start, end);
            return end;
        }

        public override string ToString()
        {
            if (float.IsNaN(End.Y))
                return (IsRelative ? "h" : "H") + End.X.ToSvgString();
            else if (float.IsNaN(End.X))
                return (IsRelative ? "v" : "V") + End.Y.ToSvgString();
            else
                return (IsRelative ? "l" : "L") + End.ToSvgString();
        }
    }
}
