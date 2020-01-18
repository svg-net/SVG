namespace Svg.FilterEffects
{
    [SvgElement("feTurbulence")]
    public class SvgTurbulence : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feTurbulence filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTurbulence>();
        }
    }
}
