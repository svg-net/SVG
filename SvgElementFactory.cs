using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Threading;
using System.Linq;

using Svg.Transforms;

namespace Svg
{
    internal class SvgElementFactory
    {
        private static List<ElementInfo> availableElements;

        private static List<ElementInfo> AvailableElements
        {
            get
            {
                if (availableElements == null)
                {
                    var svgTypes = from t in typeof(SvgDocument).Assembly.GetExportedTypes()
                                   where t.GetCustomAttributes(typeof(SvgElementAttribute), true).Length > 0
                                   select new ElementInfo { ElementName = ((SvgElementAttribute)t.GetCustomAttributes(typeof(SvgElementAttribute), true)[0]).ElementName, ElementType = t };

                    availableElements = svgTypes.ToList();
                }

                return availableElements;
            }
        }

        public static SvgDocument CreateDocument(XmlTextReader reader)
        {
            return (SvgDocument)CreateElement(reader, true, null);
        }

        public static SvgElement CreateElement(XmlTextReader reader, SvgDocument document)
        {
            return CreateElement(reader, false, document);
        }

        private static SvgElement CreateElement(XmlTextReader reader, bool fragmentIsDocument, SvgDocument document)
        {
            SvgElement createdElement = null;
            SvgFragment fragment;
            string elementName = reader.LocalName;

            Trace.TraceInformation("Begin CreateElement: {0}", elementName);

            if (elementName == "svg")
            {
                createdElement = (fragmentIsDocument) ? new SvgDocument() : new SvgFragment();
            }
            else
            {
                var validTypes = AvailableElements.Where(e => e.ElementName == elementName);

                if (validTypes.Count() > 0)
                {
                    createdElement = Activator.CreateInstance(validTypes.First().ElementType) as SvgElement;
                }
            }

            if (createdElement != null)
            {
                createdElement.ElementName = elementName;
                SetAttributes(createdElement, reader, document);
            }

            Trace.TraceInformation("End CreateElement");

            return createdElement;
        }

        private static void SetAttributes(SvgElement element, XmlTextReader reader, SvgDocument document)
        {
            Trace.TraceInformation("Begin SetAttributes");

            string[] styles = null;
            string[] style = null;
            int i = 0;

            while (reader.MoveToNextAttribute())
            {
                // Special treatment for "style"
                if (reader.LocalName.Equals("style"))
                {
                    styles = reader.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (i = 0; i < styles.Length; i++)
                    {
                        if (!styles[i].Contains(":"))
                        {
                            continue;
                        }

                        style = styles[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        SetPropertyValue(element, style[0].Trim(), style[1].Trim(), document);
                    }

                    continue;
                }

                SetPropertyValue(element, reader.LocalName, reader.Value, document);
            }

            Trace.TraceInformation("End SetAttributes");
        }

        private static void SetPropertyValue(SvgElement element, string attributeName, string attributeValue, SvgDocument document)
        {
            var properties = TypeDescriptor.GetProperties(element.GetType(), new SvgAttributeAttribute[] { new SvgAttributeAttribute(attributeName) });
            PropertyDescriptor descriptor = null;

            if (properties.Count > 0)
            {
                descriptor = properties[0];

                try
                {
                    descriptor.SetValue(element, descriptor.Converter.ConvertFrom(document, CultureInfo.InvariantCulture, attributeValue));
                }
                catch
                {
                    Trace.TraceWarning(string.Format("Attribute '{0}' cannot be set - type '{1}' cannot convert from string '{2}'.", attributeName, descriptor.PropertyType.FullName, attributeValue));
                }
            }
        }

        private struct ElementInfo
        {
            public string ElementName { get; set; }
            public Type ElementType { get; set; }

            public ElementInfo(string elementName, Type elementType)
                : this()
            {
                this.ElementName = elementName;
                this.ElementType = elementType;
            }
        }
    }
}