using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svg.DataTypes
{

	//implementaton for preserve aspect ratio
	public sealed class SvgPreserveAspectRatioConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
			{
				return new SvgAspectRatio();
			}

			if (!(value is string))
			{
				throw new ArgumentOutOfRangeException("value must be a string.");
			}

			SvgPreserveAspectRatio eAlign = SvgPreserveAspectRatio.none;
			if (!Enum.TryParse<SvgPreserveAspectRatio>(value as string, out eAlign))
				throw new ArgumentOutOfRangeException("value is not a member of SvgPreserveAspectRatio");

			SvgAspectRatio pRet = new SvgAspectRatio(eAlign);
			return (pRet);
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
				return ((SvgUnit)value).ToString();
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
