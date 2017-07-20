using System;
using System.Collections.Generic;
using System.Text;
#if NETFULL
using System.Drawing.Drawing2D;
using System.Drawing;
#else
using System.DrawingCore.Drawing2D;
using System.DrawingCore;
#endif

using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Represents and element that may be transformed.
    /// </summary>
    public interface ISvgTransformable
    {
        /// <summary>
        /// Gets or sets an <see cref="SvgTransformCollection"/> of element transforms.
        /// </summary>
        SvgTransformCollection Transforms { get; set; }
        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        void PushTransforms(ISvgRenderer renderer);
        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that should have transforms removed.</param>
        void PopTransforms(ISvgRenderer renderer);
    }
}