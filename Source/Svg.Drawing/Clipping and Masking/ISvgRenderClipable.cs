#if !NO_SDC
using System;

namespace Svg
{
    /// <summary>
    /// Defines the methods and properties that an <see cref="SvgElement"/> must implement to support renderer clipping.
    /// </summary>
    public interface ISvgRenderClipable
    {
        /// <summary>
        /// Sets the clipping region of the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region set.</param>
        void SetClip(ISvgRenderer renderer);
        /// <summary>
        /// Resets the clipping region of the specified <see cref="ISvgRenderer"/> back to where it was before the <see cref="SetClip"/> method was called.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region reset.</param>
        void ResetClip(ISvgRenderer renderer);
    }
}
#endif
