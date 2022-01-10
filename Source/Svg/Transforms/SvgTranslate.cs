namespace Svg.Transforms
{
    public sealed partial class SvgTranslate : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override string WriteToString()
        {
            return $"translate({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgTranslate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public SvgTranslate(float x)
            : this(x, 0f)
        {
        }

        public override object Clone()
        {
            return new SvgTranslate(X, Y);
        }
    }
}
