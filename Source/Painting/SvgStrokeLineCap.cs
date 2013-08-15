using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgStrokeLineCapConverter))]
    public enum SvgStrokeLineCap
    {
        Butt,
        Round,
        Square,
        Inherit
    }
}
