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
			
			return (string)value == "visible" ? true : false;
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
}
