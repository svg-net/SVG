using System;

namespace Svg
{
    [Obsolete("ISvgSupportsCoordinateUnits will be removed.")]
    internal interface ISvgSupportsCoordinateUnits
    {
        SvgCoordinateUnits GetUnits();
    }
}
