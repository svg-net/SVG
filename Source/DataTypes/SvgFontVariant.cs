using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgFontVariantConverter))]
    public enum SvgFontVariant
    {
        Normal,
        SmallCaps,
        Inherit
    }
}
