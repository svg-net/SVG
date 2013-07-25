using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;

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
            using (StringWriter str = new StringWriter())
            using (XmlTextWriter xml = new XmlTextWriter(str))
            {
                elem.WriteElement(xml);
                return str.ToString();

            }
        }
        
        public static bool HasNonEmptyCustomAttribute(this SvgElement element, string name)
        {
        	return element.CustomAttributes.ContainsKey(name) && !string.IsNullOrEmpty(element.CustomAttributes[name]);
        }
    }
}
