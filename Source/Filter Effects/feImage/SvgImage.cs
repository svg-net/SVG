namespace Svg.FilterEffects
{
    [SvgElement("feImage")]
    public partial class SvgImage : SvgFilterPrimitive
    {
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual string Href
        {
            get { return GetAttribute<string>("href", false); }
            set { Attributes["href"] = value; }
        }

        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get { return GetAttribute("preserveAspectRatio", false, new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid)); }
            set { Attributes["preserveAspectRatio"] = value; }
        }

#if !NO_SDC
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feImage filter Process().
            buffer[Result] = buffer[Input];
        }
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgImage>();
        }
    }
}
