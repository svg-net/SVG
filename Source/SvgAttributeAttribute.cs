using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg
{
    /// <summary>
    /// Specifies the SVG attribute name of the associated property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Event)]
    public class SvgAttributeAttribute : Attribute
    {
        /// <summary>
        /// Gets a <see cref="string"/> containing the XLink namespace (http://www.w3.org/1999/xlink).
        /// </summary>
        public const string SvgNamespace = "http://www.w3.org/2000/svg";
        public const string XLinkPrefix = "xlink";
        public const string XLinkNamespace = "http://www.w3.org/1999/xlink";
        public const string XmlPrefix = "xml";
        public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

        public static readonly List<KeyValuePair<string, string>> Namespaces = new List<KeyValuePair<string, string>>()
                                                                            {
                                                                                new KeyValuePair<string, string>(string.Empty, SvgNamespace),
                                                                                new KeyValuePair<string, string>(XLinkPrefix, XLinkNamespace),
                                                                                new KeyValuePair<string, string>(XmlPrefix, XmlNamespace)
                                                                            };

        public override bool Equals(object obj)
        {
            if (!(obj is SvgAttributeAttribute))
                return false;

            var indicator = (SvgAttributeAttribute)obj;

            // Always match if either value is string.Empty (wildcard)
            if (indicator.Name == string.Empty)
                return false;

            return string.Equals(Name, indicator.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the name of the SVG attribute.
        /// </summary>
        public string NamespaceAndName
        {
            get
            {
                if (NameSpace == SvgNamespace)
                    return Name;
                return Namespaces.First(x => x.Value == NameSpace).Key + ":" + Name;
            }
        }

        /// <summary>
        /// Gets the name of the SVG attribute.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the namespace of the SVG attribute.
        /// </summary>
        public string NameSpace { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgAttributeAttribute"/> class.
        /// </summary>
        internal SvgAttributeAttribute()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgAttributeAttribute"/> class with the specified attribute name.
        /// </summary>
        /// <param name="name">The name of the SVG attribute.</param>
        internal SvgAttributeAttribute(string name)
        {
            Name = name;
            NameSpace = SvgNamespace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgAttributeAttribute"/> class with the specified SVG attribute name and namespace.
        /// </summary>
        /// <param name="name">The name of the SVG attribute.</param>
        /// <param name="nameSpace">The namespace of the SVG attribute (e.g. http://www.w3.org/2000/svg).</param>
        public SvgAttributeAttribute(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }
    }
}
