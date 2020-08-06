using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgColourMatrixTypeConverter))]
    public enum SvgColourMatrixType
    {
        Matrix,
        Saturate,
        HueRotate,
        LuminanceToAlpha
    }
}
