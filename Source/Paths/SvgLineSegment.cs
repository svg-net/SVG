using System.Drawing;
#if !NO_SDC
using System.Drawing.Drawing2D;
#endif

namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(bool isRelative, PointF end)
            : base(isRelative, end)
        {
        }

#if !NO_SDC
        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            var end = ToAbsolute(End, IsRelative, start);
            graphicsPath.AddLine(start, end);
            return end;
        }

        [System.Obsolete("Use new AddToPath.")]
        public override void AddToPath(GraphicsPath graphicsPath)
        {
            AddToPath(graphicsPath, Start, null);
        }
#endif

        public override string ToString()
        {
            if (float.IsNaN(End.Y))
                return (IsRelative ? "h" : "H") + End.X.ToSvgString();
            else if (float.IsNaN(End.X))
                return (IsRelative ? "v" : "V") + End.Y.ToSvgString();
            else
                return (IsRelative ? "l" : "L") + End.ToSvgString();
        }

        [System.Obsolete("Use new constructor.")]
        public SvgLineSegment(PointF start, PointF end)
            : this(false, end)
        {
            Start = start;
        }
    }
}
