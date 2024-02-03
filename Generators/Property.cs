using Microsoft.CodeAnalysis;

namespace Svg.Generators
{
    /// <summary>
    /// The SvgElement object property/event.
    /// </summary>
    class Property
    {
        /// <summary>
        /// Gets or sets property/event symbol.
        /// </summary>
        public ISymbol Symbol { get; }

        /// <summary>
        /// Gets or sets property/event symbol member type.
        /// </summary>
        public MemberType MemberType { get; }

        /// <summary>
        /// Gets or sets property/event attribute name.
        /// </summary>
        public string AttributeName { get; }

        /// <summary>
        /// Gets or sets property/event attribute namespace.
        /// </summary>
        public string AttributeNamespace { get; }

        /// <summary>
        /// Gets or sets property/event type converter type string.
        /// </summary>
        public string? Converter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="symbol">The property/event symbol.</param>
        /// <param name="memberType">The property/event symbol member type.</param>
        /// <param name="attributeName">The property/event attribute name.</param>
        /// <param name="attributeNamespace">The property/event attribute namespace.</param>
        /// <param name="converter">The property/event type converter type string.</param>
        public Property(ISymbol symbol, MemberType memberType, string attributeName, string attributeNamespace, string? converter)
        {
            Symbol = symbol;
            MemberType = memberType;
            AttributeName = attributeName;
            AttributeNamespace = attributeNamespace;
            Converter = converter;
        }
    }
}
