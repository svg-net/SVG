using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgCompositeOperatorConverter))]
    public enum SvgCompositeOperator
    {
        Over,
        In,
        Out,
        Atop,
        Xor,
        Arithmetic
    }
}
