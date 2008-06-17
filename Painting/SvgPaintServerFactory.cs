using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;

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
            else // Otherwise try and parse as colour
            {
                SvgColourServer server = new SvgColourServer((Color)_colourConverter.ConvertFrom(value.Trim()));
                return server;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return SvgPaintServerFactory.Create((string)value, (SvgDocument)context);
        } 
    }
}
