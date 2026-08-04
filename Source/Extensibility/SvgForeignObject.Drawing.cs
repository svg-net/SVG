#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgForeignObject : SvgVisualElement
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

        ///// <summary>
        ///// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        ///// </summary>
        ///// <param name="renderer">The <see cref="Graphics"/> object to render to.</param>
        //protected override void Render(SvgRenderer renderer)
        //{
        //    if (!Visible || !Displayable)
        //        return;

        //    this.PushTransforms(renderer);
        //    this.SetClip(renderer);
        //    base.RenderChildren(renderer);
        //    this.ResetClip(renderer);
        //    this.PopTransforms(renderer);
        //}
    }
}
#endif
