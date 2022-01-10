using System.Drawing;

namespace Svg.Pathing
{
    public sealed partial class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(bool isRelative, PointF end)
            : base(isRelative, end)
        {
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

        [System.Obsolete("Use new constructor.")]
        public SvgLineSegment(PointF start, PointF end)
            : this(false, end)
        {
            Start = start;
        }
    }
}
