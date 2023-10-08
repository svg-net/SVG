﻿using System;
using System.Collections.Generic;
 using System.Diagnostics;
 using System.Linq;
 using System.Reflection;
 using ExCSS;

namespace Svg.Css
{
    internal static class ExCssQuery
    {
        public static IEnumerable<SvgElement> QuerySelectorAll(this SvgElement elem, ISelector selector, SvgElementFactory elementFactory)
        {
            var input = Enumerable.Repeat(elem, 1);
            var ops = new ExSvgElementOps(elementFactory);

            var func = GetFunc(selector, ops, ops.Universal());
            var descendants = ops.Descendant();
            var func1 = func;
            func = f => func1(descendants(f));
            return func(input).Distinct();
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            CompoundSelector selector,
            ExSvgElementOps ops,
            Func<IEnumerable<SvgElement>,
                IEnumerable<SvgElement>> inFunc)
        {
            foreach (var it in selector)
            {
                inFunc = GetFunc(it, ops, inFunc);
            }

            return inFunc;
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            FirstChildSelector selector,
            ExSvgElementOps ops)
        {
            var step = selector.Step;
            var offset = selector.Offset;

            if (offset == 0)
            {
                return ops.FirstChild();
            }

            return ops.NthChild(step, offset);
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            FirstTypeSelector selector,
            ExSvgElementOps ops)
        {
            var step = selector.Step;
            var offset = selector.Offset;

            return ops.NthType(step, offset);
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            LastTypeSelector selector,
            ExSvgElementOps ops)
        {
            var step = selector.Step;
            var offset = selector.Offset;

            return ops.NthLastType(step, offset);
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            LastChildSelector selector,
            ExSvgElementOps ops)
        {
            var step = selector.Step;
            var offset = selector.Offset;

            if (offset == 0)
            {
                return ops.LastChild();
            }

            return ops.NthLastChild(step, offset);
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            ListSelector listSelector,
            ExSvgElementOps ops,
            Func<IEnumerable<SvgElement>,
                IEnumerable<SvgElement>> inFunc)
        {
            List<Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>>> results = new();

            foreach (var selector in listSelector)
            {
                results.Add(GetFunc(selector, ops, null));
            }

            return f =>
            {
                var svgElements = inFunc(f);
                var nodes = results[0](svgElements);
                for (int i = 1; i < results.Count; i++)
                {
                    nodes = nodes.Union(results[i](svgElements));
                }

                return nodes;
            };
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            PseudoClassSelector selector,
            ExSvgElementOps ops,
            Func<IEnumerable<SvgElement>,
                IEnumerable<SvgElement>> inFunc)
        {
            Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> pseudoFunc;
            if (selector.Class == PseudoClassNames.FirstChild)
            {
                pseudoFunc = ops.FirstChild();
            }
            else if (selector.Class == PseudoClassNames.LastChild)
            {
                pseudoFunc = ops.LastChild();
            }
            else if (selector.Class == PseudoClassNames.Empty)
            {
                pseudoFunc = ops.Empty();
            }
            else if (selector.Class == PseudoClassNames.OnlyChild)
            {
                pseudoFunc = ops.OnlyChild();
            }
            else if (selector.Class == PseudoClassNames.Hover)
            {
                // Currently no Hover Property exists in SvgElement so ignoring it now and returning empty
                pseudoFunc = ops.Empty();
            }
            else
            {
                if (selector.Class.StartsWith(PseudoClassNames.Not))
                {
                    var sel = selector.Class.Substring(PseudoClassNames.Not.Length + 1, selector.Class.Length - 2 - PseudoClassNames.Not.Length);
                    var parser = new StylesheetParser(true, true, tolerateInvalidValues: true);
                    var styleSheet = parser.Parse(sel);
                    var newSelector = styleSheet.StyleRules.First().Selector;
                    var func = GetFunc(newSelector, ops, ops.Universal());
                    var descendants = ops.Descendant();
                    var func1 = func;
                    func = f => func1(descendants(f));
                    HashSet<SvgElement> notElements = null;

                    pseudoFunc = f => f.Where(e =>
                    {
                        notElements ??= func(f).ToHashSet();
                        return !notElements.Contains(e);
                    });
                }
                else if (selector.Class.StartsWith(PseudoClassNames.Lang))
                {
                    // Currently no Language Property exists in SvgElement so ignoring it now and returning empty
                    pseudoFunc = ops.Empty();
                }
                else if (selector.Class.StartsWith(PseudoClassNames.Root))
                {
                    pseudoFunc = ops.Root();
                }
                else
                {
                    throw new NotImplementedException();    
                }
            }

            if (inFunc == null)
            {
                return pseudoFunc;
            }

            return f => pseudoFunc(inFunc(f));
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(
            ComplexSelector selector,
            ExSvgElementOps ops,
            Func<IEnumerable<SvgElement>,
                IEnumerable<SvgElement>> inFunc)
        {
            List<Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>>> results = new();


            foreach (var it in selector)
            {
                results.Add(GetFunc(it.Selector, ops, null));

                Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> combinatorFunc;
                if (it.Delimiter == Combinator.Child.Delimiter)
                {
                    combinatorFunc = ops.Child();
                }
                else if (it.Delimiter == Combinators.Descendent)
                {
                    combinatorFunc = ops.Descendant();
                }
                else if (it.Delimiter == Combinator.Deep.Delimiter)
                {
                    throw new NotImplementedException();
                }
                else if (it.Delimiter == Combinators.Adjacent)
                {
                    combinatorFunc = ops.Adjacent();
                }
                else if (it.Delimiter == Combinators.Sibling)
                {
                    combinatorFunc = ops.GeneralSibling();
                }
                else if (it.Delimiter == Combinators.Pipe)
                {
                    // Namespace
                    throw new NotImplementedException();
                }
                else if (it.Delimiter == Combinators.Column)
                {
                    throw new NotImplementedException();
                }
                else if (it.Delimiter == null)
                {
                    combinatorFunc = null;
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (combinatorFunc != null)
                {
                    results.Add(combinatorFunc);
                }
            }

            Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> result = inFunc;
            foreach (var it in results)
            {
                if (result == null)
                {
                    result = it;
                }
                else
                {
                    var temp = result;
                    result = f => it(temp(f));
                }
            }

            return result;
        }

        private static Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GetFunc(ISelector selector, ExSvgElementOps ops, Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> inFunc)
        {
            var func = selector switch
            {
                AllSelector allSelector => ops.Universal(),
                AttrAvailableSelector attrAvailableSelector => ops.AttributeExists(attrAvailableSelector.Attribute),
                AttrBeginsSelector attrBeginsSelector => ops.AttributePrefixMatch(attrBeginsSelector.Attribute, attrBeginsSelector.Value),
                AttrContainsSelector attrContainsSelector => ops.AttributeSubstring(attrContainsSelector.Attribute, attrContainsSelector.Value),
                AttrEndsSelector attrEndsSelector => ops.AttributeSuffixMatch(attrEndsSelector.Attribute, attrEndsSelector.Value),
                AttrHyphenSelector attrHyphenSelector => ops.AttributeDashMatch(attrHyphenSelector.Attribute, attrHyphenSelector.Value),
                AttrListSelector attrListSelector => ops.AttributeIncludes(attrListSelector.Attribute, attrListSelector.Value),
                AttrMatchSelector attrMatchSelector => ops.AttributeExact(attrMatchSelector.Attribute, attrMatchSelector.Value),
                AttrNotMatchSelector attrNotMatchSelector => ops.AttributeNotMatch(attrNotMatchSelector.Attribute, attrNotMatchSelector.Value),
                ClassSelector classSelector => ops.Class(classSelector.Class),
                ComplexSelector complexSelector =>  GetFunc(complexSelector, ops, inFunc),
                CompoundSelector compoundSelector => GetFunc(compoundSelector, ops, inFunc),
                FirstChildSelector firstChildSelector => GetFunc(firstChildSelector, ops),
                LastChildSelector lastChildSelector => GetFunc(lastChildSelector, ops),
                FirstColumnSelector firstColumnSelector => throw new NotImplementedException(),
                LastColumnSelector lastColumnSelector => throw new NotImplementedException(),
                FirstTypeSelector firstTypeSelector => GetFunc(firstTypeSelector, ops),
                LastTypeSelector lastTypeSelector => GetFunc(lastTypeSelector, ops),
                ChildSelector childSelector => ops.Child(),
                ListSelector listSelector => GetFunc(listSelector, ops, inFunc),
                NamespaceSelector namespaceSelector => throw new NotImplementedException(),
                PseudoClassSelector pseudoClassSelector => GetFunc(pseudoClassSelector, ops, inFunc),
                PseudoElementSelector pseudoElementSelector => throw new NotImplementedException(), 
                TypeSelector typeSelector => ops.Type(typeSelector.Name),
                UnknownSelector unknownSelector => throw new NotImplementedException(),
                IdSelector idSelector => ops.Id(idSelector.Id),
                PageSelector pageSelector => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };

            if (inFunc == null)
            {
                return func;
            }

            return f => func(inFunc(f));
        }

#if NETSTANDARD2_0 || NET462_OR_GREATER
        private static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumarable)
        {
            var result = new HashSet<T>();
            foreach (var it in enumarable)
            {
                result.Add(it);
            }

            return result;
        }
#endif

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
