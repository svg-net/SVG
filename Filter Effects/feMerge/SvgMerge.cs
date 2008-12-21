using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Svg.FilterEffects
{
    public class SvgMerge : SvgFilterPrimitive
    {
        private StringCollection _mergeResults;

        public StringCollection MergeResults
        {
            get { return this._mergeResults; }
        }

        public override Bitmap Process()
        {
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
    }
}