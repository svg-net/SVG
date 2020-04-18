using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.Pathing;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public class SvgPath : SvgMarkerElement, ISvgPathElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d")]
        public SvgPathSegmentList PathData
        {
            get { return GetAttribute<SvgPathSegmentList>("d", false); }
            set
            {
                var old = PathData;
                if (old != null)
                    old.Owner = null;
                Attributes["d"] = value;
                value.Owner = this;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the length of the path.
        /// </summary>
        [SvgAttribute("pathLength")]
        public float PathLength
        {
            get { return GetAttribute<float>("pathLength", false); }
            set { Attributes["pathLength"] = value; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                _path = new GraphicsPath();

                if (PathData != null && PathData.Count > 0 && PathData.First is SvgMoveToSegment)
                {
                    foreach (var segment in PathData)
                        segment.AddToPath(_path);

                    if (_path.PointCount == 0)
                    {
                        if (PathData.Count > 0)
                        {
                            // special case with one move command only, see #223
                            var segment = PathData.Last;
                            _path.AddLine(segment.End, segment.End);
                            Fill = SvgPaintServer.None;
                        }
                        else
                            _path = null;
                    }
                    else if (renderer == null)
                    {
                        // Calculate boundary including stroke width.
                        var radius = StrokeWidth * 2;
                        var bounds = _path.GetBounds();
                        _path.AddEllipse(bounds.Left - radius, bounds.Top - radius, 2 * radius, 2 * radius);
                        _path.AddEllipse(bounds.Right - radius, bounds.Bottom - radius, 2 * radius, 2 * radius);
                    }
                }

                if (renderer != null)
                    IsPathDirty = false;
            }
            return _path;
        }

        public void OnPathUpdated()
        {
            IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs { Attribute = "d", Value = Attributes.GetAttribute<SvgPathSegmentList>("d") });
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get { return Path(null).GetBounds(); }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPath>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgPath;

            if (newObj.PathData != null)
            {
                newObj.PathData.Owner = newObj;
                newObj.IsPathDirty = true;
            }
            return newObj;
        }
    }
}
