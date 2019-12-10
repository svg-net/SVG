using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(PointF start, PointF end)
            : base(start, end)
        {
        }

        public override void AddToPath(GraphicsPath graphicsPath)
        {
            graphicsPath.AddLine(Start, End);
        }

        public override string ToString()
        {
            return "L" + End.ToSvgString();
        }
    }
}
