using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgComponentTransferType>))]
    public enum SvgComponentTransferType
    {
        Identity,
        Table,
        Discrete,
        Linear,
        Gamma
    }
}
