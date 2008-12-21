using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Svg.FilterEffects
{
    /// <summary>
    /// A filter effect consists of a series of graphics operations that are applied to a given source graphic to produce a modified graphical result.
    /// </summary>
    [SvgElement("filter")]
    public sealed class SvgFilter : SvgElement
    {
        private Bitmap sourceGraphic;
        private Bitmap sourceAlpha;

        /// <summary>
        /// Gets or sets the width of the resulting filter graphic.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
            set { this.Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the resulting filter graphic.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
            set { this.Attributes["height"] = value; }
        }

        internal Dictionary<string, Func<SvgVisualElement, SvgRenderer, Bitmap>> Buffer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFilter"/> class.
        /// </summary>
        public SvgFilter()
        {
            this.Buffer = new Dictionary<string, Func<SvgVisualElement, SvgRenderer, Bitmap>>();
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="SvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> object to render to.</param>
        protected override void Render(SvgRenderer renderer)
        {
            // Do nothing
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return (SvgFilter)this.MemberwiseClone();
        }

        public void ApplyFilter(SvgVisualElement element, SvgRenderer renderer)
        {
            this.Buffer.Clear();
            this.PopulateDefaults(element, renderer);

            IEnumerable<SvgFilterPrimitive> primitives = this.Children.OfType<SvgFilterPrimitive>();

            if (primitives.Count() > 0)
            {
                foreach (var primitive in primitives)
                {
                    this.Buffer.Add(primitive.Result, (e, r) => primitive.Process());
                }

                // Render the final filtered image
                renderer.DrawImageUnscaled(this.Buffer.Last().Value(element, renderer), new Point(0, 0));
            }
        }

        private void PopulateDefaults(SvgVisualElement element, SvgRenderer renderer)
        {
            this.ResetDefaults();

            this.Buffer.Add(SvgFilterPrimitive.SourceGraphic, this.CreateSourceGraphic);
            this.Buffer.Add(SvgFilterPrimitive.SourceAlpha, this.CreateSourceAlpha);
        }

        #region Defaults

        private void ResetDefaults()
        {
            if (this.sourceGraphic != null)
            {
                this.sourceGraphic.Dispose();
                this.sourceGraphic = null;
            }

            if (this.sourceAlpha != null)
            {
                this.sourceAlpha.Dispose();
                this.sourceAlpha = null;
            }
        }

        private Bitmap CreateSourceGraphic(SvgVisualElement element, SvgRenderer renderer)
        {
            if (this.sourceGraphic == null)
            {
                RectangleF bounds = element.Path.GetBounds();
                this.sourceGraphic = new Bitmap((int)bounds.Width, (int)bounds.Height);

                using (var graphics = Graphics.FromImage(this.sourceGraphic))
                {
                    graphics.Clip = renderer.Clip;
                    graphics.Transform = renderer.Transform;

                    element.RenderElement(SvgRenderer.FromGraphics(graphics));

                    graphics.Save();
                }
            }

            return this.sourceGraphic;
        }

        private Bitmap CreateSourceAlpha(SvgVisualElement element, SvgRenderer renderer)
        {
            if (this.sourceAlpha == null)
            {
                Bitmap source = this.Buffer[SvgFilterPrimitive.SourceGraphic](element, renderer);

                float[][] colorMatrixElements = {
                   new float[] {0, 0, 0, 0, 0},        // red
                   new float[] {0, 0, 0, 0, 0},        // green
                   new float[] {0, 0, 0, 0, 0},        // blue
                   new float[] {0, 0, 0, 1, 1},        // alpha
                   new float[] {0, 0, 0, 0, 0} };    // translations

                var matrix = new ColorMatrix(colorMatrixElements);

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix);

                this.sourceAlpha = new Bitmap(source.Width, source.Height);

                using (var graphics = Graphics.FromImage(this.sourceAlpha))
                {

                    graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0,
                          source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                    graphics.Save();
                }
            }

            return this.sourceAlpha;
        }
        #endregion
    }
}