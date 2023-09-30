﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg.Css
{
    internal class ExSvgElementOps : IExCssSelectorOps<SvgElement>
    {
        private readonly SvgElementFactory _elementFactory;

        public ExSvgElementOps(SvgElementFactory elementFactory)
        {
            _elementFactory = elementFactory;
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Type(string name)
        {
            if (_elementFactory.AvailableElementsDictionary.TryGetValue(name, out var types))
            {
                return nodes => nodes.Where(n => types.Contains(n.GetType()));
            }
            return nodes => Enumerable.Empty<SvgElement>();
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Universal()
        {
            return nodes => nodes;
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Id(string id)
        {
            return nodes => nodes.Where(n => n.ID == id);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Class(string clazz)
        {
            return AttributeIncludes("class", clazz);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeExists(string name)
        {
            return nodes => nodes.Where(n => n.ContainsAttribute(name));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeExact(string name, string value)
        {
            return nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val == value));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeNotMatch(string name, string value)
        {
            return nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val != value));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeIncludes(string name, string value)
        {
            return nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val.Split(' ').Contains(value)));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeDashMatch(string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val.Split('-').Contains(value))));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributePrefixMatch(string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val.StartsWith(value))));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeSuffixMatch(string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val.EndsWith(value))));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeSubstring(string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n => (n.TryGetAttribute(name, out var val) && val.Contains(value))));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> FirstChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.First<SvgElement>() == n);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> LastChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.Last<SvgElement>() == n);
        }

        private IEnumerable<T> GetByIds<T>(IList<T> items, IEnumerable<int> indices)
        {
            foreach (var i in indices)
            {
                if (i >= 0 && i < items.Count) yield return items[i];
            }
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> NthChild(int a, int b)
        {
            return nodes => nodes.Where(n => n.Parent != null && GetByIds(n.Parent.Children, (from i in Enumerable.Range(0, n.Parent.Children.Count / a) select a * i + b)).Contains(n));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> OnlyChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.Count == 1);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Empty()
        {
            return nodes => nodes.Where(n => n.Children.Count == 0);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Child()
        {
            return nodes => nodes.SelectMany<SvgElement, SvgElement>(n => n.Children);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Descendant()
        {
            return nodes => nodes.SelectMany(Descendants);
        }

        private IEnumerable<SvgElement> Descendants(SvgElement elem)
        {
            foreach (var child in elem.Children)
            {
                yield return child;
                foreach (var descendant in child.Descendants())
                {
                    yield return descendant;
                }
            }
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Adjacent()
        {
            return nodes => nodes.SelectMany(n => ElementsAfterSelf(n).Take(1));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> GeneralSibling()
        {
            return nodes => nodes.SelectMany(ElementsAfterSelf);
        }

        private IEnumerable<SvgElement> ElementsAfterSelf(SvgElement self)
        {
            return (self.Parent == null ? Enumerable.Empty<SvgElement>() : self.Parent.Children.Skip<SvgElement>(self.Parent.Children.IndexOf(self) + 1));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> NthLastChild(int a, int b)
        {
            return nodes => nodes.Reverse().Where(n => n.Parent != null && GetByIds(n.Parent.Children, (from i in Enumerable.Range(0, n.Parent.Children.Count / a) select a * i + b)).Contains(n));
        }
    }
}
