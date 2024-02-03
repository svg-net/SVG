using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Svg.Generators
{
    /// <summary>
    /// Custom <see cref="Property"/> equality comparer using <see cref="ISymbol"/> for cmparison.
    /// </summary>
    class PropertyEqualityComparer : IEqualityComparer<Property>
    {
        /// <inheritdoc/>
        public bool Equals(Property p1, Property p2)
        {
            return SymbolEqualityComparer.Default.Equals(p1.Symbol, p2.Symbol);
        }

        /// <inheritdoc/>
        public int GetHashCode(Property p)
        {
#pragma warning disable RS1024
            return p.Symbol.GetHashCode();
#pragma warning restore RS1024
        }
    }
}
