using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using Svg.Pathing;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public class SvgPath : SvgVisualElement
    {
        private SvgPathSegmentList _pathData;
        private GraphicsPath _path;
        private int _pathLength;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d")]
        public SvgPathSegmentList PathData
        {
            get { return this._pathData; }
            set
            {
                this._pathData = value;
                this._pathData._owner = this;
                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the length of the path.
        /// </summary>
        [SvgAttribute("pathLength")]
        public int PathLength
        {
            get { return this._pathLength; }
            set { this._pathLength = value; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    _path = new GraphicsPath();

                    foreach (SvgPathSegment segment in this.PathData)
                    {
                        segment.AddToPath(_path);
                    }

                    this.IsPathDirty = false;
                }
                return _path;
            }
        }

        internal void OnPathUpdated()
        {
            this.IsPathDirty = true;
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPath"/> class.
        /// </summary>
        public SvgPath()
        {
            this._pathData = new SvgPathSegmentList();
            this._pathData._owner = this;
        }
    }
}