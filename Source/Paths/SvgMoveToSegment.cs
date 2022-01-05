using System.Drawing;
#if !NO_SDC
using System.Drawing.Drawing2D;
#endif

namespace Svg.Pathing
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(bool isRelative, PointF moveTo)
            : base(isRelative, moveTo)
        {
        }

#if !NO_SDC
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
#endif

        public override string ToString()
        {
            return (IsRelative ? "m" : "M") + End.ToSvgString();
        }

        [System.Obsolete("Use new constructor.")]
        public SvgMoveToSegment(PointF moveTo)
            : this(false, moveTo)
        {
            Start = moveTo;
        }
    }
}
