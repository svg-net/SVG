#if !NO_SDC
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgShear : SvgTransform
    {
        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Shear(X, Y);
                return matrix;
            }
        }
    }
}
#endif
