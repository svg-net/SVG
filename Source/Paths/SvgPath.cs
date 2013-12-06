using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using Svg.Pathing;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public class SvgPath : SvgVisualElement
    {
        private GraphicsPath _path;
        private int _pathLength;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d")]
        public SvgPathSegmentList PathData
        {
        	get { return this.Attributes.GetAttribute<SvgPathSegmentList>("d"); }
            set
            {
            	this.Attributes["d"] = value;
            	value._owner = this;
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
        /// Gets or sets the marker (end cap) of the path.
        /// </summary>
		[SvgAttribute("marker-end")]
		public Uri MarkerEnd
        {
			get { return this.Attributes.GetAttribute<Uri>("marker-end"); }
			set { this.Attributes["marker-end"] = value; }
		}


		/// <summary>
		/// Gets or sets the marker (start cap) of the path.
		/// </summary>
		[SvgAttribute("marker-start")]
		public Uri MarkerStart
		{
			get { return this.Attributes.GetAttribute<Uri>("marker-start"); }
			set { this.Attributes["marker-start"] = value; }
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
            protected set
            {
                _path = value;
            }
        }

        internal void OnPathUpdated()
        {
            this.IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs{ Attribute = "d", Value = this.Attributes.GetAttribute<SvgPathSegmentList>("d") });
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
            var pathData = new SvgPathSegmentList();
            this.Attributes["d"] = pathData;
            pathData._owner = this;
        }

		/// <summary>
		/// Renders the stroke of the <see cref="SvgVisualElement"/> to the specified <see cref="SvgRenderer"/>
		/// </summary>
		/// <param name="renderer">The <see cref="SvgRenderer"/> object to render to.</param>
		protected internal override void  RenderStroke(SvgRenderer renderer)
		{
 			if (this.Stroke != null)
			{
				float strokeWidth = this.StrokeWidth.ToDeviceValue(this);
				using (var pen = new Pen(this.Stroke.GetBrush(this, this.StrokeOpacity), strokeWidth))
				{
					if (this.StrokeDashArray != null && this.StrokeDashArray.Count > 0)
					{
						/* divide by stroke width - GDI behaviour that I don't quite understand yet.*/
						pen.DashPattern = this.StrokeDashArray.ConvertAll(u => u.Value / ((strokeWidth <= 0) ? 1 : strokeWidth)).ToArray();
					}

					//hardcoded transformation matrix. I am not sure why this is not in proportion or rotated correctly (something to do with how the endcaps are determined in GDI)
					var transMatrix = new Matrix();
					transMatrix.Rotate(-90f);
					transMatrix.Scale(.6f, .6f);

					if (this.MarkerStart != null)
					{
						var marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerStart.ToString());
						var markerPath = marker.Path.Clone() as GraphicsPath;
						markerPath.Transform(transMatrix);
						pen.CustomStartCap = new CustomLineCap(markerPath, null);
					}

					if (this.MarkerEnd != null)
					{
						var marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerEnd.ToString());
						var markerPath = marker.Path.Clone() as GraphicsPath;
						markerPath.Transform(transMatrix);
						pen.CustomEndCap = new CustomLineCap(markerPath, null);
					}

					renderer.DrawPath(pen, this.Path);
				}
			}
		}


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgPath>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgPath;
			foreach (var pathData in this.PathData)
				newObj.PathData.Add(pathData.Clone());
			newObj.PathLength = this.PathLength;
			newObj.MarkerStart = this.MarkerStart;
			newObj.MarkerEnd = this.MarkerEnd;
			return newObj;

		}




    }
}