namespace Svg
{
    /// <summary>
    /// The 'foreignObject' element allows for inclusion of a foreign namespace which has its graphical content drawn by a different user agent
    /// </summary>
    [SvgElement("foreignObject")]
    public partial class SvgForeignObject : SvgVisualElement
    {
        protected override bool Renderable { get { return false; } }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgForeignObject>();
        }
    }
}
