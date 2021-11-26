using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public abstract class SvgPathSegment
    {
        public PointF End { get; set; }

        protected SvgPathSegment(PointF end)
        {
            End = end;
        }

        public abstract PointF AddToPath(GraphicsPath graphicsPath, PointF start);

        public SvgPathSegment Clone()
        {
            return MemberwiseClone() as SvgPathSegment;
        }
    }
}
