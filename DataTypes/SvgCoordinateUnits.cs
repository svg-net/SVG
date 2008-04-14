using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Defines the various coordinate units certain SVG elements may use.
    /// </summary>
    public enum SvgCoordinateUnits
    {
        /// <summary>
        /// Indicates that the coordinate system of the entire document is to be used.
        /// </summary>
        UserSpaceOnUse,
        /// <summary>
        /// Indicates that the coordinate system of the owner element is to be used.
        /// </summary>
        ObjectBoundingBox
    }
}
