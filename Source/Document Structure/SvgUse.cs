using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;

namespace Svg
{
    [SvgElement("use")]
    public class SvgUse : SvgVisualElement
    {
        private Uri _referencedElement;

        [SvgAttribute("href")]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
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
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        protected internal override void PushTransforms(SvgRenderer renderer)
        {
            base.PushTransforms(renderer);
            renderer.TranslateTransform(this.X.ToDeviceValue(this), this.Y.ToDeviceValue(this, true));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgUse"/> class.
        /// </summary>
        public SvgUse()
        {
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get
            {
                SvgVisualElement element = (SvgVisualElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
                return (element != null) ? element.Path : null;
            }
        }

        public override System.Drawing.RectangleF Bounds
        {
            get { return new System.Drawing.RectangleF(); }
        }

        public override SvgElementCollection Children
        {
            get
            {
                SvgElement element = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
                SvgElementCollection elements = new SvgElementCollection(this, true);
                elements.Add(element);
                return elements;
            }
        }

        protected override void Render(SvgRenderer renderer)
        {
            this.PushTransforms(renderer);

            SvgVisualElement element = (SvgVisualElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
            // For the time of rendering we want the referenced element to inherit
            // this elements transforms
            SvgElement parent = element._parent;
            element._parent = this;
            element.RenderElement(renderer);
            element._parent = parent;

            this.PopTransforms(renderer);
        }
    }
}