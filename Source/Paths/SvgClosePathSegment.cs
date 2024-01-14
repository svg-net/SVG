namespace Svg.Pathing
{
    public sealed partial class SvgClosePathSegment : SvgPathSegment
    {
        public SvgClosePathSegment(bool isRelative)
            : base(isRelative)
        {
        }

        public override string ToString()
        {
            return IsRelative ? "z" : "Z";
        }

        [System.Obsolete("Use new constructor.")]
        public SvgClosePathSegment()
            : this(true)
        {
        }
    }
}
