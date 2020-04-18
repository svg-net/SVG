namespace Svg
{
    [SvgElement("a")]
    public class SvgAnchor : SvgElement
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public string Href
        {
            get { return GetAttribute<string>("href", false); }
            set { Attributes["href"] = value; }
        }

        [SvgAttribute("show", SvgAttributeAttribute.XLinkNamespace)]
        public string Show
        {
            get { return GetAttribute<string>("show", false); }
            set { Attributes["show"] = value; }
        }

        [SvgAttribute("title", SvgAttributeAttribute.XLinkNamespace)]
        public string Title
        {
            get { return GetAttribute<string>("title", false); }
            set { Attributes["title"] = value; }
        }

        [SvgAttribute("target")]
        public string Target
        {
            get { return GetAttribute<string>("target", false); }
            set { Attributes["target"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgAnchor>();
        }
    }
}
