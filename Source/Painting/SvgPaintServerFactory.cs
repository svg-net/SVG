using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Svg
{
    internal class SvgPaintServerFactory : TypeConverter
    {
#if !NETSTANDARD20
        private static readonly SvgColourConverter _colourConverter;

        static SvgPaintServerFactory()
        {
            _colourConverter = new SvgColourConverter();
        }
#endif
        public static SvgPaintServer Create(string value, SvgDocument document)
        {
            if (value == null)
                return SvgPaintServer.NotSet;

            var colorValue = value.Trim();
            // If it's pointing to a paint server
            if (string.IsNullOrEmpty(colorValue))
                return SvgPaintServer.NotSet;
            else if (string.Equals(colorValue, "none", StringComparison.OrdinalIgnoreCase))
                // none
                return SvgPaintServer.None;
            else if (string.Equals(colorValue, "currentColor", StringComparison.OrdinalIgnoreCase))
                // currentColor
                return new SvgDeferredPaintServer("currentColor");
            else if (string.Equals(colorValue, "inherit", StringComparison.OrdinalIgnoreCase))
                // inherit
                return SvgPaintServer.Inherit;
            else if (colorValue.StartsWith("url(", StringComparison.OrdinalIgnoreCase))
            {
                var nextIndex = colorValue.IndexOf(')', 4) + 1;
                var id = colorValue.Substring(0, nextIndex);

                colorValue = colorValue.Substring(nextIndex).Trim();
                var fallbackServer = string.IsNullOrEmpty(colorValue) ? SvgPaintServer.None : Create(colorValue, document);
                if (!(fallbackServer is SvgColourServer ||
                    (fallbackServer is SvgDeferredPaintServer && string.Equals(((SvgDeferredPaintServer)fallbackServer).DeferredId, "currentColor"))))
                    fallbackServer = SvgPaintServer.Inherit;

                return new SvgDeferredPaintServer(id, fallbackServer);
            }

#if NETSTANDARD20
            return SvgPaintServer.NotSet;
#else
            // Otherwise try and parse as colour
            return new SvgColourServer((Color)_colourConverter.ConvertFrom(colorValue));
#endif
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
                // check for none
                if (value == SvgPaintServer.None) return "none";
                if (value == SvgPaintServer.Inherit) return "inherit";
                if (value == SvgPaintServer.NotSet) return string.Empty;
#if !NETSTANDARD20
                var colourServer = value as SvgColourServer;
                if (colourServer != null)
                {
                    return new SvgColourConverter().ConvertTo(colourServer.Colour, typeof(string));
                }
#endif
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
    }
}
