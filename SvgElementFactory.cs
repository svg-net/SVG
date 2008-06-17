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

using Svg.Transforms;

namespace Svg
{
    internal class SvgElementFactory
    {
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

            // Parse element
            switch (elementName)
            {
                case "path":
                    createdElement = new SvgPath();
                    break;
                case "linearGradient":
                    createdElement = new SvgLinearGradientServer();
                    break;
                case "radialGradient":
                    createdElement = new SvgRadialGradientServer();
                    break;
                case "pattern":
                    createdElement = new SvgPatternServer();
                    break;
                case "defs":
                    createdElement = new SvgDefinitionList();
                    break;
                case "stop":
                    createdElement = new SvgGradientStop();
                    break;
                case "desc":
                    createdElement = new SvgDescription();
                    break;
                case "clipPath":
                    createdElement = new SvgClipPath();
                    break;
                case "svg":
                    if (!fragmentIsDocument)
                    {
                        fragment = new SvgFragment();
                    }
                    else
                    {
                        fragment = new SvgDocument();
                    }

                    createdElement = (fragmentIsDocument) ? (SvgDocument)fragment : fragment;
                    break;
                case "circle":
                    createdElement = new SvgCircle();
                    break;
                case "ellipse":
                    createdElement = new SvgEllipse();
                    break;
                case "rect":
                    createdElement = new SvgRectangle();
                    break;
                case "line":
                    createdElement = new SvgLine();
                    break;
                case "polyline":
                    createdElement = new SvgPolyline();
                    break;
                case "polygon":
                    createdElement = new SvgPolygon();
                    break;
                case "g":
                    createdElement = new SvgGroup();
                    break;
                case "use":
                    createdElement = new SvgUse();
                    break;
                case "text":
                    createdElement = new SvgText();
                    break;
                default:
                    // Do nothing - unsupported
                    createdElement = null;
                    break;
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
            TypeConverter converter = null;

            if (properties.Count > 0)
            {
                descriptor = properties[0];
                converter = (properties[0].Converter != null) ? properties[0].Converter : TypeDescriptor.GetConverter(descriptor.PropertyType);

                try
                {
                    descriptor.SetValue(element, converter.ConvertFrom(document, CultureInfo.InvariantCulture, attributeValue));
                }
                catch
                {
                    Trace.TraceWarning(string.Format("Attribute '{0}' cannot be set - type '{1}' cannot convert from string '{2}'.", attributeName, descriptor.PropertyType.FullName, attributeValue));
                }
            }
        }
    }
}