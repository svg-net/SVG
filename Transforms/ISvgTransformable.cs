using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

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
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        void PushTransforms(SvgRenderer renderer);
        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> that should have transforms removed.</param>
        void PopTransforms(SvgRenderer renderer);
    }
}