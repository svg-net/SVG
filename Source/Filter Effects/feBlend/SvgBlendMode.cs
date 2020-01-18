using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgBlendMode>))]
    public enum SvgBlendMode
    {
        Normal,
        Multiply,
        Screen,
        Darken,
        Lighten
    }
}
