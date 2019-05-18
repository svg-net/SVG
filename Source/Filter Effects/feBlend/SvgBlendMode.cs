using System.ComponentModel;

namespace Svg
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
        Darken,
        Lighten
    }
}