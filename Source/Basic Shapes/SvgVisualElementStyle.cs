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
        [TypeConverter(typeof(SvgBoolConverter))]
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return (this.Attributes["visibility"] == null) ? true : (bool)this.Attributes["visibility"]; }
            set { this.Attributes["visibility"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// Needed to support SVG attribute display="none"
        /// </summary>
        [SvgAttribute("display")]
        public virtual string Display
        {
            get { return this.Attributes["display"] as string; }
            set { this.Attributes["display"] = value; }
        }

        // Displayable - false if attribute display="none", true otherwise
        protected virtual bool Displayable
        {
            get
            {
                string checkForDisplayNone = this.Attributes["display"] as string;
                if ((!string.IsNullOrEmpty(checkForDisplayNone)) && (checkForDisplayNone == "none"))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("fill")]
        public virtual SvgPaintServer Fill
        {
            get { return (this.Attributes["fill"] == null) ? SvgColourServer.NotSet : (SvgPaintServer)this.Attributes["fill"]; }
            set { this.Attributes["fill"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SvgPaintServer"/> to be used when rendering a stroke around this element.
        /// </summary>
        [SvgAttribute("stroke")]
        public virtual SvgPaintServer Stroke
        {
            get { return (this.Attributes["stroke"] == null) ? null : (SvgPaintServer)this.Attributes["stroke"]; }
            set { this.Attributes["stroke"] = value; }
        }

        [SvgAttribute("fill-rule")]
        public virtual SvgFillRule FillRule
        {
            get { return (this.Attributes["fill-rule"] == null) ? SvgFillRule.NonZero : (SvgFillRule)this.Attributes["fill-rule"]; }
            set { this.Attributes["fill-rule"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of this element's <see cref="Fill"/>.
        /// </summary>
        [SvgAttribute("fill-opacity")]
        public virtual float FillOpacity
        {
            get { return (this.Attributes["fill-opacity"] == null) ? this.Opacity : (float)this.Attributes["fill-opacity"]; }
            set { this.Attributes["fill-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the width of the stroke (if the <see cref="Stroke"/> property has a valid value specified.
        /// </summary>
        [SvgAttribute("stroke-width")]
        public virtual SvgUnit StrokeWidth
        {
            get { return (this.Attributes["stroke-width"] == null) ? new SvgUnit(1.0f) : (SvgUnit)this.Attributes["stroke-width"]; }
            set { this.Attributes["stroke-width"] = value; }
        }

        [SvgAttribute("stroke-linecap")]
        public virtual SvgStrokeLineCap StrokeLineCap
        {
            get { return (this.Attributes["stroke-linecap"] == null) ? SvgStrokeLineCap.Butt : (SvgStrokeLineCap)this.Attributes["stroke-linecap"]; }
            set { this.Attributes["stroke-linecap"] = value; }
        }

        [SvgAttribute("stroke-linejoin")]
        public virtual SvgStrokeLineJoin StrokeLineJoin
        {
            get { return (this.Attributes["stroke-linejoin"] == null) ? SvgStrokeLineJoin.Miter : (SvgStrokeLineJoin)this.Attributes["stroke-linejoin"]; }
            set { this.Attributes["stroke-linejoin"] = value; }
        }

        [SvgAttribute("stroke-miterlimit")]
        public virtual float StrokeMiterLimit
        {
            get { return (this.Attributes["stroke-miterlimit"] == null) ? 4.0f : (float)this.Attributes["stroke-miterlimit"]; }
            set { this.Attributes["stroke-miterlimit"] = value; }
        }

        [SvgAttribute("stroke-dasharray")]
        public virtual SvgUnitCollection StrokeDashArray
        {
            get { return this.Attributes["stroke-dasharray"] as SvgUnitCollection; }
            set { this.Attributes["stroke-dasharray"] = value; }
        }

        [SvgAttribute("stroke-dashoffset")]
        public virtual SvgUnit StrokeDashOffset
        {
            get { return (this.Attributes["stroke-dashoffset"] == null) ? SvgUnit.Empty : (SvgUnit)this.Attributes["stroke-dashoffset"]; }
            set { this.Attributes["stroke-dashoffset"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the stroke, if the <see cref="Stroke"/> property has been specified. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("stroke-opacity")]
        public virtual float StrokeOpacity
        {
            get { return (this.Attributes["stroke-opacity"] == null) ? this.Opacity : (float)this.Attributes["stroke-opacity"]; }
            set { this.Attributes["stroke-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the opacity of the element. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("opacity")]
        public virtual float Opacity
        {
            get { return (this.Attributes["opacity"] == null) ? 1.0f : (float)this.Attributes["opacity"]; }
            set { this.Attributes["opacity"] = FixOpacityValue(value); }
        }
    }
}