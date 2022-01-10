using System.Drawing;

namespace Svg.Pathing
{
    public sealed partial class SvgCubicCurveSegment : SvgPathSegment
    {
        public PointF FirstControlPoint { get; set; }
        public PointF SecondControlPoint { get; set; }

        public SvgCubicCurveSegment(bool isRelative, PointF firstControlPoint, PointF secondControlPoint, PointF end)
            : base(isRelative, end)
        {
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
        }

        public SvgCubicCurveSegment(bool isRelative, PointF secondControlPoint, PointF end)
            : this(isRelative, NaN, secondControlPoint, end)
        {
        }

        public override string ToString()
        {
            if (float.IsNaN(FirstControlPoint.X) || float.IsNaN(FirstControlPoint.Y))
                return (IsRelative ? "s" : "S") + SecondControlPoint.ToSvgString() + " " + End.ToSvgString();
            else
                return (IsRelative ? "c" : "C") + FirstControlPoint.ToSvgString() + " " + SecondControlPoint.ToSvgString() + " " + End.ToSvgString();
        }

        [System.Obsolete("Use new constructor.")]
        public SvgCubicCurveSegment(PointF start, PointF firstControlPoint, PointF secondControlPoint, PointF end)
            : this(false, firstControlPoint, secondControlPoint, end)
        {
            Start = start;
        }
    }
}
