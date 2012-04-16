using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Svg.FilterEffects
{

	[SvgElement("feMergeNode")]
    public class SvgMergeNode : SvgFilterPrimitive
    {
        public override Bitmap Process()
        {
            //Todo

            return null;
        }

		public override SvgElement DeepCopy()
		{
			throw new NotImplementedException();
		}

    }
}