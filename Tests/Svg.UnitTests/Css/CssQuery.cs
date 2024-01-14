using System.Collections.Generic;
using System.Linq;
using Fizzler;
using ExCSS;

namespace Svg.Css
{
    internal static class CssQuery
    {
        public static IEnumerable<SvgElement> QuerySelectorAll(this SvgElement elem, string selector, SvgElementFactory elementFactory)
        {
            var generator = new SelectorGenerator<SvgElement>(new SvgElementOps(elementFactory));
            Fizzler.Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(elem, 1));
        }

        public static int GetSpecificity(this ISelector selector)
        {
            var specificity = 0x0;
            // ID selector
            specificity |= (1 << 12) * selector.Specificity.Ids;
            // class selector
            specificity |= (1 << 8) * selector.Specificity.Classes;
            // element selector
            specificity |= (1 << 4) * selector.Specificity.Tags;
            return specificity;
        }
    }
}
