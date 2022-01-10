#if !NO_SDC
using System;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgSkew : SvgTransform
    {
        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Shear((float)Math.Tan(AngleX / 180f * Math.PI), (float)Math.Tan(AngleY / 180f * Math.PI));
                return matrix;
            }
        }
    }
}
#endif
