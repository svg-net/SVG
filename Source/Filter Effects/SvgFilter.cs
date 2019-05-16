using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Svg.DataTypes;

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
        /// Gets or sets the coordinate interpretation mode.
        /// </summary>
        [SvgAttribute("filterUnits")]
        public SvgCoordinateUnits FilterUnits
        {
            get { return Attributes.GetAttribute("filterUnits", SvgCoordinateUnits.ObjectBoundingBox); }
            set { Attributes["filterUnits"] = value; }
        }

        /// <summary>
        /// Gets or sets the position where the left point of the filter.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
            set { this.Attributes["x"] = value; }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the filter.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
            set { this.Attributes["y"] = value; }
        }

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

        /// <summary>
        /// Gets or sets the color-interpolation-filters of the resulting filter graphic.
        /// NOT currently mapped through to bitmap
        /// </summary>
        [SvgAttribute("color-interpolation-filters")]
        public SvgColourInterpolation ColorInterpolationFilters
        {
            get { return this.Attributes.GetAttribute<SvgColourInterpolation>("color-interpolation-filters"); }
            set { this.Attributes["color-interpolation-filters"] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFilter"/> class.
        /// </summary>
        public SvgFilter()
        {
            X = new SvgUnit(SvgUnitType.Percentage, -10f);
            Y = new SvgUnit(SvgUnitType.Percentage, -10f);
            Width = new SvgUnit(SvgUnitType.Percentage, 120f);
            Height = new SvgUnit(SvgUnitType.Percentage, 120f);
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            base.RenderChildren(renderer);
        }

        private Matrix GetTransform(SvgVisualElement element)
        {
            var transformMatrix = new Matrix();
            foreach (var transformation in element.Transforms)
            {
                transformMatrix.Multiply(transformation.Matrix);
            }
            return transformMatrix;
        }

        private RectangleF GetPathBounds(SvgVisualElement element, ISvgRenderer renderer, Matrix transform)
        {
            var bounds = element.Path(renderer).GetBounds();
            var pts = new PointF[] { bounds.Location, new PointF(bounds.Right, bounds.Bottom) };
            transform.TransformPoints(pts);

            return new RectangleF(Math.Min(pts[0].X, pts[1].X), Math.Min(pts[0].Y, pts[1].Y),
                                  Math.Abs(pts[0].X - pts[1].X), Math.Abs(pts[0].Y - pts[1].Y));
        }

        private RectangleF GetClipRect(RectangleF bounds, ISvgRenderer renderer)
        {
            var xScale = FilterUnits == SvgCoordinateUnits.ObjectBoundingBox ? bounds.Width : 1;
            var yScale = FilterUnits == SvgCoordinateUnits.ObjectBoundingBox ? bounds.Height : 1;

            float x = xScale * X.NormalizeUnit(FilterUnits).ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
            float y = yScale * Y.NormalizeUnit(FilterUnits).ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
            float width = xScale * Width.NormalizeUnit(FilterUnits).ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
            float height = yScale * Height.NormalizeUnit(FilterUnits).ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

            if (FilterUnits == SvgCoordinateUnits.ObjectBoundingBox)
            {
                x += bounds.X;
                y += bounds.Y;
            }
            return new RectangleF(x, y, width, height);
        }

        public void ApplyFilter(SvgVisualElement element, ISvgRenderer renderer, Action<ISvgRenderer> renderMethod)
        {
            var inflate = 0.5f;
            var transform = GetTransform(element);
            var bounds = GetPathBounds(element, renderer, transform);

            if (bounds.Width == 0 || bounds.Height == 0)
                return;

            var buffer = new ImageBuffer(bounds, inflate, renderer, renderMethod) { Transform = transform };

            foreach (var primitive in this.Children.OfType<SvgFilterPrimitive>())
            {
                primitive.Process(buffer);
            }

            // Render the final filtered image
            var bufferImg = buffer.Buffer;
            var imgDraw = RectangleF.Inflate(bounds, inflate * bounds.Width, inflate * bounds.Height);
            var prevClip = renderer.GetClip();
            renderer.SetClip(new Region(GetClipRect(bounds, renderer)));
            renderer.DrawImage(bufferImg, imgDraw, new RectangleF(bounds.X, bounds.Y, imgDraw.Width, imgDraw.Height), GraphicsUnit.Pixel);
            renderer.SetClip(prevClip);
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

        #endregion

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFilter>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFilter;
            newObj.Height = this.Height;
            newObj.Width = this.Width;
            newObj.X = this.X;
            newObj.Y = this.Y;
            newObj.ColorInterpolationFilters = this.ColorInterpolationFilters;
            return newObj;
        }
    }
}
