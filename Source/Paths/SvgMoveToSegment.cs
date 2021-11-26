using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(PointF moveTo)
            : base(moveTo)
        {
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start)
        {
            graphicsPath.StartFigure();
            return End;
        }

        public override string ToString()
        {
            return "M" + End.ToSvgString();
        }
    }
}
