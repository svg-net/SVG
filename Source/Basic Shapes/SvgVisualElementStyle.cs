using System;
using System.ComponentModel;

namespace Svg
{
    public abstract partial class SvgVisualElement
    {
        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// </summary>
        [TypeConverter(typeof(SvgBoolConverter))]
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return GetAttribute("visibility", Inherited, true); }
            set { Attributes["visibility"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// Needed to support SVG attribute display="none"
        /// </summary>
        [SvgAttribute("display")]
        public virtual string Display
        {
            get { return GetAttribute("display", Inherited, "inline"); }
            set { Attributes["display"] = value; }
        }

        // Displayable - false if attribute display="none", true otherwise
        protected virtual bool Displayable
        {
            get { return !string.Equals(Display, "none", StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("enable-background")]
        public virtual string EnableBackground
        {
            get { return GetAttribute<string>("enable-background", Inherited); }
            set { Attributes["enable-background"] = value; }
        }
    }
}
