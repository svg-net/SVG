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

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
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
        protected internal override bool PushTransforms(SvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;
            renderer.TranslateTransform(this.X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                        this.Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
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

        public override System.Drawing.Drawing2D.GraphicsPath Path(SvgRenderer renderer)
        {
            SvgVisualElement element = (SvgVisualElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
            return (element != null) ? element.Path(renderer) : null;
        }

        public override System.Drawing.RectangleF Bounds
        {
            get { return new System.Drawing.RectangleF(); }
        }

        //        public override SvgElementCollection Children
        //        {
        //            get
        //            {
        //                SvgElement element = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
        //                SvgElementCollection elements = new SvgElementCollection(this, true);
        //                elements.Add(element);
        //                return elements;
        //            }
        //        }

        protected override bool Renderable { get { return false; } }

        protected override void Render(SvgRenderer renderer)
        {
            if (!Visible || !Displayable)
                return;

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