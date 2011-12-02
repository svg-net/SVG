using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Svg.Transforms
{
    [TypeConverter(typeof(SvgTransformConverter))]
    public class SvgTransformCollection : List<SvgTransform>
    {
    	/// <summary>
    	/// Multiplies all matrices
    	/// </summary>
    	/// <returns>The result of all transforms</returns>
    	public Matrix GetMatrix()
    	{
    		var transformMatrix =  new Matrix();
    		
    		// Return if there are no transforms
            if (this.Count == 0)
            {
            	return transformMatrix;
            }

            foreach (SvgTransform transformation in this)
            {
                transformMatrix.Multiply(transformation.Matrix);
            }

            return transformMatrix;
    	}
    		
    }
}
