namespace Svg
{
    public class SvgUnknownElement : SvgElement
    {
        public SvgUnknownElement()
        {
        }

        public SvgUnknownElement(string elementName)
        {
            ElementName = elementName;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgUnknownElement>();
        }
    }
}
