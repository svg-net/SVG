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
    }
}
