using System;
using System.Drawing;

namespace Svg.Pathing
{
    public sealed partial class SvgArcSegment : SvgPathSegment
    {
        private const double RadiansPerDegree = Math.PI / 180.0;
        private const double DoublePI = Math.PI * 2;

        public float RadiusX { get; set; }

        public float RadiusY { get; set; }

        public float Angle { get; set; }

        public SvgArcSweep Sweep { get; set; }

        public SvgArcSize Size { get; set; }

        public SvgArcSegment(float radiusX, float radiusY, float angle, SvgArcSize size, SvgArcSweep sweep, bool isRelative, PointF end)
            : base(isRelative, end)
        {
            RadiusX = Math.Abs(radiusX);
            RadiusY = Math.Abs(radiusY);
            Angle = angle;
            Sweep = sweep;
            Size = size;
        }

        public override string ToString()
        {
            var arcFlag = Size == SvgArcSize.Large ? "1" : "0";
            var sweepFlag = Sweep == SvgArcSweep.Positive ? "1" : "0";
            return (IsRelative ? "a" : "A") + RadiusX.ToSvgString() + " " + RadiusY.ToSvgString() + " " + Angle.ToSvgString() + " " + arcFlag + " " + sweepFlag + " " + End.ToSvgString();
        }

        [Obsolete("Use new constructor.")]
        public SvgArcSegment(PointF start, float radiusX, float radiusY, float angle, SvgArcSize size, SvgArcSweep sweep, PointF end)
            : this(radiusX, radiusY, angle, size, sweep, false, end)
        {
            Start = start;
        }
    }

    [Flags]
    public enum SvgArcSweep
    {
        Negative = 0,
        Positive = 1
    }

    [Flags]
    public enum SvgArcSize
    {
        Small = 0,
        Large = 1
    }
}
