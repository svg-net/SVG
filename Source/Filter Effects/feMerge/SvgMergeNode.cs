using System;
using System.Drawing;

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