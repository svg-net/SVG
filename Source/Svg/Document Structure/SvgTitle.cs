namespace Svg
{
    [SvgElement("title")]
    public partial class SvgTitle : SvgElement, ISvgDescriptiveElement
    {
        public override string ToString()
        {
            return this.Content;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTitle>();
        }
    }
}
