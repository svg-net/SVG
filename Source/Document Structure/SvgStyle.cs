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
        /// <summary>
        /// This attribute specifies the style sheet language of the element's contents, as a media type.
        /// If the attribute is not specified, then the style sheet language is assumed to be CSS.
        /// </summary>
        [SvgAttribute("type")]
        public string StyleType
        {
            get { return GetAttribute<string>("type", false); }
            set { Attributes["type"] = value; }
        }

        /// <summary>
        /// This attribute specifies a media query that must be matched for the style sheet to apply.
        /// Its value is parsed as a <see href="https://www.w3.org/TR/mediaqueries-3/#syntax">media_query_list</see>. If not specified, the style sheet applies unconditionally.
        /// </summary>
        [SvgAttribute("media")]
        public string Media
        {
            get { return GetAttribute<string>("media", false); }
            set { Attributes["media"] = value; }
        }

        /// <summary>
        /// This attribute specifies a title for the style sheet, which is used when exposing and selecting between alternate style sheets. The attribute takes any value.
        /// </summary>
        [SvgAttribute("title")]
        public string Title
        {
            get { return GetAttribute<string>("title", false); }
            set { Attributes["title"] = value; }
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
