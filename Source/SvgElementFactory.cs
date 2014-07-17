using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Svg
{
    /// <summary>
    /// Provides the methods required in order to parse and create <see cref="SvgElement"/> instances from XML.
    /// </summary>
    internal class SvgElementFactory
    {
        private static List<ElementInfo> availableElements;
        private const string svgNS = "http://www.w3.org/2000/svg";

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
        public static T CreateDocument<T>(XmlTextReader reader) where T : SvgDocument, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (reader.LocalName != "svg")
            {
                throw new InvalidOperationException("The CreateDocument method can only be used to parse root <svg> elements.");
            }

            return (T)CreateElement<T>(reader, true, null);
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

            return CreateElement<SvgDocument>(reader, false, document);
        }

        private static SvgElement CreateElement<T>(XmlTextReader reader, bool fragmentIsDocument, SvgDocument document)  where T : SvgDocument, new()
        {
            SvgElement createdElement = null;
            string elementName = reader.LocalName;
            string elementNS = reader.NamespaceURI;

            //Trace.TraceInformation("Begin CreateElement: {0}", elementName);

            if (elementNS == svgNS)
            {
                if (elementName == "svg")
                {
                    createdElement = (fragmentIsDocument) ? new T() : new SvgFragment();
                }
                else
                {
                    ElementInfo validType = AvailableElements.SingleOrDefault(e => e.ElementName == elementName);
                    if (validType != null)
                    {
                        createdElement = (SvgElement) Activator.CreateInstance(validType.ElementType);
                    }
                    else
                    {
                        createdElement = new SvgUnknownElement(elementName);
                    }
                }

                if (createdElement != null)
                {
                    SetAttributes(createdElement, reader, document);
                }
            }
            else
            {
                // All non svg element (html, ...)
                createdElement = new NonSvgElement(elementName);
                SetAttributes(createdElement, reader, document);
            }

            //Trace.TraceInformation("End CreateElement");

            return createdElement;
        }

        private static void SetAttributes(SvgElement element, XmlTextReader reader, SvgDocument document)
        {
            //Trace.TraceInformation("Begin SetAttributes");

            string[] styles = null;
            string[] style = null;
            int i = 0;

            while (reader.MoveToNextAttribute())
            {
                // Special treatment for "style"
                if (reader.LocalName.Equals("style") && !(element is NonSvgElement))
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

					//defaults for text can come from the document
					if (element.ElementName == "text")
					{
						if (!styles.Contains("font-size") && document.CustomAttributes.ContainsKey("font-size") && document.CustomAttributes["font-size"] != null)
						{
							SetPropertyValue(element, "font-size", document.CustomAttributes["font-size"], document);
						}
						if (!styles.Contains("font-family") && document.CustomAttributes.ContainsKey("font-family") && document.CustomAttributes["font-family"] != null)
						{
							SetPropertyValue(element, "font-family", document.CustomAttributes["font-family"], document);
						}
						
					}
                    continue; 
                }

                SetPropertyValue(element, reader.LocalName, reader.Value, document);
            }

            //Trace.TraceInformation("End SetAttributes");
        }

        private static Dictionary<Type, Dictionary<string, PropertyDescriptorCollection>> _propertyDescriptors = new Dictionary<Type, Dictionary<string, PropertyDescriptorCollection>>();
        private static object syncLock = new object();

        private static void SetPropertyValue(SvgElement element, string attributeName, string attributeValue, SvgDocument document)
        {
            var elementType = element.GetType();

            PropertyDescriptorCollection properties;
            lock (syncLock)
            {
                if (_propertyDescriptors.Keys.Contains(elementType))
                {
                    if (_propertyDescriptors[elementType].Keys.Contains(attributeName))
                    {
                        properties = _propertyDescriptors[elementType][attributeName];
                    }
                    else
                    {
                        properties = TypeDescriptor.GetProperties(elementType, new[] { new SvgAttributeAttribute(attributeName) });
                        _propertyDescriptors[elementType].Add(attributeName, properties);
                    }
                }
                else
                {
                    properties = TypeDescriptor.GetProperties(elementType, new[] { new SvgAttributeAttribute(attributeName) });
                    _propertyDescriptors.Add(elementType, new Dictionary<string, PropertyDescriptorCollection>());

                    _propertyDescriptors[elementType].Add(attributeName, properties);
                } 
            }

            if (properties.Count > 0)
            {
                PropertyDescriptor descriptor = properties[0];

                try
                {
					if (attributeName == "opacity" && attributeValue == "undefined")
					{
						attributeValue = "1";
					}

					descriptor.SetValue(element, descriptor.Converter.ConvertFrom(document, CultureInfo.InvariantCulture, attributeValue));
					

                }
                catch
                {
                    Trace.TraceWarning(string.Format("Attribute '{0}' cannot be set - type '{1}' cannot convert from string '{2}'.", attributeName, descriptor.PropertyType.FullName, attributeValue));
                }
            }
            else
            {
                //check for namespace declaration in svg element
                if (string.Equals(element.ElementName, "svg", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(attributeName, "xmlns", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(attributeName, "xlink", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(attributeName, "xmlns:xlink", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(attributeName, "version", StringComparison.OrdinalIgnoreCase))
                    {
                        //nothing to do
                    }
                    else
                    {
                        //attribute is not a svg attribute, store it in custom attributes
                        element.CustomAttributes[attributeName] = attributeValue;
                    }
                }
                else
                {
                    //attribute is not a svg attribute, store it in custom attributes
                    element.CustomAttributes[attributeName] = attributeValue;
                }
            }
        }

        /// <summary>
        /// Contains information about a type inheriting from <see cref="SvgElement"/>.
        /// </summary>
        [DebuggerDisplay("{ElementName}, {ElementType}")]
        internal sealed class ElementInfo
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
            {
                this.ElementName = elementName;
                this.ElementType = elementType;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ElementInfo"/> class.
            /// </summary>
            public ElementInfo()
            {
            }
        }
    }
}