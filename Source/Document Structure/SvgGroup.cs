using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// An element used to group SVG shapes.
    /// </summary>
    [SvgElement("g")]
    public class SvgGroup : SvgVisualElement
    {
        public SvgGroup()
        {
        }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>The fill.</value>
        [SvgAttribute("fill")]
        public override SvgPaintServer Fill
        {
            get { return (this.Attributes["Fill"] == null) ? null : (SvgPaintServer)this.Attributes["Fill"]; }
            set { this.Attributes["Fill"] = value; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get 
            { 
            	var path = new GraphicsPath();

            	//AddPaths(this, path);
  
            	return GetPaths(this);
            }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get 
            { 
            	var r = new RectangleF();
            	foreach(var c in this.Children)
            	{
            		if(c is SvgVisualElement)
            			r = RectangleF.Union(r, ((SvgVisualElement)c).Bounds);
            	}
				return r;         	
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(SvgRenderer renderer)
        {
            this.PushTransforms(renderer);
            this.SetClip(renderer);
            base.RenderChildren(renderer);
            this.ResetClip(renderer);
            this.PopTransforms(renderer);
        }

		
		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgGroup>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgGroup;
			if (this.Fill != null)
				newObj.Fill = this.Fill.DeepCopy() as SvgPaintServer;
			return newObj;
		}
    }
}