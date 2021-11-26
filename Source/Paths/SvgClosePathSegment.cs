using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public SvgClosePathSegment(PointF end)
            : base(end)
        {
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start)
        {
            graphicsPath.CloseFigure();

            return End;
        }

        public override string ToString()
        {
            return "z";
        }
    }
}
