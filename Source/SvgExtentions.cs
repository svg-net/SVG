﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Threading;
using System.Globalization;

namespace Svg
{
    /// <summary>
    /// Svg helpers
    /// </summary>
    public static class SvgExtentions
    {
        public static void SetRectangle(this SvgRectangle r, RectangleF bounds)
        {
            r.X = bounds.X;
            r.Y = bounds.Y;
            r.Width = bounds.Width;
            r.Height = bounds.Height;
        }

        public static RectangleF GetRectangle(this SvgRectangle r)
        {
            return new RectangleF(r.X, r.Y, r.Width, r.Height);
        }

        public static string GetXML(this SvgDocument doc)
        {
            var ret = "";

            using (var ms = new MemoryStream())
            {
                doc.Write(ms);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                ret = sr.ReadToEnd();
                sr.Close();
            }

            return ret;
        }

        public static string GetXML(this SvgElement elem)
        {
            var result = "";

            var currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                using (StringWriter str = new StringWriter())
                {
                    using (XmlTextWriter xml = new XmlTextWriter(str))
                    {
                        elem.Write(xml);
                        result = str.ToString();

                    }
                }
            }
            finally
            {
                // Make sure to set back the old culture even an error occurred.
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            return result;
        }

        public static bool HasNonEmptyCustomAttribute(this SvgElement element, string name)
        {
            return element.CustomAttributes.ContainsKey(name) && !string.IsNullOrEmpty(element.CustomAttributes[name]);
        }

        public static void ApplyRecursive(this SvgElement elem, Action<SvgElement> action)
        {
            foreach (var e in elem
                .Traverse(e => e.Children))
            {
                action(e);
            }
        }

        public static void ApplyRecursiveDepthFirst(this SvgElement elem, Action<SvgElement> action)
        {
            foreach (var e in elem
                .TraverseDepthFirst(e => e.Children))
            {
                action(e);
            }
        }

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (childrenSelector == null) throw new ArgumentNullException(nameof(childrenSelector));

            var itemQueue = new Queue<T>(items);
            while (itemQueue.Count > 0)
            {
                var current = itemQueue.Dequeue();
                yield return current;
                foreach (var child in childrenSelector(current) ?? Enumerable.Empty<T>()) itemQueue.Enqueue(child);
            }
        }

        public static IEnumerable<T> Traverse<T>(this T root, Func<T, IEnumerable<T>> childrenSelector)
            => Enumerable.Repeat(root, 1).Traverse(childrenSelector);

        public static IEnumerable<T> TraverseDepthFirst<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (childrenSelector == null) throw new ArgumentNullException(nameof(childrenSelector));
            var itemStack = new Stack<T>(items ?? Enumerable.Empty<T>());

            while (itemStack.Count > 0)
            {
                var current = itemStack.Pop();
                yield return current;
                foreach (var child in childrenSelector(current) ?? Enumerable.Empty<T>()) itemStack.Push(child);
            }
        }

        public static IEnumerable<T> TraverseDepthFirst<T>(this T root, Func<T, IEnumerable<T>> childrenSelector)
            => Enumerable.Repeat(root, 1).TraverseDepthFirst(childrenSelector);
    }
}
