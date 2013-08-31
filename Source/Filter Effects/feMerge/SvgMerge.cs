using System;
using System.Collections.Specialized;
using System.Drawing;

namespace Svg.FilterEffects
{

    [SvgElement("feMerge")]
    public class SvgMerge : SvgFilterPrimitive
    {
        public StringCollection MergeResults { get; private set; }

        public SvgMerge()
        {
            MergeResults = new StringCollection();
        }

        public override Bitmap Process()
        {
            //Todo

            //Bitmap merged = new Bitmap((int)this.Owner.Width.Value, (int)this.Owner.Height.Value); 
            //Graphics mergedGraphics = Graphics.FromImage(merged);

            //foreach (string resultId in this.MergeResults)
            //{
            //    mergedGraphics.DrawImageUnscaled(this.Owner.Results[resultId](), new Point(0, 0));
            //}

            //mergedGraphics.Save();
            //mergedGraphics.Dispose();

            //results.Add(this.Result, () => merged);

            return null;
        }


        public override SvgElement DeepCopy()
        {
            throw new NotImplementedException();
        }

    }
}