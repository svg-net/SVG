using System;

namespace Svg
{
    /// <summary>
    /// A paint server that resolves a CSS custom property (CSS variable) referenced via the
    /// <c>var()</c> function, e.g. <c>var(--my-color)</c> or <c>var(--my-color, red)</c>.
    /// The variable is looked up by walking the element's ancestor chain at render time,
    /// checking every style specificity level so that properties defined in external
    /// stylesheets are found as well as inline ones.
    /// </summary>
    public partial class SvgCssVariablePaintServer : SvgPaintServer
    {
        /// <summary>
        /// Gets or sets the CSS custom property name, including the leading '<c>--</c>'.
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Gets the fallback <see cref="SvgPaintServer"/> used when the variable cannot be
        /// resolved, or <c>null</c> if no fallback was specified.
        /// </summary>
        public SvgPaintServer FallbackServer { get; private set; }

        /// <summary>
        /// Initialises a new, empty instance (required by <see cref="SvgElement.DeepCopy{T}"/>).
        /// </summary>
        public SvgCssVariablePaintServer()
        {
        }

        /// <summary>
        /// Initialises a new instance with the given variable name and no fallback.
        /// </summary>
        /// <param name="variableName">
        /// The CSS custom property name, including the leading '<c>--</c>' (e.g. <c>--my-color</c>).
        /// </param>
        public SvgCssVariablePaintServer(string variableName)
            : this(variableName, null)
        {
        }

        /// <summary>
        /// Initialises a new instance with the given variable name and fallback.
        /// </summary>
        /// <param name="variableName">
        /// The CSS custom property name, including the leading '<c>--</c>' (e.g. <c>--my-color</c>).
        /// </param>
        /// <param name="fallbackServer">
        /// The paint server to use when the variable cannot be resolved, or <c>null</c>.
        /// </param>
        public SvgCssVariablePaintServer(string variableName, SvgPaintServer fallbackServer)
        {
            VariableName = variableName;
            FallbackServer = fallbackServer;
        }

        /// <summary>
        /// Resolves the CSS custom property by walking <paramref name="element"/>'s ancestor
        /// chain (self included) and returns the first <see cref="SvgPaintServer"/> the value
        /// maps to. Falls back to <see cref="FallbackServer"/> (or <see cref="SvgPaintServer.None"/>)
        /// when the property is not defined anywhere in the hierarchy.
        /// </summary>
        /// <remarks>
        /// Circular variable references (e.g. <c>--a: var(--a)</c>) are not detected and will
        /// cause a <see cref="StackOverflowException"/>. Such declarations are invalid per the
        /// CSS specification and the variable is treated as unset by conforming browsers.
        /// </remarks>
        public SvgPaintServer Resolve(SvgElement element)
        {
            if (element == null || string.IsNullOrEmpty(VariableName))
                return FallbackServer ?? None;

            var document = element.OwnerDocument;

            foreach (var ancestor in element.ParentsAndSelf)
            {
                if (ancestor.TryGetCustomProperty(VariableName, out var rawValue))
                {
                    var trimmed = rawValue?.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        return SvgPaintServerFactory.Create(trimmed, document);
                }
            }

            return FallbackServer ?? None;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (FallbackServer == null)
                return string.Concat("var(", VariableName, ")");

            return string.Concat("var(", VariableName, ", ", FallbackServer.ToString(), ")");
        }

        /// <inheritdoc/>
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgCssVariablePaintServer>();
        }

        /// <inheritdoc/>
        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgCssVariablePaintServer;
            newObj.VariableName = VariableName;
            newObj.FallbackServer = FallbackServer?.DeepCopy() as SvgPaintServer;
            return newObj;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var other = obj as SvgCssVariablePaintServer;
            if (other == null)
                return false;

            return string.Equals(VariableName, other.VariableName, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return VariableName == null ? 0 : VariableName.GetHashCode();
        }
    }
}
