#if !NO_SDC
namespace Svg
{
    /// <summary>
    /// Represents and element that may be transformed by renderer.
    /// </summary>
    public interface ISvgRenderTransformable
    {
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
#endif
