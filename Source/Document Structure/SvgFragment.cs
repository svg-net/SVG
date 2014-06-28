using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// An <see cref="SvgFragment"/> represents an SVG fragment that can be the root element or an embedded fragment of an SVG document.
    /// </summary>
    [SvgElement("svg")]
    public class SvgFragment : SvgElement, ISvgViewPort
    {
        /// <summary>
        /// Gets the SVG namespace string.
        /// </summary>
        public static readonly Uri Namespace = new Uri("http://www.w3.org/2000/svg");
        
        private SvgUnit _x;
        private SvgUnit _y;
        
        /// <summary>
        /// Gets or sets the position where the left point of the svg should start.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
        	get { return _x; }
        	set
        	{
        		if(_x != value)
        		{
        			_x = value;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "x", Value = value });
        		}
        	}
        }

        /// <summary>
        /// Gets or sets the position where the top point of the svg should start.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
        	get { return _y; }
        	set
        	{
        		if(_y != value)
        		{
        			_y = value;
        			OnAttributeChanged(new AttributeEventArgs{ Attribute = "y", Value = value });
        		}
        	}
        }

        /// <summary>
        /// Gets or sets the width of the fragment.
        /// </summary>
        /// <value>The width.</value>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
			set { this.Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the fragment.
        /// </summary>
        /// <value>The height.</value>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
			set { this.Attributes["height"] = value; }
        }

		[SvgAttribute("overflow")]
		public virtual SvgOverflow Overflow
		{
			get { return this.Attributes.GetAttribute<SvgOverflow>("overflow"); }
			set { this.Attributes["overflow"] = value; }
		}

        /// <summary>
        /// Gets or sets the viewport of the element.
        /// </summary>
        /// <value></value>
        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return this.Attributes.GetAttribute<SvgViewBox>("viewBox"); }
            set { this.Attributes["viewBox"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
		[SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
			get { return this.Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
			set { this.Attributes["preserveAspectRatio"] = value; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        protected internal override void PushTransforms(SvgRenderer renderer)
        {
            base.PushTransforms(renderer);

            if (!this.ViewBox.Equals(SvgViewBox.Empty))
            {
				float fScaleX = this.Width.ToDeviceValue() / this.ViewBox.Width;
				float fScaleY = this.Height.ToDeviceValue() / this.ViewBox.Height;
				float fMinX = -this.ViewBox.MinX;
				float fMinY = -this.ViewBox.MinY;

				if (AspectRatio.Align != SvgPreserveAspectRatio.none)
				{
					fScaleX = Math.Min(fScaleX, fScaleY);
					fScaleY = Math.Min(fScaleX, fScaleY);
					float fViewMidX = (this.ViewBox.Width / 2) * fScaleX;
					float fViewMidY = (this.ViewBox.Height / 2) * fScaleY;
					float fMidX = this.Width.ToDeviceValue() / 2;
					float fMidY = this.Height.ToDeviceValue() / 2;

					switch (AspectRatio.Align)
					{
						case SvgPreserveAspectRatio.xMinYMin:
							break;
						case SvgPreserveAspectRatio.xMidYMin:
							fMinX += (fMidX - fViewMidX) / fScaleX;
							break;
						case SvgPreserveAspectRatio.xMaxYMin:
							fMinX += this.ViewBox.Width - this.Width.ToDeviceValue();
							break;
						case SvgPreserveAspectRatio.xMinYMid:
							fMinY += (fMidY - fViewMidY) / fScaleY;
							break;
						case SvgPreserveAspectRatio.xMidYMid:
							fMinX += (fMidX - fViewMidX) / fScaleX;
							fMinY += (fMidY - fViewMidY) / fScaleY;
							break;
						case SvgPreserveAspectRatio.xMaxYMid:
							fMinX += this.ViewBox.Width - this.Width.ToDeviceValue();
							fMinY += (fMidY - fViewMidY) / fScaleY;
							break;
						case SvgPreserveAspectRatio.xMinYMax:
							fMinY += this.ViewBox.Height - this.Height.ToDeviceValue();
							break;
						case SvgPreserveAspectRatio.xMidYMax:
							fMinX += (fMidX - fViewMidX) / fScaleX;
							fMinY += this.ViewBox.Height - this.Height.ToDeviceValue();
							break;
						case SvgPreserveAspectRatio.xMaxYMax:
							fMinX += this.ViewBox.Width - this.Width.ToDeviceValue();
							fMinY += this.ViewBox.Height - this.Height.ToDeviceValue();
							break;
						default:
							break;
					}
				}

            	renderer.TranslateTransform(_x, _y, MatrixOrder.Append);
				renderer.TranslateTransform(fMinX, fMinY, MatrixOrder.Append);
				renderer.ScaleTransform(fScaleX, fScaleY, MatrixOrder.Append);
            }
        }
        
        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public GraphicsPath Path
        {
            get 
            { 
            	var path = new GraphicsPath();

            	AddPaths(this, path);
  
            	return path;
            }
        }
        
        /// <summary>
        /// Gets the bounds of the svg element.
        /// </summary>
        /// <value>The bounds.</value>
        public RectangleF Bounds 
        { 
        	get
        	{
        		return this.Path.GetBounds();
        	}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFragment"/> class.
        /// </summary>
        public SvgFragment()
        {
        	_x = 0.0f;
            _y = 0.0f;
            this.Height = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this.Width = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this.ViewBox = SvgViewBox.Empty;
            this.AspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid);
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgFragment>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgFragment;
			newObj.Height = this.Height;
			newObj.Width = this.Width;
			newObj.Overflow = this.Overflow;
			newObj.ViewBox = this.ViewBox;
			newObj.AspectRatio = this.AspectRatio;
			return newObj;
		}
    }
}