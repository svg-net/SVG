namespace Svg
{
    /// <summary>
    /// An element used to group SVG shapes.
    /// </summary>
    [SvgElement("g")]
    public partial class SvgGroup : SvgMarkerElement
    {
        protected override bool Renderable { get { return false; } }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGroup>();
        }
    }
}
