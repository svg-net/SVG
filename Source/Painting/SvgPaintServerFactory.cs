using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;
using System.Globalization;

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
            if (string.IsNullOrEmpty(value))
            {
                return SvgColourServer.NotSet;
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

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
            	var s = (string) value;
            	if(String.Equals( s.Trim(), "none", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(s) || s.Trim().Length < 1)
            		return SvgPaintServer.None;
            	else
                	return SvgPaintServerFactory.Create(s, (SvgDocument)context);
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

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                //check for none
                if (value == SvgPaintServer.None) return "none";

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
