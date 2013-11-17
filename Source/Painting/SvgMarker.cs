using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;

namespace Svg
{
    [SvgElement("marker")]
    public class SvgMarker : SvgVisualElement, ISvgViewPort
    {
    	private SvgOrient _svgOrient = new SvgOrient();

        [SvgAttribute("refX")]
        public virtual SvgUnit RefX
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("refX"); }
            set { this.Attributes["refX"] = value; }
        }

        [SvgAttribute("refY")]
        public virtual SvgUnit RefY
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("refY"); }
            set { this.Attributes["refY"] = value; }
        }


		[SvgAttribute("orient")]
		public virtual SvgOrient Orient
		{
			get { return _svgOrient; }
			set { _svgOrient = value; }
		}


		[SvgAttribute("overflow")]
		public virtual SvgOverflow Overflow
		{
			get { return this.Attributes.GetAttribute<SvgOverflow>("overflow"); }
			set { this.Attributes["overflow"] = value; }
		}


		[SvgAttribute("viewBox")]
		public virtual SvgViewBox ViewBox
		{
			get { return this.Attributes.GetAttribute<SvgViewBox>("viewBox"); }
			set { this.Attributes["viewBox"] = value; }
		}


		[SvgAttribute("preserveAspectRatio")]
		public virtual SvgAspectRatio AspectRatio
		{
			get { return this.Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
			set { this.Attributes["preserveAspectRatio"] = value; }
		}


		public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get
            {
            	var path = this.Children.FirstOrDefault(x => x is SvgPath);
				if (path != null)
	            	return (path as SvgPath).Path;
				return null;
            }
            protected set
            {
                // No-op
            }
        }

        public override System.Drawing.RectangleF Bounds
        {
            get
            {
            	var path = this.Path;
				if (path != null)
					return path.GetBounds();
				return new System.Drawing.RectangleF();
            }
        }

		//protected internal override void RenderStroke(SvgRenderer renderer)
		//{
		//    this.PushTransforms(renderer);

		//    SvgElement parent = element._parent;
		//    element._parent = this;
		//    element.RenderElement(renderer);
		//    element._parent = parent;

		//    this.PopTransforms(renderer);
		//}


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgMarker>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgMarker;
			newObj.RefX = this.RefX;
			newObj.RefY = this.RefY;
			newObj.Orient = this.Orient;
			newObj.ViewBox = this.ViewBox;
			newObj.Overflow = this.Overflow;
			newObj.AspectRatio = this.AspectRatio;

			return newObj;
		}
    }
}