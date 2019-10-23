using System;

namespace Svg
{
    [SvgElement("font-face-uri")]
    public class SvgFontFaceUri : SvgElement
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return GetAttribute<Uri>("href", false); }
            set { Attributes["href"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFontFaceUri>();
        }
    }
}
