using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Svg.Generators
{
    /// <summary>
    /// The SvgElement object.
    /// </summary>
    class Element
    {
        /// <summary>
        /// Gets or sets element type symbol.
        /// </summary>
        public INamedTypeSymbol Symbol { get; }

        /// <summary>
        /// Gets or sets element name.
        /// </summary>
        public string? ElementName { get; }

        /// <summary>
        /// Gets or sets classes that use element name.
        /// </summary>
        public List<string> ClassNames { get; }

        /// <summary>
        /// Gets or sets element properties list.
        /// </summary>
        public List<Property> Properties { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="symbol">The element type symbol.</param>
        /// <param name="elementName">The element name.</param>
        /// <param name="classNames">The classes that use element name.</param>
        /// <param name="properties">The element properties list.</param>
        public Element(INamedTypeSymbol symbol, string? elementName, List<string> classNames, List<Property> properties)
        {
            Symbol = symbol;
            ElementName = elementName;
            ClassNames = classNames;
            Properties = properties;
        }
    }
}
