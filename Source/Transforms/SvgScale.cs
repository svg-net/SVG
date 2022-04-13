namespace Svg.Transforms
{
    public sealed partial class SvgScale : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override string WriteToString()
        {
            if (X == Y)
                return $"scale({X.ToSvgString()})";
            return $"scale({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgScale(float x)
            : this(x, x)
        {
        }

        public SvgScale(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override object Clone()
        {
            return new SvgScale(X, Y);
        }
    }
}
