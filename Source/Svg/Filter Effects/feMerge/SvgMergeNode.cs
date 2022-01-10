namespace Svg.FilterEffects
{
    [SvgElement("feMergeNode")]
    public partial class SvgMergeNode : SvgElement
    {
        [SvgAttribute("in")]
        public string Input
        {
            get { return GetAttribute<string>("in", false); }
            set { Attributes["in"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMergeNode>();
        }
    }
}
