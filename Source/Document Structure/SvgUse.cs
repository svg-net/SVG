using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Svg
{
    [SvgElement("use")]
    public class SvgUse : SvgVisualElement
    {
        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
        }

        private bool ElementReferencesUri(SvgElement element, List<Uri> elementUris)
        {
            var useElement = element as SvgUse;
            if (useElement != null)
            {
                if (elementUris.Contains(useElement.ReferencedElement))
                {
                    return true;
                }
                // also detect cycles in referenced elements
                var refElement = this.OwnerDocument.IdManager.GetElementById(useElement.ReferencedElement);
                if (refElement is SvgUse)
                {
                    elementUris.Add(useElement.ReferencedElement);
                }
                return useElement.ReferencedElementReferencesUri(elementUris);
            }
            var groupElement = element as SvgGroup;
            if (groupElement != null)
            {
                foreach (var child in groupElement.Children)
                {
                    if (ElementReferencesUri(child, elementUris))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ReferencedElementReferencesUri( List<Uri> elementUris )
        {
            var refElement = this.OwnerDocument.IdManager.GetElementById(ReferencedElement);
            return ElementReferencesUri(refElement, elementUris);
        }

        /// <summary>
        /// Checks for any direct or indirect recursions in referenced elements, 
        /// including recursions via groups.
        /// </summary>
        /// <returns>True if any recursions are found.</returns>
        private bool HasRecursiveReference()
        {
            var refElement = this.OwnerDocument.IdManager.GetElementById(ReferencedElement);
            var uris = new List<Uri>() { ReferencedElement };
            return ElementReferencesUri(refElement, uris);
        }

        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
            set { this.Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
            set { this.Attributes["y"] = value; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;
            renderer.TranslateTransform(this.X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                        this.Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this),
                                        MatrixOrder.Prepend);
            return true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgUse"/> class.
        /// </summary>
        public SvgUse()
        {
            this.X = 0;
            this.Y = 0;
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            SvgVisualElement element = (SvgVisualElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
            return (element != null && !this.HasRecursiveReference()) ? element.Path(renderer) : null;
        }

        public override System.Drawing.RectangleF Bounds
        {
            get
            {
                var element = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) as SvgVisualElement;
                if (element != null)
                {
                    return element.Bounds;
                }
                return new System.Drawing.RectangleF();
            }
        }

        protected override bool Renderable { get { return false; } }

        protected override void Render(ISvgRenderer renderer)
        {
            if (this.Visible && this.Displayable && !this.HasRecursiveReference() && this.PushTransforms(renderer))
            {
                this.SetClip(renderer);

                var element = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) as SvgVisualElement;
                if (element != null)
                {
                    var origParent = element.Parent;
                    element._parent = this;
                    // as the new parent may have other styles that are inherited,
                    // we have to redraw the paths for the children
                    element.InvalidateChildPaths();
                    element.RenderElement(renderer);
                    element._parent = origParent;
                }

                this.ResetClip(renderer);
                this.PopTransforms(renderer);
            }
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgUse>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgUse;
            newObj.ReferencedElement = this.ReferencedElement;
            newObj.X = this.X;
            newObj.Y = this.Y;

            return newObj;
        }

    }
}