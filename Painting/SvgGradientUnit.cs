using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Defines the various cordinate systems that a gradient server may use.
    /// </summary>
    public enum SvgGradientUnit
    {
        /// <summary>
        /// Indicates that the coordinate system of the entire document is to be used.
        /// </summary>
        UserSpaceOnUse,
        /// <summary>
        /// Indicates that the coordinate system of the element using the gradient is to be used.
        /// </summary>
        ObjectBoundingBox
    }
}
