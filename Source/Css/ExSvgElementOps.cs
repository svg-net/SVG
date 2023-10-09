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

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> NthType(int step, int offset)
        {
            return nodes => nodes.Where(n => n.Parent != null && GetByTypes(n.Parent.Children, step, offset).Contains(n));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> NthLastType(int step, int offset)
        {
            return nodes => nodes.Where(n => n.Parent != null && GetByTypes(n.Parent.Children.Reverse(), step, offset).Contains(n));
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
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.First() == n);
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> LastChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.Last() == n);
        }

        private IEnumerable<T> GetByIds<T>(IList<T> items, IEnumerable<int> indices)
        {
            foreach (var i in indices)
            {
                if (i >= 0 && i < items.Count) yield return items[i];
            }
        }

        private IEnumerable<SvgElement> GetByTypes(IEnumerable<SvgElement> items, int step, int offset)
        {
            Dictionary<string, int> counter = new();

            foreach (var it in items)
            {
                var type = it.ElementName;
                counter.TryGetValue(type, out var count);

                if (offset == count)
                {
                    yield return it;
                }
                else if (offset > count)
                {
                    if (step != 0)
                    {
                        if ((count - offset) % step == 0)
                        {
                            yield return it;
                        }
                    }
                }

                count++;
                counter[type] = count;
            }
        }

        private IEnumerable<T> GetByIdsReverse<T>(IList<T> items, IEnumerable<int> indices)
        {
            foreach (var i in indices)
            {
                if (i >= 0 && i < items.Count) yield return items[items.Count - 1 - i];
            }
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> NthChild(int step, int offset)
        {
            return nodes => nodes.Where(n => n.Parent != null && GetByIds(n.Parent.Children, step == 0 ? new[]{offset} : (from i in Enumerable.Range(0, n.Parent.Children.Count / step) select step * i + offset)).Contains(n));
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
            return nodes => nodes.SelectMany(n => n.Children);
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
            return (self.Parent == null ? Enumerable.Empty<SvgElement>() : self.Parent.Children.Skip(self.Parent.Children.IndexOf(self) + 1));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> NthLastChild(int step, int offset)
        {
            return nodes => nodes.Where(n => n.Parent != null && GetByIdsReverse(n.Parent.Children, step == 0 ? new[]{offset} : (from i in Enumerable.Range(0, n.Parent.Children.Count / step) select step * i + offset)).Contains(n));
        }

        public Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> Root()
        {
            return nodes =>
            {
                var node = nodes.FirstOrDefault();
                if (node == null)
                {
                    return Enumerable.Empty<SvgElement>();
                }

                var  root = node;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                return new List<SvgElement> { root };
            };
        }
    }
}
