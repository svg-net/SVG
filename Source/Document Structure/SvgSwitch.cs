namespace Svg
{
    /// <summary>
    /// The 'switch' element evaluates the 'requiredFeatures', 'requiredExtensions' and 'systemLanguage' attributes on its direct child elements in order, and then processes and renders the first child for which these attributes evaluate to true
    /// </summary>
    [SvgElement("switch")]
    public partial class SvgSwitch : SvgVisualElement
    {

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgSwitch>();
        }
    }
}
