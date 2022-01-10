using System;

namespace Svg
{
    /// <summary>
    /// Represents a path based element that can have markers.
    /// </summary>
    public abstract partial class SvgMarkerElement : SvgPathBasedElement
    {
        /// <summary>
        /// Gets or sets the marker (end cap) of the path.
        /// </summary>
        [SvgAttribute("marker-end")]
        public Uri MarkerEnd
        {
            get { return GetAttribute<Uri>("marker-end", true); }
            set { Attributes["marker-end"] = value; }
        }

        /// <summary>
        /// Gets or sets the marker (mid points) of the path.
        /// </summary>
        [SvgAttribute("marker-mid")]
        public Uri MarkerMid
        {
            get { return GetAttribute<Uri>("marker-mid", true); }
            set { Attributes["marker-mid"] = value; }
        }

        /// <summary>
        /// Gets or sets the marker (start cap) of the path.
        /// </summary>
        [SvgAttribute("marker-start")]
        public Uri MarkerStart
        {
            get { return GetAttribute<Uri>("marker-start", true); }
            set { Attributes["marker-start"] = value; }
        }
    }
}
