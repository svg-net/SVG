using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgFillRuleConverter))]
    public enum SvgFillRule
    {
        NonZero,
        EvenOdd,
        Inherit
    }
}