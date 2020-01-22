using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgStitchType>))]
    public enum SvgStitchType
    {
        Stitch,
        NoStitch
    }
}
