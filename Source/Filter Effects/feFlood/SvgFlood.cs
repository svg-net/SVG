namespace Svg.FilterEffects
{
    [SvgElement("feFlood")]
    public partial class SvgFlood : SvgFilterPrimitive
    {
        [SvgAttribute("flood-color")]
        public virtual SvgPaintServer FloodColor
        {
            get { return GetAttribute("flood-color", true, SvgPaintServer.NotSet); }
            set { Attributes["flood-color"] = value; }
        }

        [SvgAttribute("flood-opacity")]
        public virtual float FloodOpacity
        {
            get { return GetAttribute("flood-opacity", true, 1f); }
            set { Attributes["flood-opacity"] = FixOpacityValue(value); }
        }

#if !NO_SDC
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFlood filter Process().
            buffer[Result] = buffer[Input];
        }
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFlood>();
        }
    }
}
