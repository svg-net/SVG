using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgChannelSelector>))]
    public enum SvgChannelSelector
    {
        R,
        G,
        B,
        A
    }
}
