using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("tref")]
    public class SvgTextRef : SvgTextBase
    {
        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
        }

        internal override IEnumerable<ISvgNode> GetContentNodes()
        {
            var refText = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) as SvgTextBase;
            if (refText == null)
            {
                return base.GetContentNodes();
            }
            else
            {
                return refText.GetContentNodes();
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextRef>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgTextRef;
            newObj.X = this.X;
            newObj.Y = this.Y;
            newObj.Dx = this.Dx;
            newObj.Dy = this.Dy;
            newObj.Text = this.Text;
            newObj.ReferencedElement = this.ReferencedElement;

            return newObj;
        }


    }
}
