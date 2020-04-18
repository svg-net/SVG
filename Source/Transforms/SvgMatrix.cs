using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies custom transform to this Matrix (Required for projects created by the Inkscape).
    /// </summary>
    public sealed class SvgMatrix : SvgTransform
    {
        public List<float> Points { get; set; }

        public override Matrix Matrix
        {
            get
            {
                return new Matrix(
                    Points[0],
                    Points[1],
                    Points[2],
                    Points[3],
                    Points[4],
                    Points[5]
                );
            }
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "matrix({0}, {1}, {2}, {3}, {4}, {5})",
                Points[0], Points[1], Points[2], Points[3], Points[4], Points[5]);
        }

        public SvgMatrix(List<float> m)
        {
            Points = m;
        }

        public override object Clone()
        {
            return new SvgMatrix(Points);
        }
    }
}
