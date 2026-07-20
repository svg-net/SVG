using System.Xml;

namespace Svg
{
    /// <summary>
    /// Represents a list of re-usable SVG components.
    /// </summary>
    [SvgElement("metadata")]
    public partial class SvgDocumentMetadata : SvgElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgDocumentMetadata"/> class.
        /// </summary>
        public SvgDocumentMetadata()
        {
            Content = string.Empty;
        }

        protected override void WriteChildren(XmlWriter writer)
        {
            if (Nodes.Count > 0 || Children.Count > 0)
            {
                base.WriteChildren(writer);
                return;
            }

            if (!string.IsNullOrEmpty(Content))
            {
                writer.WriteRaw(Content); // write out metadata fragment as-is
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDocumentMetadata>();
        }

        public override void InitialiseFromXML(XmlReader reader, SvgDocument document)
        {
            // read in the metadata just as a string ready to be written straight back out again
            Content = reader.ReadInnerXml();
        }
    }
}
