namespace Svg.FilterEffects
{
    [SvgElement("feDisplacementMap")]
    public class SvgDisplacementMap : SvgFilterPrimitive
    {
        [SvgAttribute("scale")]
        public float Scale
        {
            get { return GetAttribute("scale", false, 0f); }
            set { Attributes["scale"] = value; }
        }

        [SvgAttribute("xChannelSelector")]
        public SvgChannelSelector XChannelSelector
        {
            get { return GetAttribute("xChannelSelector", false, SvgChannelSelector.A); }
            set { Attributes["xChannelSelector"] = value; }
        }

        [SvgAttribute("yChannelSelector")]
        public SvgChannelSelector YChannelSelector
        {
            get { return GetAttribute("yChannelSelector", false, SvgChannelSelector.A); }
            set { Attributes["yChannelSelector"] = value; }
        }

        [SvgAttribute("in2")]
        public string Input2
        {
            get { return GetAttribute<string>("in2", false); }
            set { Attributes["in2"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feDisplacementMap filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDisplacementMap>();
        }
    }
}
