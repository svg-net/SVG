using System;

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

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextRef>();
        }
    }
}
