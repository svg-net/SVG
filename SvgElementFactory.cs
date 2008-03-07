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
                case "svg":
                    if (!fragmentIsDocument)
                        fragment = new SvgFragment();
                    else
                        fragment = new SvgDocument();

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
                    return null;
            }

            SetAttributes(createdElement, reader, document);

            return createdElement;
        }

        private static void SetAttributes(SvgElement element, XmlTextReader reader, SvgDocument document)
        {
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
        }

        private static void SetPropertyValue(SvgElement element, string attributeName, string attributeValue, SvgDocument document)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(element.GetType(), new SvgAttributeAttribute[] { new SvgAttributeAttribute(attributeName) });
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

        //private static void SetAttributes(SvgElement element, Dictionary<string, string> attributes, SvgDocument document)
        //{
        //    // Parse attributes
        //    foreach(KeyValuePair<string, string> keyValuePair in attributes)
        //    {
        //        string name = keyValuePair.Key;
        //        string value = keyValuePair.Value;

        //        switch (name)
        //        {
        //            case "id":
        //                if (!String.IsNullOrEmpty(value))
        //                    SetProperty(element, name, value);
        //                break;
        //            case "style":
        //                string[] styles = value.Split(';');
        //                Dictionary<string, string> styleAttributes = new Dictionary<string, string>();
        //                foreach (string style in styles)
        //                {
        //                    if (String.IsNullOrEmpty(style) || style.IndexOf(":") == -1)
        //                        continue;

        //                    string[] pair = style.Split(':');
        //                    styleAttributes.Add(pair[0].Trim(), pair[1].Trim());
        //                }
        //                SetAttributes(element, styleAttributes, document);
        //                break;
        //            case "href":
        //                if (element is SvgUse)
        //                    SetProperty(element, name, document.GetElementById(value));
        //                break;
        //            case "transform":
        //                SetProperty(element, name, _transformConverter.ConvertFrom(value));
        //                break;
        //            case "stroke":
        //            case "fill":
        //                SetProperty(element, name, SvgPaintServerFactory.Create(value, document));
        //                break;
        //            case "font":
        //                break;
        //            case "font-family":
        //                // TODO: create font family converter, loop through families list. return generic if it's not in the list
        //                try
        //                {
        //                    SetProperty(element, name, new FontFamily(value));
        //                }
        //                catch
        //                {
        //                    Trace.TraceWarning("\"{0}\" is not a recognised font.", value);
        //                    SetProperty(element, name, FontFamily.GenericSansSerif);
        //                }
        //                break;
        //            case "font-weight":
        //                //SetProperty(createdElement, reader.LocalName, reader.Value);
        //                break;
        //            case "fill-opacity":
        //            case "stroke-opacity":
        //            case "stop-opacity":
        //            case "opacity":
        //                SetProperty(element, name, float.Parse(value));
        //                break;
        //            case "points":
        //                // TODO: TypeConverter for this?
        //                string points = value.Replace(",", " ").Trim();
        //                Regex spaceReplace = new Regex(@"\s+");
        //                points = spaceReplace.Replace(points, " ");
        //                string[] pts = points.Split(' ');
        //                List<SvgUnit> units = new List<SvgUnit>();

        //                foreach (string point in pts)
        //                    units.Add((SvgUnit)_unitConverter.ConvertFrom(point));

        //                SetProperty(element, name, units);
        //                break;
        //            case "font-size":
        //            case "letter-spacing":
        //            case "word-spacing":
        //            case "r":
        //            case "width":
        //            case "height":
        //            case "ry":
        //            case "rx":
        //            case "x":
        //            case "y":
        //            case "x1":
        //            case "y1":
        //            case "x2":
        //            case "y2":
        //            case "cy":
        //            case "cx":
        //            case "offset":
        //            case "stroke-width":
        //                SetProperty(element, name, (SvgUnit)_unitConverter.ConvertFrom(value));
        //                break;
        //            case "stop-color":
        //                SetProperty(element, name, (Color)_colourConverter.ConvertFrom(value));
        //                break;
        //            case "d":
        //                SvgPathBuilder.Parse(value, ((SvgPath)element).PathData);
        //                break;
        //            case "pathLength":
        //                SetProperty(element, name, int.Parse(value));
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        //private static void SetProperty(object element, string attributeName, object attributeValue)
        //{
        //    string key = String.Format("{0}{1}", element.GetType().Name, attributeName);

        //    if (!_propertyDescriptorLookup.ContainsKey(key))
        //    {
        //        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(element.GetType(), new Attribute[] { new SvgAttributeAttribute(attributeName) });

        //        if (properties.Count == 0)
        //            return;

        //        _propertyDescriptorLookup.Add(key, properties[0]);
        //    }

        //    PropertyDescriptor property = _propertyDescriptorLookup[key];
        //    property.SetValue(element, attributeValue);
        //}
    }
}