using System.Xml;

namespace Svg
{
    /// <summary>
    /// An element used to embed CSS style sheets within SVG documents.
    /// Use the <see cref="SvgElement.Content"/> property to get or set the CSS content.
    /// </summary>
    [SvgElement("style")]
    public partial class SvgStyle : SvgElement, ISvgDescriptiveElement
    {
        [SvgAttribute("type")]
        public string StyleType
        {
            get { return GetAttribute<string>("type", false); }
            set { Attributes["type"] = value; }
        }

        [SvgAttribute("media")]
        public string Media
        {
            get { return GetAttribute<string>("media", false); }
            set { Attributes["media"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgStyle>();
        }

        protected override void WriteChildren(XmlWriter writer)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                writer.WriteCData(Content);
            }
        }
    }
}
