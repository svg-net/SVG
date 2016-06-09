namespace Svg
{
    public class NonSvgElement : SvgElement
    {
        public NonSvgElement()
        {
        }

        public NonSvgElement(string elementName)
        {
            this.ElementName = elementName;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<NonSvgElement>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as NonSvgElement;

            return newObj;
        }
    }
}