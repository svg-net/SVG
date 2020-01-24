using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgEdgeMode>))]
    public enum SvgEdgeMode
    {
        Duplicate,
        Wrap,
        None
    }
}
