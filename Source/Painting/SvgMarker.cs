using System.Linq;
using Svg.DataTypes;

namespace Svg
{
    [SvgElement("marker")]
    public partial class SvgMarker : SvgPathBasedElement, ISvgViewPort
    {
        private SvgVisualElement _markerElement = null;

        /// <summary>
        /// Return the child element that represent the marker
        /// </summary>
        private SvgVisualElement MarkerElement
        {
            get
            {
                if (_markerElement == null)
                {
                    _markerElement = (SvgVisualElement)this.Children.FirstOrDefault(x => x is SvgVisualElement);
                }
                return _markerElement;
            }
        }

        [SvgAttribute("refX")]
        public virtual SvgUnit RefX
        {
            get { return GetAttribute<SvgUnit>("refX", false, 0f); }
            set { Attributes["refX"] = value; }
        }

        [SvgAttribute("refY")]
        public virtual SvgUnit RefY
        {
            get { return GetAttribute<SvgUnit>("refY", false, 0f); }
            set { Attributes["refY"] = value; }
        }

        [SvgAttribute("orient")]
        public virtual SvgOrient Orient
        {
            get { return GetAttribute<SvgOrient>("orient", false, 0f); }
            set { Attributes["orient"] = value; }
        }

        [SvgAttribute("overflow")]
        public virtual SvgOverflow Overflow
        {
            get { return GetAttribute("overflow", false, SvgOverflow.Hidden); }
            set { Attributes["overflow"] = value; }
        }

        [SvgAttribute("viewBox")]
        public virtual SvgViewBox ViewBox
        {
            get { return GetAttribute<SvgViewBox>("viewBox", false); }
            set { Attributes["viewBox"] = value; }
        }

        [SvgAttribute("preserveAspectRatio")]
        public virtual SvgAspectRatio AspectRatio
        {
            get { return GetAttribute<SvgAspectRatio>("preserveAspectRatio", false); }
            set { Attributes["preserveAspectRatio"] = value; }
        }

        [SvgAttribute("markerWidth")]
        public virtual SvgUnit MarkerWidth
        {
            get { return GetAttribute<SvgUnit>("markerWidth", false, 3f); }
            set { Attributes["markerWidth"] = value; }
        }

        [SvgAttribute("markerHeight")]
        public virtual SvgUnit MarkerHeight
        {
            get { return GetAttribute<SvgUnit>("markerHeight", false, 3f); }
            set { Attributes["markerHeight"] = value; }
        }

        [SvgAttribute("markerUnits")]
        public virtual SvgMarkerUnits MarkerUnits
        {
            get { return GetAttribute("markerUnits", false, SvgMarkerUnits.StrokeWidth); }
            set { Attributes["markerUnits"] = value; }
        }

        /// <summary>
        /// If not set set in the marker, consider the attribute in the drawing element.
        /// </summary>
        public override SvgPaintServer Fill
        {
            get
            {
                if (MarkerElement != null)
                    return MarkerElement.Fill;
                return base.Fill;
            }
        }

        /// <summary>
        /// If not set set in the marker, consider the attribute in the drawing element.
        /// </summary>
        public override SvgPaintServer Stroke
        {
            get
            {
                if (MarkerElement != null)
                    return MarkerElement.Stroke;
                return base.Stroke;
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMarker>();
        }
    }
}
