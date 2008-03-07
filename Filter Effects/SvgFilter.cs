using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    public class SvgFilter : SvgElement, ISvgFilter
    {
        private readonly List<SvgFilterPrimitive> _primitives;
        private readonly Dictionary<string, Bitmap> _results;
        private SvgUnit _width;
        private SvgUnit _height;

        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this._width; }
            set { this._width = value; }
        }

        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this._height; }
            set { this._height = value; }
        }

        public List<SvgFilterPrimitive> Primitives
        {
            get { return this._primitives; }
        }

        public Dictionary<string, Bitmap> Results
        {
            get { return this._results; }
        }

        public SvgFilter()
        {
            this._primitives = new List<SvgFilterPrimitive>();
            this._results = new Dictionary<string, Bitmap>();
        }

        protected override void Render(Graphics graphics)
        {
            // Do nothing
        }

        public override object Clone()
        {
            return (SvgFilter)this.MemberwiseClone();
        }

        public void ApplyFilter(Bitmap sourceGraphic, Graphics renderer)
        {
            // A bit inefficient to create all the defaults when they may not even be used
            this.PopulateDefaults(sourceGraphic);
            Bitmap result = null;

            foreach (SvgFilterPrimitive primitive in this.Primitives)
            {
                result = primitive.Apply();
                // Add the result to the dictionary for use by other primitives
                this._results.Add(primitive.Result, result);
            }

            // Render the final filtered image
            renderer.DrawImageUnscaled(result, new Point(0,0));
        }

        private void PopulateDefaults(Bitmap sourceGraphic)
        {
            // Source graphic
            //this._results.Add("SourceGraphic", sourceGraphic);

            // Source alpha
            //this._results.Add("SourceAlpha", ImageProcessor.GetAlphaChannel(sourceGraphic));
        }
    }
}