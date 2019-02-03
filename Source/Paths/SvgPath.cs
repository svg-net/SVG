using System.Drawing.Drawing2D;
using Svg.Pathing;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public class SvgPath : SvgMarkerElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d", true)]
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
        [SvgAttribute("pathLength", true)]
        public float PathLength
        {
            get { return this.Attributes.GetAttribute<float>("pathLength"); }
            set { this.Attributes["pathLength"] = value; }
        }

	

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
                this._path = new GraphicsPath();

                foreach (SvgPathSegment segment in this.PathData)
                {
                    segment.AddToPath(_path);
                }

                if (_path.PointCount == 0)
                {
                    if (this.PathData.Count > 0)
                    {
                        // special case with one move command only, see #223
                        var segment = this.PathData.Last;
                        _path.AddLine(segment.End, segment.End);
                        this.Fill = SvgPaintServer.None;
                    }
                    else
                    {
                        _path = null;
                    }
                }
                this.IsPathDirty = false;
            }
            return _path;
        }

        internal void OnPathUpdated()
        {
            this.IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs{ Attribute = "d", Value = this.Attributes.GetAttribute<SvgPathSegmentList>("d") });
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get { return this.Path(null).GetBounds(); }
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