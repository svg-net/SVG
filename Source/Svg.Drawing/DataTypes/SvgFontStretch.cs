using System.ComponentModel;

namespace Svg
{
    /// <summary>The desired amount of condensing or expansion in the glyphs used to render the text.</summary>
    [TypeConverter(typeof(SvgFontStretchConverter))]
    public enum SvgFontStretch
    {
        Normal,
        Wider,
        Narrower,
        UltraCondensed,
        ExtraCondensed,
        Condensed,
        SemiCondensed,
        SemiExpanded,
        Expanded,
        ExtraExpanded,
        UltraExpanded,
        Inherit
    }
}
