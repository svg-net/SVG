using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgComponentTransferTypeConverter))]
    public enum SvgComponentTransferType
    {
        Identity,
        Table,
        Discrete,
        Linear,
        Gamma
    }
}
