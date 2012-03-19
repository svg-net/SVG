using System;
using System.Drawing;
using System.Collections.Generic;
using Svg.Filter_Effects.feColourMatrix;

namespace Svg.FilterEffects
{
	/// <summary>
	/// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
	/// </summary>
    [SvgElement("feOffset")]
	public class SvgOffset : SvgFilterPrimitive
    {


		/// <summary>
		/// The amount to offset the input graphic along the x-axis. The offset amount is expressed in the coordinate system established by attribute ‘primitiveUnits’ on the ‘filter’ element.
		/// If the attribute is not specified, then the effect is as if a value of 0 were specified.
		/// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
		/// </summary>
		[SvgAttribute("dx")]
		public SvgUnit Dx { get; set; }


		/// <summary>
		/// The amount to offset the input graphic along the y-axis. The offset amount is expressed in the coordinate system established by attribute ‘primitiveUnits’ on the ‘filter’ element.
		/// If the attribute is not specified, then the effect is as if a value of 0 were specified.
		/// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
		/// </summary>
		[SvgAttribute("dy")]
		public string Dy { get; set; }



		public override Bitmap Process()
		{
			return null;
		}




		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgOffset>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgOffset;
			newObj.Dx = this.Dx;
			newObj.Dy = this.Dy;
	
			return newObj;
		}

    }
}