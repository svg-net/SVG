namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified skew vector to this Matrix.
    /// </summary>
    public sealed partial class SvgSkew : SvgTransform
    {
        public float AngleX { get; set; }

        public float AngleY { get; set; }

        public override string WriteToString()
        {
            if (AngleY == 0f)
                return $"skewX({AngleX.ToSvgString()})";
            return $"skewY({AngleY.ToSvgString()})";
        }

        public SvgSkew(float x, float y)
        {
            AngleX = x;
            AngleY = y;
        }

        public override object Clone()
        {
            return new SvgSkew(AngleX, AngleY);
        }
    }
}
