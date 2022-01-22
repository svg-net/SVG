#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public abstract partial class SvgPathBasedElement : SvgVisualElement
    {
        /// <inheritdoc/>
        public override RectangleF Bounds => TransformedBoundsFromPathToClone(Path(null));
        /// <inheritdoc/>
        public override RectangleF BoundsRelativeToTop => TransformedBoundsPlusParentsFromPathToClone(Path(null));
    }
}
#endif
