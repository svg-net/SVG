using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgMorphologyOperatorConverter))]
    public enum SvgMorphologyOperator
    {
        Erode,
        Dilate
    }
}
