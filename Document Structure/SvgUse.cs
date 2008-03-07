using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Svg
{
    public class SvgUse : SvgGraphicsElement
    {
        private Uri _referencedElement;

        [SvgAttribute("href")]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
        }

        public SvgUse()
        {
            
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get { return null; }
        }

        public override System.Drawing.RectangleF Bounds
        {
            get { return new System.Drawing.RectangleF(); }
        }

        protected override string  ElementName
        {
            get { return "use"; }
        }

        protected override void Render(System.Drawing.Graphics graphics)
        {
            this.PushTransforms(graphics);

            SvgGraphicsElement element = (SvgGraphicsElement)this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement);
            // For the time of rendering we want the referenced element to inherit
            // this elements transforms
            SvgElement parent = element._parent;
            element._parent = this;
            element.RenderElement(graphics);
            element._parent = parent;

            this.PopTransforms(graphics);
        }
    }
}