namespace Svg
{
    public class NonSvgElement : SvgElement
    {
        public NonSvgElement()
        {
        }

        public NonSvgElement(string elementName)
        {
            ElementName = elementName;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<NonSvgElement>();
        }

        /// <summary>
        /// Publish the element name to be able to differentiate non-svg elements.
        /// </summary>
        public string Name
        {
            get { return ElementName; }
        }
    }
}
