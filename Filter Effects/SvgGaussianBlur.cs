using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public class SvgGaussianBlur : SvgFilterPrimitive
    {
        private float _standardDeviation;

        public SvgGaussianBlur(ISvgFilter owner, string inputGraphic)
            : base(owner, inputGraphic)
        {
            
        }

        /// <summary>
        /// The standard deviation for the blur operation.
        /// </summary>
        public float StandardDeviation
        {
            get { return this._standardDeviation; }
            set { this._standardDeviation = value; }
        }

        public override Bitmap Apply()
        {
            Bitmap source = this.Owner.Results[this.In];
            Bitmap blur = new Bitmap(source.Width, source.Height);



            return source;
        }
    }
}