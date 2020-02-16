using System.ComponentModel;

namespace Svg.FilterEffects
{
    [TypeConverter(typeof(SvgTurbulenceTypeConverter))]
    public enum SvgTurbulenceType
    {
        FractalNoise,
        Turbulence
    }
}
