using System.ComponentModel;

namespace Svg.FilterEffects
{
    /// <summary>
    /// Indicates the blend mode used to blend 2 images using the feBlend filter primitive.
    /// </summary>
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
