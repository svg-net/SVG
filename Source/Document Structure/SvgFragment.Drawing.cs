#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgFragment : SvgElement, ISvgBoundable
    {
        PointF ISvgBoundable.Location
        {
            get { return PointF.Empty; }
        }

        SizeF ISvgBoundable.Size
        {
            get
            {
                // Prevent stack overflow due to mutually recursive call.
                if (Width.Type == SvgUnitType.Percentage || Height.Type == SvgUnitType.Percentage)
                    return new SizeF();
                return GetDimensions();
            }
        }

        RectangleF ISvgBoundable.Bounds
        {
            get { return new RectangleF(((ISvgBoundable)this).Location, ((ISvgBoundable)this).Size); }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer))
                return false;
            ViewBox.AddViewBoxTransform(AspectRatio, renderer, this);
            return true;
        }

        protected override void Render(ISvgRenderer renderer)
        {
            switch (Overflow)
            {
                case SvgOverflow.Auto:
                case SvgOverflow.Visible:
                case SvgOverflow.Inherit:
                    base.Render(renderer);
                    break;
                default:
                    var prevClip = renderer.GetClip();
                    try
                    {
                        var size = this is SvgDocument ? renderer.GetBoundable().Bounds.Size : GetDimensions(renderer);
                        var clip = new RectangleF(X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                            Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this),
                            size.Width, size.Height);
                        renderer.SetClip(new Region(clip), CombineMode.Intersect);
                        try
                        {
                            renderer.SetBoundable(new GenericBoundable(clip));
                            base.Render(renderer);
                        }
                        finally
                        {
                            renderer.PopBoundable();
                        }
                    }
                    finally
                    {
                        renderer.SetClip(prevClip, CombineMode.Replace);
                    }
                    break;
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
                var bounds = new RectangleF();
                foreach (var child in Children)
                {
                    RectangleF childBounds = new RectangleF();
                    if (child is SvgFragment)
                    {
                        childBounds = ((SvgFragment)child).Bounds;
                        childBounds.Offset(((SvgFragment)child).X, ((SvgFragment)child).Y);
                    }
                    else if (child is SvgVisualElement)
                    {
                        childBounds = ((SvgVisualElement)child).Bounds;
                    }

                    if (!childBounds.IsEmpty)
                    {
                        if (bounds.IsEmpty)
                        {
                            bounds = childBounds;
                        }
                        else
                        {
                            bounds = RectangleF.Union(bounds, childBounds);
                        }
                    }
                }

                return TransformedBounds(bounds);
            }
        }

        public SizeF GetDimensions()
        {
            return GetDimensions(null);
        }

        internal SizeF GetDimensions(ISvgRenderer renderer)
        {
            float w, h;
            var isWidthperc = Width.Type == SvgUnitType.Percentage;
            var isHeightperc = Height.Type == SvgUnitType.Percentage;

            var bounds = new RectangleF();
            if (isWidthperc || isHeightperc)
            {
                if (ViewBox.Width > 0 && ViewBox.Height > 0)
                {
                    bounds = new RectangleF(ViewBox.MinX, ViewBox.MinY, ViewBox.Width, ViewBox.Height);
                }
                else
                {
                    bounds = Bounds; // do just one call to the recursive bounds property
                }
            }

            if (isWidthperc && this is SvgDocument)
            {
                w = (bounds.Width + bounds.X) * (Width.Value * 0.01f);
            }
            else
            {
                w = Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
            }
            if (isHeightperc && this is SvgDocument)
            {
                h = (bounds.Height + bounds.Y) * (Height.Value * 0.01f);
            }
            else
            {
                h = Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
            }

            return new SizeF(w, h);
        }
    }
}
#endif
