using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg
{
    [SvgElement("tref")]
    public partial class SvgTextRef : SvgTextBase
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return GetAttribute<Uri>("href", false); }
            set { Attributes["href"] = value; }
        }

#if !NO_SDC
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
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextRef>();
        }
    }
}
