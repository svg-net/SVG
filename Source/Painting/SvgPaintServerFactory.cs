using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Svg
{
    internal class SvgPaintServerFactory : TypeConverter
    {
        private static readonly SvgColourConverter _colourConverter;

        static SvgPaintServerFactory()
        {
            _colourConverter = new SvgColourConverter();
        }

        public static SvgPaintServer Create(string value, SvgDocument document)
        {
            if (value == null)
                return SvgPaintServer.NotSet;

            var colorValue = value.Trim();
            // If it's pointing to a paint server
            if (string.IsNullOrEmpty(colorValue))
                return SvgPaintServer.NotSet;
            else if (colorValue.Equals("none", StringComparison.OrdinalIgnoreCase))
                return SvgPaintServer.None;
            else if (colorValue.Equals("currentColor", StringComparison.OrdinalIgnoreCase))
                return new SvgDeferredPaintServer("currentColor");
            else if (colorValue.Equals("inherit", StringComparison.OrdinalIgnoreCase))
                return SvgPaintServer.Inherit;
            else if (colorValue.StartsWith("url(", StringComparison.OrdinalIgnoreCase))
            {
                var nextIndex = colorValue.IndexOf(')', 4) + 1;

                // Malformed url, missing closing parenthesis
                if (nextIndex == 0)
                    return new SvgDeferredPaintServer(colorValue + ")", null);

                var id = colorValue.Substring(0, nextIndex);

                colorValue = colorValue.Substring(nextIndex).Trim();
                var fallbackServer = string.IsNullOrEmpty(colorValue) ? null : Create(colorValue, document);

                return new SvgDeferredPaintServer(id, fallbackServer);
            }
            else if (colorValue.StartsWith("var(", StringComparison.OrdinalIgnoreCase))
            {
                // Find the closing ')' that matches the '(' after 'var'
                var closeIndex = FindMatchingClose(colorValue, 3);
                if (closeIndex < 0)
                    closeIndex = colorValue.Length - 1;

                // Content between 'var(' and its matching ')'
                var inner = colorValue.Substring(4, closeIndex - 4);

                var commaIndex = FindFirstTopLevelComma(inner);
                if (commaIndex < 0)
                {
                    return new SvgCssVariablePaintServer(inner.Trim());
                }
                else
                {
                    var varName = inner.Substring(0, commaIndex).Trim();
                    var fallbackStr = inner.Substring(commaIndex + 1).Trim();
                    var fallback = string.IsNullOrEmpty(fallbackStr) ? null : Create(fallbackStr, document);
                    return new SvgCssVariablePaintServer(varName, fallback);
                }
            }

            // Otherwise try and parse as colour
            return new SvgColourServer((Color)_colourConverter.ConvertFrom(colorValue));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return Create((string)value, (SvgDocument)context);

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                // check for constant
                if (value == SvgPaintServer.None || value == SvgPaintServer.Inherit || value == SvgPaintServer.NotSet)
                    return value.ToString();

                var colourServer = value as SvgColourServer;
                if (colourServer != null)
                {
                    return new SvgColourConverter().ConvertTo(colourServer.Colour, typeof(string));
                }

                var cssVar = value as SvgCssVariablePaintServer;
                if (cssVar != null)
                {
                    return cssVar.ToString();
                }

                var deferred = value as SvgDeferredPaintServer;
                if (deferred != null)
                {
                    return deferred.ToString();
                }

                if (value != null)
                {
                    return string.Format(CultureInfo.InvariantCulture, "url(#{0})", ((SvgPaintServer)value).ID);
                }
                else
                {
                    return "none";
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Returns the index of the <c>)</c> that matches the <c>(</c> at <paramref name="openIndex"/>.
        /// Returns -1 if no matching close is found.
        /// </summary>
        private static int FindMatchingClose(string s, int openIndex)
        {
            var depth = 1;
            for (var i = openIndex + 1; i < s.Length; i++)
            {
                if (s[i] == '(') depth++;
                else if (s[i] == ')') { depth--; if (depth == 0) return i; }
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the first comma in <paramref name="s"/> that is not nested
        /// inside parentheses, or -1 if none is found.
        /// </summary>
        private static int FindFirstTopLevelComma(string s)
        {
            var depth = 0;
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '(') depth++;
                else if (s[i] == ')') depth--;
                else if (s[i] == ',' && depth == 0) return i;
            }
            return -1;
        }
    }
}
