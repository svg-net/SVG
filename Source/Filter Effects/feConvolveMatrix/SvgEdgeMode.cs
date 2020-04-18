using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgEdgeModeConverter))]
    public enum SvgEdgeMode
    {
        Duplicate,
        Wrap,
        None
    }
}
