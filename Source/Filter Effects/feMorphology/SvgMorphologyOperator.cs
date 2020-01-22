using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgMorphologyOperator>))]
    public enum SvgMorphologyOperator
    {
        Erode,
        Dilate
    }
}
