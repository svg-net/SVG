using System.Drawing;

namespace Svg.Pathing
{
    public sealed partial class SvgQuadraticCurveSegment : SvgPathSegment
    {
        public PointF ControlPoint { get; set; }

        public SvgQuadraticCurveSegment(bool isRelative, PointF controlPoint, PointF end)
            : base(isRelative, end)
        {
            ControlPoint = controlPoint;
        }

        public SvgQuadraticCurveSegment(bool isRelative, PointF end)
            : this(isRelative, NaN, end)
        {
        }

        public override string ToString()
        {
            if (float.IsNaN(ControlPoint.X) || float.IsNaN(ControlPoint.Y))
                return (IsRelative ? "t" : "T") + End.ToSvgString();
            else
                return (IsRelative ? "q" : "Q") + ControlPoint.ToSvgString() + " " + End.ToSvgString();
        }

        [System.Obsolete("Use new constructor.")]
        public SvgQuadraticCurveSegment(PointF start, PointF controlPoint, PointF end)
            : this(false, controlPoint, end)
        {
            Start = start;
        }
    }
}
