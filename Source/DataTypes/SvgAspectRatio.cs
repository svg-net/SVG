using System;
using System.ComponentModel;

namespace Svg
{
	/// <summary>
	/// Description of SvgAspectRatio.
	/// </summary>
	public class SvgAspectRatio
	{
		public SvgAspectRatio()
		{
		}
		
		public SvgAspectRatio(SvgPreserveAspectRatio align)
			: this(align, false)
		{
		}
		
		public SvgAspectRatio(SvgPreserveAspectRatio align, bool slice)
		{
			this.Align = align;
			this.Slice = slice;
		}
		
		public SvgPreserveAspectRatio Align
		{
			get;
			set;
		}
		
		public bool Slice
		{
			get;
			set;
		}
		
		public override string ToString()
		{
			return TypeDescriptor.GetConverter(typeof(SvgPreserveAspectRatio)).ConvertToString(this.Align) + (Slice ? " slice" : "");
		}

	}
	
	[TypeConverter(typeof(SvgPreserverAspectRatioConverter))]
	public enum SvgPreserveAspectRatio
	{
		XMidYMid, //default
		None,
		XMinYMin,
		XMidYMin,
		XMaxYMin,
		XMinYMid,
		XMaxYMid,
		XMinYMax,
		XMidYMax,
		XMaxYMax
	}
}
