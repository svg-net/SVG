using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public SvgClosePathSegment(bool isRelative)
            : base(isRelative)
        {
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            graphicsPath.CloseFigure();

            var end = start;
            for (var i = graphicsPath.PointCount - 1; i >= 0; --i)
                if ((graphicsPath.PathTypes[i] & 0x7) == 0)
                {
                    end = graphicsPath.PathPoints[i];
                    break;
                }
            return end;
        }

        public override string ToString()
        {
            return IsRelative ? "z" : "Z";
        }
    }
}
