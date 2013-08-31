using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgStrokeLineJoinConverter))]
    public enum SvgStrokeLineJoin
    {
        Miter,
        Round,
        Bevel
    }
}
