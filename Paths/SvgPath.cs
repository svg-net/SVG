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
    [Serializable()]
    public class SvgPath : SvgGraphicsElement
    {
        private SvgPathSegmentList _pathData;
        private GraphicsPath _path;
        private int _pathLength;

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

        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        public override System.Drawing.RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        protected override string ElementName
        {
            get { return "path"; }
        }

        public SvgPath()
        {
            this._pathData = new SvgPathSegmentList();
            this._pathData._owner = this;
        }
    }
}