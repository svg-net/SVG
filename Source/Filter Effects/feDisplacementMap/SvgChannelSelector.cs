using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgChannelSelectorConverter))]
    public enum SvgChannelSelector
    {
        R,
        G,
        B,
        A
    }
}
