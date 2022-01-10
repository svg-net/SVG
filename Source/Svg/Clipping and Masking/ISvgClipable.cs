using System;

namespace Svg
{
    /// <summary>
    /// Defines the methods and properties that an <see cref="SvgElement"/> must implement to support clipping.
    /// </summary>
    public interface ISvgClipable
    {
        /// <summary>
        /// Gets or sets the ID of the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        Uri ClipPath { get; set; }
        /// <summary>
        /// Specifies the rule used to define the clipping region when the element is within a <see cref="SvgClipPath"/>.
        /// </summary>
        SvgClipRule ClipRule { get; set; }
    }
}
