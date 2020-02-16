using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgBlendModeConverter))]
    public enum SvgBlendMode
    {
        Normal,
        Multiply,
        Screen,
        Overlay,
        Darken,
        Lighten,
        ColorDodge,
        ColorBurn,
        HardLight,
        SoftLight,
        Difference,
        Exclusion,
        Hue,
        Saturation,
        Color,
        Luminosity
    }
}
