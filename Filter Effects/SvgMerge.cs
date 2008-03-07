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

        public SvgMerge(ISvgFilter owner, string input)
            : base(owner, input)
        {
            this._mergeResults = new StringCollection();
        }

        public override Bitmap Apply()
        {
            Bitmap merged = new Bitmap((int)this.Owner.Width.Value, (int)this.Owner.Height.Value); 
            Graphics mergedGraphics = Graphics.FromImage(merged);

            foreach (string resultId in this.MergeResults)
                mergedGraphics.DrawImageUnscaled(this.Owner.Results[resultId], new Point(0, 0));

            mergedGraphics.Save();
            mergedGraphics.Dispose();

            return merged;
        }
    }
}