#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed partial class SvgMoveToSegment : SvgPathSegment
    {
        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            graphicsPath.StartFigure();
            return ToAbsolute(End, IsRelative, start);
        }

        [System.Obsolete("Use new AddToPath.")]
        public override void AddToPath(GraphicsPath graphicsPath)
        {
            AddToPath(graphicsPath, Start, null);
        }
    }
}
#endif
