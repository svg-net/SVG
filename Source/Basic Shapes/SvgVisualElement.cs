using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Linq;
using Svg.FilterEffects;
using System.Globalization;

namespace Svg
{
    /// <summary>
    /// The class that all SVG elements should derive from when they are to be rendered.
    /// </summary>
    public abstract partial class SvgVisualElement : SvgElement, ISvgBoundable, ISvgStylable, ISvgClipable
    {
        private bool? _requiresSmoothRendering;
        private Region _previousClip;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public abstract GraphicsPath Path(ISvgRenderer renderer);

        PointF ISvgBoundable.Location
        {
            get
            {
                return Bounds.Location;
            }
        }

        SizeF ISvgBoundable.Size
        {
            get
            {
                return Bounds.Size;
            }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// Gets the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        [SvgAttribute("clip")]
        public virtual string Clip
        {
            get { return GetAttribute<string>("clip", Inherited); }
            set { Attributes["clip"] = value; }
        }

        /// <summary>
        /// Gets the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        [SvgAttribute("clip-path")]
        public virtual Uri ClipPath
        {
            get { return GetAttribute<Uri>("clip-path", false); }
            set { Attributes["clip-path"] = value; }
        }

        /// <summary>
        /// Gets or sets the algorithm which is to be used to determine the clipping region.
        /// </summary>
        [SvgAttribute("clip-rule")]
        public SvgClipRule ClipRule
        {
            get { return GetAttribute("clip-rule", false, SvgClipRule.NonZero); }
            set { Attributes["clip-rule"] = value; }
        }

        /// <summary>
        /// Gets the associated <see cref="SvgFilter"/> if one has been specified.
        /// </summary>
        [SvgAttribute("filter")]
        public virtual Uri Filter
        {
            get { return GetAttribute<Uri>("filter", Inherited); }
            set { Attributes["filter"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected virtual bool RequiresSmoothRendering
        {
            get
            {
                if (_requiresSmoothRendering == null)
                    _requiresSmoothRendering = ConvertShapeRendering2AntiAlias(ShapeRendering);

                return _requiresSmoothRendering.Value;
            }
        }

        private bool ConvertShapeRendering2AntiAlias(SvgShapeRendering shapeRendering)
        {
            switch (shapeRendering)
            {
                case SvgShapeRendering.OptimizeSpeed:
                case SvgShapeRendering.CrispEdges:
                case SvgShapeRendering.GeometricPrecision:
                    return false;
                default:
                    // SvgShapeRendering.Auto
                    // SvgShapeRendering.Inherit
                    return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgVisualElement"/> class.
        /// </summary>
        public SvgVisualElement()
        {
            this.IsPathDirty = true;
        }

        protected virtual bool Renderable { get { return true; } }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            this.Render(renderer, true);
        }

        private void Render(ISvgRenderer renderer, bool renderFilter)
        {
            if (this.Visible && this.Displayable && this.PushTransforms(renderer) &&
                (!this.Renderable || this.Path(renderer) != null))
            {
                if (!(renderFilter && this.RenderFilter(renderer)))
                {
                    this.SetClip(renderer);

                    if (this.Renderable)
                    {
                        var opacity = Math.Min(Math.Max(this.Opacity, 0), 1);
                        if (opacity == 1f)
                            this.RenderFillAndStroke(renderer);
                        else
                        {
                            IsPathDirty = true;
                            var bounds = this.Bounds;
                            IsPathDirty = true;

                            using (var canvas = new Bitmap((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height)))
                            {
                                using (var canvasRenderer = SvgRenderer.FromImage(canvas))
                                {
                                    canvasRenderer.SetBoundable(renderer.GetBoundable());
                                    canvasRenderer.TranslateTransform(-bounds.X, -bounds.Y);

                                    this.RenderFillAndStroke(canvasRenderer);
                                }
                                var srcRect = new RectangleF(0f, 0f, bounds.Width, bounds.Height);
                                renderer.DrawImage(canvas, bounds, srcRect, GraphicsUnit.Pixel, opacity);
                            }
                        }
                    }
                    else
                    {
                        base.RenderChildren(renderer);
                    }

                    this.ResetClip(renderer);
                    this.PopTransforms(renderer);
                }
            }
        }

        private bool RenderFilter(ISvgRenderer renderer)
        {
            var rendered = false;

            var filterPath = this.Filter;
            if (filterPath != null)
            {
                var element = this.OwnerDocument.IdManager.GetElementById(filterPath);
                if (element is SvgFilter)
                {
                    this.PopTransforms(renderer);
                    try
                    {
                        ((SvgFilter)element).ApplyFilter(this, renderer, (r) => this.Render(r, false));
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                    }
                    rendered = true;
                }
            }

            return rendered;
        }

        protected void RenderFillAndStroke(ISvgRenderer renderer)
        {
            // If this element needs smoothing enabled turn anti-aliasing on
            if (this.RequiresSmoothRendering)
            {
                renderer.SmoothingMode = SmoothingMode.AntiAlias;
            }

            this.RenderFill(renderer);
            this.RenderStroke(renderer);

            // Reset the smoothing mode
            if (this.RequiresSmoothRendering && renderer.SmoothingMode == SmoothingMode.AntiAlias)
            {
                renderer.SmoothingMode = SmoothingMode.Default;
            }
        }

        /// <summary>
        /// Renders the fill of the <see cref="SvgVisualElement"/> to the specified <see cref="ISvgRenderer"/>
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal virtual void RenderFill(ISvgRenderer renderer)
        {
            if (this.Fill != null)
            {
                using (var brush = this.Fill.GetBrush(this, renderer, Math.Min(Math.Max(this.FillOpacity, 0), 1)))
                {
                    if (brush != null)
                    {
                        this.Path(renderer).FillMode = this.FillRule == SvgFillRule.NonZero ? FillMode.Winding : FillMode.Alternate;
                        renderer.FillPath(brush, this.Path(renderer));
                    }
                }
            }
        }

        /// <summary>
        /// Renders the stroke of the <see cref="SvgVisualElement"/> to the specified <see cref="ISvgRenderer"/>
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal virtual bool RenderStroke(ISvgRenderer renderer)
        {
            if (Stroke != null && Stroke != SvgColourServer.None && StrokeWidth > 0)
            {
                var strokeWidth = StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this);
                using (var brush = Stroke.GetBrush(this, renderer, Math.Min(Math.Max(StrokeOpacity, 0), 1), true))
                {
                    if (brush != null)
                    {
                        var path = Path(renderer);
                        var bounds = path.GetBounds();
                        if (path.PointCount < 1) return false;
                        if (bounds.Width <= 0 && bounds.Height <= 0)
                        {
                            switch (StrokeLineCap)
                            {
                                case SvgStrokeLineCap.Round:
                                    using (var capPath = new GraphicsPath())
                                    {
                                        capPath.AddEllipse(path.PathPoints[0].X - strokeWidth / 2, path.PathPoints[0].Y - strokeWidth / 2, strokeWidth, strokeWidth);
                                        renderer.FillPath(brush, capPath);
                                    }
                                    break;
                                case SvgStrokeLineCap.Square:
                                    using (var capPath = new GraphicsPath())
                                    {
                                        capPath.AddRectangle(new RectangleF(path.PathPoints[0].X - strokeWidth / 2, path.PathPoints[0].Y - strokeWidth / 2, strokeWidth, strokeWidth));
                                        renderer.FillPath(brush, capPath);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            using (var pen = new Pen(brush, strokeWidth))
                            {
                                if (StrokeDashArray != null && StrokeDashArray.Count > 0)
                                {
                                    strokeWidth = strokeWidth <= 0 ? 1 : strokeWidth;
                                    if (StrokeDashArray.Count % 2 != 0)
                                    {
                                        // handle odd dash arrays by repeating them once
                                        StrokeDashArray.AddRange(StrokeDashArray);
                                    }

                                    var dashOffset = StrokeDashOffset != null ? StrokeDashOffset : 0;

                                    /* divide by stroke width - GDI uses stroke width as unit.*/
                                    var dashPattern = StrokeDashArray.Select(u => ((u.ToDeviceValue(renderer, UnitRenderingType.Other, this) <= 0) ? 1 : 
                                        u.ToDeviceValue(renderer, UnitRenderingType.Other, this)) / strokeWidth).ToArray();
                                    int length = dashPattern.Length;

                                    if (StrokeLineCap == SvgStrokeLineCap.Round)
                                    {
                                        // to handle round caps, we have to adapt the dash pattern
                                        // by increasing the dash length by the stroke width - GDI draws the rounded 
                                        // edge inside the dash line, SVG draws it outside the line
                                        var pattern = new float[length];
                                        int offset = 1; // the values are already normalized to dash width
                                        for (int i = 0; i < length; i++)
                                        {
                                            pattern[i] = dashPattern[i] + offset;
                                            if (pattern[i] <= 0)
                                            {
                                                // overlapping caps - remove the gap for simplicity, see #508
                                                if (i < length - 1)
                                                {
                                                    // add the next dash segment to the current one
                                                    dashPattern[i - 1] += dashPattern[i] + dashPattern[i + 1];
                                                    length -= 2;
                                                    for (int k = i; k < length; k++)
                                                        dashPattern[k] = dashPattern[k + 2];

                                                    // and handle the combined segment again
                                                    i -= 2;
                                                }
                                                else if (i > 2)
                                                {
                                                    // add the last dash segment to the first one
                                                    // this will change the start point, so adapt the offset
                                                    var dashLength = dashPattern[i - 1] + dashPattern[i];
                                                    pattern[0] += dashLength;
                                                    length -= 2;
                                                    dashOffset += dashLength * strokeWidth;
                                                }
                                                else
                                                {
                                                    // we have only one dash with the gap too small -
                                                    // do not use dash at all
                                                    length = 0;
                                                    break;
                                                }
                                            }
                                            offset *= -1; // increase dash length, decrease spaces
                                        }
                                        if (length > 0)
                                        {
                                            if (length < dashPattern.Length)
                                                Array.Resize(ref pattern, length);
                                            dashPattern = pattern;
                                            pen.DashCap = DashCap.Round;
                                        }
                                    }

                                    if (length > 0)
                                    {
                                        pen.DashPattern = dashPattern;

                                        if (dashOffset != 0)
                                        {
                                            pen.DashOffset = ((dashOffset.ToDeviceValue(renderer, UnitRenderingType.Other, this) <= 0) ? 1 : 
                                                dashOffset.ToDeviceValue(renderer, UnitRenderingType.Other, this)) / strokeWidth;
                                        }
                                    }
                                }
                                switch (StrokeLineJoin)
                                {
                                    case SvgStrokeLineJoin.Bevel:
                                        pen.LineJoin = LineJoin.Bevel;
                                        break;
                                    case SvgStrokeLineJoin.Round:
                                        pen.LineJoin = LineJoin.Round;
                                        break;
                                    default:
                                        pen.LineJoin = LineJoin.Miter;
                                        break;
                                }
                                pen.MiterLimit = StrokeMiterLimit;
                                switch (StrokeLineCap)
                                {
                                    case SvgStrokeLineCap.Round:
                                        pen.StartCap = LineCap.Round;
                                        pen.EndCap = LineCap.Round;
                                        break;
                                    case SvgStrokeLineCap.Square:
                                        pen.StartCap = LineCap.Square;
                                        pen.EndCap = LineCap.Square;
                                        break;
                                }

                                renderer.DrawPath(pen, path);

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the clipping region of the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region set.</param>
        protected internal virtual void SetClip(ISvgRenderer renderer)
        {
            if (this.ClipPath != null || !string.IsNullOrEmpty(this.Clip))
            {
                this._previousClip = renderer.GetClip();

                if (this.ClipPath != null)
                {
                    SvgClipPath clipPath = this.OwnerDocument.GetElementById<SvgClipPath>(this.ClipPath.ToString());
                    if (clipPath != null) renderer.SetClip(clipPath.GetClipRegion(this, renderer), CombineMode.Intersect);
                }

                var clip = this.Clip;
                if (!string.IsNullOrEmpty(clip) && clip.StartsWith("rect("))
                {
                    clip = clip.Trim();
                    var offsets = (from o in clip.Substring(5, clip.Length - 6).Split(',')
                                   select float.Parse(o.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture)).ToList();
                    var bounds = this.Bounds;
                    var clipRect = new RectangleF(bounds.Left + offsets[3], bounds.Top + offsets[0],
                                                  bounds.Width - (offsets[3] + offsets[1]),
                                                  bounds.Height - (offsets[2] + offsets[0]));
                    renderer.SetClip(new Region(clipRect), CombineMode.Intersect);
                }
            }
        }

        /// <summary>
        /// Resets the clipping region of the specified <see cref="ISvgRenderer"/> back to where it was before the <see cref="SetClip"/> method was called.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region reset.</param>
        protected internal virtual void ResetClip(ISvgRenderer renderer)
        {
            if (this._previousClip != null)
            {
                renderer.SetClip(this._previousClip);
                this._previousClip = null;
            }
        }

        /// <summary>
        /// Sets the clipping region of the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region set.</param>
        void ISvgClipable.SetClip(ISvgRenderer renderer)
        {
            this.SetClip(renderer);
        }

        /// <summary>
        /// Resets the clipping region of the specified <see cref="ISvgRenderer"/> back to where it was before the <see cref="SetClip"/> method was called.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region reset.</param>
        void ISvgClipable.ResetClip(ISvgRenderer renderer)
        {
            this.ResetClip(renderer);
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgVisualElement;
            newObj.ClipPath = this.ClipPath;
            newObj.ClipRule = this.ClipRule;
            newObj.Filter = this.Filter;

            newObj.Visible = this.Visible;
            if (this.Fill != null)
                newObj.Fill = this.Fill;
            if (this.Stroke != null)
                newObj.Stroke = this.Stroke;
            newObj.FillRule = this.FillRule;
            newObj.FillOpacity = this.FillOpacity;
            newObj.StrokeWidth = this.StrokeWidth;
            newObj.StrokeLineCap = this.StrokeLineCap;
            newObj.StrokeLineJoin = this.StrokeLineJoin;
            newObj.StrokeMiterLimit = this.StrokeMiterLimit;
            newObj.StrokeDashArray = this.StrokeDashArray;
            newObj.StrokeDashOffset = this.StrokeDashOffset;
            newObj.StrokeOpacity = this.StrokeOpacity;
            newObj.Opacity = this.Opacity;

            return newObj;
        }
    }
}
