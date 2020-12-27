using System;
using System.Collections.Generic;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Specifies the SVG name of an <see cref="SvgElement"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NonSvgElementAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the SVG element.
        /// </summary>
        public string ElementName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonSvgElementAttribute"/> class with the specified element name;
        /// </summary>
        /// <param name="elementName">The name of the non SVG element.</param>
        public NonSvgElementAttribute(string elementName)
        {
            this.ElementName = elementName;
        }
    }
}
