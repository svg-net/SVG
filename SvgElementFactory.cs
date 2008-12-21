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
    /// <summary>
    /// Provides the methods required in order to parse and create <see cref="SvgElement"/> instances from XML.
    /// </summary>
    internal class SvgElementFactory
    {
        private static List<ElementInfo> availableElements;

        /// <summary>
        /// Gets a list of available types that can be used when creating an <see cref="SvgElement"/>.
        /// </summary>
        private static List<ElementInfo> AvailableElements
        {
            get
            {
                if (availableElements == null)
                {
                    var svgTypes = from t in typeof(SvgDocument).Assembly.GetExportedTypes()
                                   where t.GetCustomAttributes(typeof(SvgElementAttribute), true).Length > 0
                                   && t.IsSubclassOf(typeof(SvgElement))
                                   select new ElementInfo { ElementName = ((SvgElementAttribute)t.GetCustomAttributes(typeof(SvgElementAttribute), true)[0]).ElementName, ElementType = t };

                    availableElements = svgTypes.ToList();
                }

                return availableElements;
            }
        }

        /// <summary>
        /// Creates an <see cref="SvgDocument"/> from the current node in the specified <see cref="XmlTextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlTextReader"/> containing the node to parse into an <see cref="SvgDocument"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> parameter cannot be <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The CreateDocument method can only be used to parse root &lt;svg&gt; elements.</exception>
        public static SvgDocument CreateDocument(XmlTextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (reader.LocalName != "svg")
            {
                throw new InvalidOperationException("The CreateDocument method can only be used to parse root <svg> elements.");
            }

            return (SvgDocument)CreateElement(reader, true, null);
        }

        /// <summary>
        /// Creates an <see cref="SvgElement"/> from the current node in the specified <see cref="XmlTextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlTextReader"/> containing the node to parse into a subclass of <see cref="SvgElement"/>.</param>
        /// <param name="document">The <see cref="SvgDocument"/> that the created element belongs to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> and <paramref name="document"/> parameters cannot be <c>null</c>.</exception>
        public static SvgElement CreateElement(XmlTextReader reader, SvgDocument document)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            return CreateElement(reader, false, document);
        }

        private static SvgElement CreateElement(XmlTextReader reader, bool fragmentIsDocument, SvgDocument document)
        {
            SvgElement createdElement = null;
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

        /// <summary>
        /// Contains information about a type inheriting from <see cref="SvgElement"/>.
        /// </summary>
        private struct ElementInfo
        {
            /// <summary>
            /// Gets the SVG name of the <see cref="SvgElement"/>.
            /// </summary>
            public string ElementName { get; set; }
            /// <summary>
            /// Gets the <see cref="Type"/> of the <see cref="SvgElement"/> subclass.
            /// </summary>
            public Type ElementType { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ElementInfo"/> struct.
            /// </summary>
            /// <param name="elementName">Name of the element.</param>
            /// <param name="elementType">Type of the element.</param>
            public ElementInfo(string elementName, Type elementType)
                : this()
            {
                this.ElementName = elementName;
                this.ElementType = elementType;
            }
        }
    }
}