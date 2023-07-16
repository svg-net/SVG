using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgMatrix : SvgTransform
    {
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
    }
}
