using System;
using System.Drawing;

namespace Svg.Pathing
{
    public abstract partial class SvgPathSegment
    {
        protected static readonly PointF NaN = new PointF(float.NaN, float.NaN);

        public bool IsRelative { get; set; }

        public PointF End { get; set; }

        protected SvgPathSegment(bool isRelative)
        {
            IsRelative = isRelative;
        }

        protected SvgPathSegment(bool isRelative, PointF end)
            : this(isRelative)
        {
            End = end;
        }

        public SvgPathSegment Clone()
        {
            return MemberwiseClone() as SvgPathSegment;
        }

        [Obsolete("Will be removed.")]
        public PointF Start { get; set; }
    }
}
