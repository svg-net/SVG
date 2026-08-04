#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgGroup : SvgMarkerElement
    {
        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <inheritdoc/>
        public override RectangleF Bounds => BoundsFromChildren(e => e.Bounds, TransformedBounds);
        /// <inheritdoc/>
        public override RectangleF BoundsRelativeToTop => BoundsFromChildren(e => e.BoundsRelativeToTop, r => r);
    }
}
#endif
