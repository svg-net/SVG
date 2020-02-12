using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgStitchTypeConverter))]
    public enum SvgStitchType
    {
        Stitch,
        NoStitch
    }
}
