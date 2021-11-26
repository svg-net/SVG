using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(PointF end)
            : base(end)
        {
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start)
        {
            var end = End;
            graphicsPath.AddLine(start, end);
            return end;
        }

        public override string ToString()
        {
            return "L" + End.ToSvgString();
        }
    }
}
