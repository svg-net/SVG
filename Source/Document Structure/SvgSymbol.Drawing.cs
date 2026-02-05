#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgSymbol : SvgVisualElement
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

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer))
                return false;
            ViewBox.AddViewBoxTransform(AspectRatio, renderer, null);
            return true;
        }

        // Only render if the parent is set to a Use element
        protected override void Render(ISvgRenderer renderer)
        {
            if (_parent is SvgUse) base.Render(renderer);
        }
    }
}
#endif
