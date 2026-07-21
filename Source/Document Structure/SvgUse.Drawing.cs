#if !NO_SDC
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgUse : SvgVisualElement
    {
        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer))
                return false;
            renderer.TranslateTransform(X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this),
                MatrixOrder.Prepend);
            return true;
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            SvgVisualElement element = (SvgVisualElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
            return (element != null && !this.HasRecursiveReference()) ? element.Path(renderer) : null;
        }

        RectangleF RawBounds(Func<SvgVisualElement, RectangleF> boundsGetter, Func<RectangleF, RectangleF> transform)
        {
            var ew = this.Width.ToDeviceValue(null, UnitRenderingType.Horizontal, this);
            var eh = this.Height.ToDeviceValue(null, UnitRenderingType.Vertical, this);
            if (ew > 0 && eh > 0)
                return transform(new RectangleF(this.Location.ToDeviceValue(null, this),
                    new SizeF(ew, eh)));
            if (this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) is SvgVisualElement element)
            {
                return boundsGetter(element);
            }

            return new RectangleF();
        }
        /// <inheritdoc/>
        public override RectangleF Bounds => RawBounds(e => e.Bounds, TransformedBounds);
        /// <inheritdoc/>
        public override RectangleF BoundsRelativeToTop => RawBounds(e => e.BoundsRelativeToTop, r => r);

        protected override void RenderChildren(ISvgRenderer renderer)
        {
            if (ReferencedElement != null && !HasRecursiveReference())
            {
                var element = OwnerDocument.IdManager.GetElementById(ReferencedElement) as SvgVisualElement;
                if (element != null)
                {
                    var ew = Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                    var eh = Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                    if (ew > 0 && eh > 0)
                    {
                        var viewBox = element.Attributes.GetAttribute<SvgViewBox>("viewBox");
                        if (viewBox != SvgViewBox.Empty && Math.Abs(ew - viewBox.Width) > float.Epsilon && Math.Abs(eh - viewBox.Height) > float.Epsilon)
                        {
                            var sw = ew / viewBox.Width;
                            var sh = eh / viewBox.Height;
                            renderer.ScaleTransform(sw, sh, MatrixOrder.Prepend);
                        }
                    }

                    var origParent = element.Parent;
                    element._parent = this;
                    // as the new parent may have other styles that are inherited,
                    // we have to redraw the paths for the children
                    element.InvalidateChildPaths();
                    element.RenderElement(renderer);
                    element._parent = origParent;
                }
            }
        }
    }
}
#endif
