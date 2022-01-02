#if !NO_SDC
using System.Drawing.Drawing2D;
#endif
using System.Globalization;

namespace Svg.Transforms
{
    public sealed class SvgRotate : SvgTransform
    {
        public float Angle { get; set; }

        public float CenterX { get; set; }

        public float CenterY { get; set; }

#if !NO_SDC
        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Translate(CenterX, CenterY);
                matrix.Rotate(Angle);
                matrix.Translate(-CenterX, -CenterY);
                return matrix;
            }
        }
#endif

        public override string WriteToString()
        {
            return $"rotate({Angle.ToSvgString()}, {CenterX.ToSvgString()}, {CenterY.ToSvgString()})";
        }

        public SvgRotate(float angle)
        {
            Angle = angle;
        }

        public SvgRotate(float angle, float centerX, float centerY)
            : this(angle)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public override object Clone()
        {
            return new SvgRotate(Angle, CenterX, CenterY);
        }
    }
}
