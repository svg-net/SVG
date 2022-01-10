#if !NO_SDC
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgRotate : SvgTransform
    {
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
    }
}
#endif
