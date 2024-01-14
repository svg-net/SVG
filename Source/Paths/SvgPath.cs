using Svg.Pathing;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public partial class SvgPath : SvgMarkerElement, ISvgPathElement
    {
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

        public void OnPathUpdated()
        {
            IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs { Attribute = "d", Value = Attributes.GetAttribute<SvgPathSegmentList>("d") });
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
