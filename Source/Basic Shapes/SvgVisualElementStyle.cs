using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Svg
{
    public abstract partial class SvgVisualElement
    {
        private static float FixOpacityValue(float value)
        {
            const float max = 1.0f;
            const float min = 0.0f;
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// </summary>
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return (this.Attributes["Visible"] == null) ? true : (bool)this.Attributes["Visible"]; }
            set { this.Attributes["Visible"] = value; }
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("fill")]
        public virtual SvgPaintServer Fill
        {
            get { return (this.Attributes["Fill"] == null) ? new SvgColourServer() : (SvgPaintServer)this.Attributes["Fill"]; }
            set { this.Attributes["Fill"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SvgPaintServer"/> to be used when rendering a stroke around this element.
        /// </summary>
        [SvgAttribute("stroke")]
        public virtual SvgPaintServer Stroke
        {
            get { return (this.Attributes["Stroke"] == null) ? null : (SvgPaintServer)this.Attributes["Stroke"]; }
            set { this.Attributes["Stroke"] = value; }
        }

        [SvgAttribute("fill-rule")]
        public virtual SvgFillRule FillRule
        {
            get { return (this.Attributes["FillRule"] == null) ? SvgFillRule.NonZero : (SvgFillRule)this.Attributes["FillRule"]; }
            set { this.Attributes["FillRule"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of this element's <see cref="Fill"/>.
        /// </summary>
        [SvgAttribute("fill-opacity")]
        public virtual float FillOpacity
        {
            get { return (this.Attributes["FillOpacity"] == null) ? this.Opacity : (float)this.Attributes["FillOpacity"]; }
            set { this.Attributes["FillOpacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the width of the stroke (if the <see cref="Stroke"/> property has a valid value specified.
        /// </summary>
        [SvgAttribute("stroke-width")]
        public virtual SvgUnit StrokeWidth
        {
            get { return (this.Attributes["StrokeWidth"] == null) ? new SvgUnit(1.0f) : (SvgUnit)this.Attributes["StrokeWidth"]; }
            set { this.Attributes["StrokeWidth"] = value; }
        }

        [SvgAttribute("stroke-linecap")]
        public virtual SvgStrokeLineCap StrokeLineCap
        {
            get { return (this.Attributes["StrokeLineCap"] == null) ? SvgStrokeLineCap.Butt : (SvgStrokeLineCap)this.Attributes["StrokeLineCap"]; }
            set { this.Attributes["StrokeLineCap"] = value; }
        }

        [SvgAttribute("stroke-linejoin")]
        public virtual SvgStrokeLineJoin StrokeLineJoin
        {
            get { return (this.Attributes["StrokeLineJoin"] == null) ? SvgStrokeLineJoin.Miter : (SvgStrokeLineJoin)this.Attributes["StrokeLineJoin"]; }
            set { this.Attributes["StrokeLineJoin"] = value; }
        }

        [SvgAttribute("stroke-miterlimit")]
        public virtual float StrokeMiterLimit
        {
            get { return (this.Attributes["StrokeMiterLimit"] == null) ? 4.0f : (float)this.Attributes["StrokeMiterLimit"]; }
            set { this.Attributes["StrokeMiterLimit"] = value; }
        }

        [SvgAttribute("stroke-dasharray")]
        public virtual SvgUnitCollection StrokeDashArray
        {
            get { return this.Attributes["StrokeDashArray"] as SvgUnitCollection; }
            set { this.Attributes["StrokeDashArray"] = value; }
        }

        [SvgAttribute("stroke-dashoffset")]
        public virtual SvgUnit StrokeDashOffset
        {
            get { return (this.Attributes["StrokeDashOffset"] == null) ? SvgUnit.Empty : (SvgUnit)this.Attributes["StrokeDashOffset"]; }
            set { this.Attributes["StrokeDashOffset"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the stroke, if the <see cref="Stroke"/> property has been specified. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("stroke-opacity")]
        public virtual float StrokeOpacity
        {
            get { return (this.Attributes["StrokeOpacity"] == null) ? this.Opacity : (float)this.Attributes["StrokeOpacity"]; }
            set { this.Attributes["StrokeOpacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the opacity of the element. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("opacity")]
        public virtual float Opacity
        {
            get { return (this.Attributes["Opacity"] == null) ? 1.0f : (float)this.Attributes["Opacity"]; }
            set { this.Attributes["Opacity"] = FixOpacityValue(value); }
        }
    }
}