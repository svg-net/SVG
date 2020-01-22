using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(EnumBaseConverter<SvgTurbulenceType>))]
    public enum SvgTurbulenceType
    {
        FractalNoise,
        Turbulence
    }
}
