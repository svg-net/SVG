using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg
{
    [SvgElement("tref")]
    public class SvgTextRef : SvgTextBase
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return GetAttribute<Uri>("href", false); }
            set { Attributes["href"] = value; }
        }

        internal override IEnumerable<ISvgNode> GetContentNodes()
        {
            var refText = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) as SvgTextBase;
            IEnumerable<ISvgNode> contentNodes = null;

            if (refText == null)
            {
                contentNodes = base.GetContentNodes();
            }
            else
            {
                contentNodes = refText.GetContentNodes();
            }

            contentNodes = contentNodes.Where(o => !(o is ISvgDescriptiveElement));

            return contentNodes;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextRef>();
        }
    }
}
