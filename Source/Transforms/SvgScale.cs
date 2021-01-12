using System.Drawing.Drawing2D;
using System.Globalization;

namespace Svg.Transforms
{
    public sealed class SvgScale : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Scale(X, Y);
                return matrix;
            }
        }

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
