using System;
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
        public static Brush GetBrush(this SvgPaintServer source, SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity)
        {
            return source.GetBrush(styleOwner, renderer, opacity, false);
        }

        public static void SetClip(this ISvgRenderer source,Region region)
        {
            source.SetClip(region, System.Drawing.Drawing2D.CombineMode.Replace);
        }
        public static void ScaleTransform(this ISvgRenderer source, float dx, float dy)
        {
            source.ScaleTransform(dx, dy, System.Drawing.Drawing2D.MatrixOrder.Append);
        }

        public static void TranslateTransform(this ISvgRenderer source, float dx, float dy)
        {
            source.TranslateTransform(dx, dy, System.Drawing.Drawing2D.MatrixOrder.Append);
        }
        public static void SetRectangle(this SvgRectangle r, RectangleF bounds)
        {
            r.X = bounds.X;
            r.Y = bounds.Y;
            r.Width = bounds.Width;
            r.Height = bounds.Height;
        }


        public static IEnumerable<SvgElement> Descendants<T>(this IEnumerable<T> source) where T : SvgElement
        {
            if (source == null) throw new ArgumentNullException("source");
            return GetDescendants<T>(source, false);
        }
        private static IEnumerable<SvgElement> GetAncestors<T>(IEnumerable<T> source, bool self) where T : SvgElement
        {
            foreach (var start in source)
            {
                if (start != null)
                {
                    for (var elem = (self ? start : start.Parent) as SvgElement; elem != null; elem = (elem.Parent as SvgElement))
                    {
                        yield return elem;
                    }
                }
            }
            yield break;
        }

        private static IEnumerable<SvgElement> GetDescendants<T>(IEnumerable<T> source, bool self) where T : SvgElement
        {
            var positons = new Stack<int>();
            int currPos;
            SvgElement currParent;
            foreach (var start in source)
            {
                if (start != null)
                {
                    if (self) yield return start;

                    positons.Push(0);
                    currParent = start;

                    while (positons.Count > 0)
                    {
                        currPos = positons.Pop();
                        if (currPos < currParent.Children.Count)
                        {
                            yield return currParent.Children[currPos];
                            currParent = currParent.Children[currPos];
                            positons.Push(currPos + 1);
                            positons.Push(0);
                        }
                        else
                        {
                            currParent = currParent.Parent;
                        }
                    }
                }
            }
            yield break;
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
            action(elem);

            if (!(elem is SvgDocument)) //don't apply action to subtree of documents
            {
                foreach (var element in elem.Children)
                {
                    element.ApplyRecursive(action);
                }
            }
        }

        public static void ApplyRecursiveDepthFirst(this SvgElement elem, Action<SvgElement> action)
        {
            if (!(elem is SvgDocument)) //don't apply action to subtree of documents
            {
                foreach (var element in elem.Children)
                {
                    element.ApplyRecursiveDepthFirst(action);
                }
            }

            action(elem);
        }
    }
}
