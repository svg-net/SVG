using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgCompositeOperator>))]
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
