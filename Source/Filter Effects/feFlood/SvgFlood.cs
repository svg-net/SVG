namespace Svg.FilterEffects
{
    [SvgElement("feFlood")]
    public class SvgFlood : SvgFilterPrimitive
    {
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
