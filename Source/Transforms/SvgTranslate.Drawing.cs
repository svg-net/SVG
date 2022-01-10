#if !NO_SDC
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgTranslate : SvgTransform
    {
        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Translate(X, Y);
                return matrix;
            }
        }
    }
}
#endif
