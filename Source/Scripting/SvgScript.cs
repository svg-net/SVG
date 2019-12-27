using System.Xml;

namespace Svg
{
    /// <summary>
    /// An element used to define scripts within SVG documents.
    /// Use the Script property to get the script content (proxies the content)
    /// </summary>
    [SvgElement("script")]
    public class SvgScript : SvgElement
    {
        public string Script
        {
            get { return this.Content; }
            set { this.Content = value; }
        }

        [SvgAttribute("type")]
        public string ScriptType
        {
            get { return GetAttribute<string>("type", false); }
            set { Attributes["type"] = value; }
        }

        [SvgAttribute("crossorigin")]
        public string CrossOrigin
        {
            get { return GetAttribute<string>("crossorigin", false); }
            set { Attributes["crossorigin"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public string Href
        {
            get { return GetAttribute<string>("href", false); }
            set { Attributes["href"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgScript>();
        }

        protected override void WriteChildren(XmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                // Always put the script in a CDATA tag
                writer.WriteCData(this.Content);
            }
        }
    }
}
