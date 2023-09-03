#if AngleSharp
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using ExCSS;
using Fizzler;
using Svg.Css;

namespace Svg.UnitTests.Css
{
    internal static class AngleSharpCssQuery
    {
        public static IEnumerable<SvgElement> AngleSharpQuerySelectorAll(this SvgElement elem, string selector, SvgElementFactory elementFactory)
        {
            var parser = new CssSelectorParser();
            var angleSelector = parser.ParseSelector(selector);
            return angleSelector.MatchAll(elem.Children.OfType<SvgElement>(), elem).Cast<SvgElement>();
        }
    }
}
#endif
