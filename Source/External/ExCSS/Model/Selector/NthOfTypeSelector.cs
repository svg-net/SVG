
// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal sealed class NthOfTypeSelector : NthChildSelector, IToString
    {
        public override string ToString(bool friendlyFormat, int indentation)
        {
            return FormatSelector(PseudoSelectorPrefix.PseudoFunctionNthOfType);
        }
    }
}