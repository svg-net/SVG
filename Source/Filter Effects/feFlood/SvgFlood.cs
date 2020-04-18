namespace Svg.FilterEffects
{
    [SvgElement("feFlood")]
    public class SvgFlood : SvgFilterPrimitive
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

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFlood filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFlood>();
        }
    }
}
