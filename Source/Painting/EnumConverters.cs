using Svg.DataTypes;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Svg
{
	//just overrrides canconvert and returns true
	public class BaseConverter : TypeConverter
    {
     	
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
    }
	
	public sealed class SvgBoolConverter : BaseConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
			{
				return true;
			}
			
			if (!(value is string))
			{
				throw new ArgumentOutOfRangeException("value must be a string.");
			}

			// Note: currently only used by SvgVisualElement.Visible but if
			// conversion is used elsewhere these checks below will need to change
			string visibility = (string)value;
			if ((visibility == "hidden") || (visibility == "collapse"))
				return false;
			else
				return true;
        }
		
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
            	return ((bool)value) ? "visible" : "hidden";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }	
	}
	
	//converts enums to lower case strings
	public class EnumBaseConverter<T> : BaseConverter
    {
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
			{
				return Activator.CreateInstance(typeof(T));
			}
			
			if (!(value is string))
			{
				throw new ArgumentOutOfRangeException("value must be a string.");
			}

			return (T)Enum.Parse(typeof(T), (string)value, true);
        }
		
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
            	return ((T)value).ToString().ToLower();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }	
	}
	
	//implementation for fill-rule
    public sealed class SvgFillRuleConverter : EnumBaseConverter<SvgFillRule>
    {
    }
    
    //implementaton for clip rule
    public sealed class SvgClipRuleConverter : EnumBaseConverter<SvgClipRule>
    {
    }
    
    //implementaton for clip rule
    public sealed class SvgTextAnchorConverter : EnumBaseConverter<SvgTextAnchor>
    {
    }
    
    public sealed class SvgStrokeLineCapConverter : EnumBaseConverter<SvgStrokeLineCap>
    {
    }

    public sealed class SvgStrokeLineJoinConverter : EnumBaseConverter<SvgStrokeLineJoin>
    {
    }

	public sealed class SvgMarkerUnitsConverter : EnumBaseConverter<SvgMarkerUnits>
	{
	}

    public sealed class SvgFontVariantConverter : EnumBaseConverter<SvgFontVariant>
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.ToString() == "small-caps") return SvgFontVariant.smallcaps;
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgFontVariant && (SvgFontVariant)value == SvgFontVariant.smallcaps)
            {
                return "small-caps";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgFontWeightConverter : EnumBaseConverter<SvgFontWeight>
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                switch ((string)value)
                {
                    case "100": return SvgFontWeight.w100;
                    case "200": return SvgFontWeight.w200;
                    case "300": return SvgFontWeight.w300;
                    case "400": return SvgFontWeight.w400;
                    case "500": return SvgFontWeight.w500;
                    case "600": return SvgFontWeight.w600;
                    case "700": return SvgFontWeight.w700;
                    case "800": return SvgFontWeight.w800;
                    case "900": return SvgFontWeight.w900;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgFontWeight)
            {
                switch ((SvgFontWeight)value)
                {
                    case SvgFontWeight.w100: return "100";
                    case SvgFontWeight.w200: return "200";
                    case SvgFontWeight.w300: return "300";
                    case SvgFontWeight.w400: return "400";
                    case SvgFontWeight.w500: return "500";
                    case SvgFontWeight.w600: return "600";
                    case SvgFontWeight.w700: return "700";
                    case SvgFontWeight.w800: return "800";
                    case SvgFontWeight.w900: return "900";
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public static class Enums 
    {
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
        {
            var retValue = value == null ?
                        false :
                        Enum.IsDefined(typeof(TEnum), value);
            result = retValue ?
                        (TEnum)Enum.Parse(typeof(TEnum), value) :
                        default(TEnum);
            return retValue;
        }
    }
}
