using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fizzler;
using ExCSS;

namespace Svg.Css
{
    internal static class CssQuery
    {
        public static IEnumerable<SvgElement> QuerySelectorAll(this SvgElement elem, string selector)
        {
            var generator = new SelectorGenerator<SvgElement>(new SvgElementOps());
            Fizzler.Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(elem, 1));
        }

        public static int GetSpecificity(this BaseSelector selector)
        {
            if (selector is SimpleSelector)
            {
                var simpleCode = selector.ToString();
                if (simpleCode.StartsWith("#"))
                {
                    return 1 << 12;
                }
                else if (simpleCode.StartsWith("."))
                {
                    return 1 << 8;
                }
                else
                {
                    return 1 << 4;
                }
            }
            return 0;
        }
    }
}
