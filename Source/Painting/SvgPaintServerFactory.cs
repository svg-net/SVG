using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Svg
{
    internal class SvgPaintServerFactory : TypeConverter
    {
        private static readonly SvgColourConverter _colourConverter;
        private static readonly Regex _urlRefPattern;

        static SvgPaintServerFactory()
        {
            _colourConverter = new SvgColourConverter();
            _urlRefPattern = new Regex(@"url\((#[^)]+)\)");
        }

        public static SvgPaintServer Create(string value, SvgDocument document)
        {
            // If it's pointing to a paint server
            if (string.IsNullOrEmpty(value) || value.ToLower().Trim() == "none")
            {
                return new SvgColourServer(Color.Transparent);
            }
            else if (value.IndexOf("url(#") > -1)
            {
                Match match = _urlRefPattern.Match(value);
                Uri id = new Uri(match.Groups[1].Value, UriKind.Relative);
                return (SvgPaintServer)document.IdManager.GetElementById(id);
            }
            // If referenced to to a different (linear or radial) gradient
            else if (document.IdManager.GetElementById(value) != null && document.IdManager.GetElementById(value).GetType().BaseType == typeof(SvgGradientServer))
            {
                return (SvgPaintServer)document.IdManager.GetElementById(value);
            }
            else // Otherwise try and parse as colour
            {
                return new SvgColourServer((Color)_colourConverter.ConvertFrom(value.Trim()));
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                if(s == "none")
                    return null;
                else
                    return Create(s, (SvgDocument)context);
            }

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

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var colourServer = value as SvgColourServer;

                if (colourServer != null)
                {
                    return new SvgColourConverter().ConvertTo(colourServer.Colour, typeof(string));
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
