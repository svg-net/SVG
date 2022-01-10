#if !NO_SDC
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public abstract partial class SvgElement : ISvgRenderElement, ISvgRenderTransformable
    {
        private Matrix _graphicsTransform;
        private Region _graphicsClip;

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal virtual bool PushTransforms(ISvgRenderer renderer)
        {
            _graphicsTransform = renderer.Transform;
            _graphicsClip = renderer.GetClip();

            var transforms = Transforms;
            // Return if there are no transforms
            if (transforms == null || transforms.Count == 0)
                return true;

            using (var transformMatrix = transforms.GetMatrix())
            {
                using (var zeroMatrix = new Matrix(0f, 0f, 0f, 0f, 0f, 0f))
                    if (zeroMatrix.Equals(transformMatrix))
                        return false;

                using (var graphicsTransform = _graphicsTransform.Clone())
                {
                    graphicsTransform.Multiply(transformMatrix);
                    renderer.Transform = graphicsTransform;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that should have transforms removed.</param>
        protected internal virtual void PopTransforms(ISvgRenderer renderer)
        {
            renderer.Transform = _graphicsTransform;
            _graphicsTransform.Dispose();
            _graphicsTransform = null;
            renderer.SetClip(_graphicsClip);
            _graphicsClip = null;
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        void ISvgRenderTransformable.PushTransforms(ISvgRenderer renderer)
        {
            PushTransforms(renderer);
        }

        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that should have transforms removed.</param>
        void ISvgRenderTransformable.PopTransforms(ISvgRenderer renderer)
        {
            PopTransforms(renderer);
        }

        /// <summary>
        /// Transforms the given rectangle with the set transformation, if any.
        /// Can be applied to bounds calculated without considering the element transformation.
        /// </summary>
        /// <param name="bounds">The rectangle to be transformed.</param>
        /// <returns>The transformed rectangle, or the original rectangle if no transformation exists.</returns>
        protected RectangleF TransformedBounds(RectangleF bounds)
        {
            if (Transforms != null && Transforms.Count > 0)
            {
                using (var path = new GraphicsPath())
                using (var matrix = Transforms.GetMatrix())
                {
                    path.AddRectangle(bounds);
                    path.Transform(matrix);
                    return path.GetBounds();
                }
            }
            return bounds;
        }

        /// <summary>
        /// Renders this element to the <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that the element should use to render itself.</param>
        public void RenderElement(ISvgRenderer renderer)
        {
            Render(renderer);
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected virtual void Render(ISvgRenderer renderer)
        {
            try
            {
                PushTransforms(renderer);
                RenderChildren(renderer);
            }
            finally
            {
                PopTransforms(renderer);
            }
        }

        /// <summary>
        /// Renders the children of this <see cref="SvgElement"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to render the child <see cref="SvgElement"/>s to.</param>
        protected virtual void RenderChildren(ISvgRenderer renderer)
        {
            foreach (var element in Children)
                element.Render(renderer);
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        void ISvgRenderElement.Render(ISvgRenderer renderer)
        {
            Render(renderer);
        }

        /// <summary>
        /// Recursive method to add up the paths of all children
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="path"></param>
        protected void AddPaths(SvgElement elem, GraphicsPath path)
        {
            foreach (var child in elem.Children)
            {
                // Skip to avoid double calculate Symbol element
                // symbol element is only referenced by use element
                // So here we need to skip when it is directly considered
                if (child is SvgSymbol)
                    continue;

                if (child is SvgVisualElement)
                {
                    if (!(child is SvgGroup))
                    {
                        var childPath = ((SvgVisualElement)child).Path(null);
                        if (childPath != null)
                            using (childPath = (GraphicsPath)childPath.Clone())
                            {
                                if (child.Transforms != null)
                                    using (var matrix = child.Transforms.GetMatrix())
                                        childPath.Transform(matrix);

                                if (childPath.PointCount > 0)
                                    path.AddPath(childPath, false);
                            }
                    }
                }

                if (!(child is SvgPaintServer)) AddPaths(child, path);
            }
        }

        /// <summary>
        /// Recursive method to add up the paths of all children
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="renderer"></param>
        protected GraphicsPath GetPaths(SvgElement elem, ISvgRenderer renderer)
        {
            var ret = new GraphicsPath();

            foreach (var child in elem.Children)
            {
                if (child is SvgVisualElement)
                {
                    if (child is SvgGroup)
                    {
                        var childPath = GetPaths(child, renderer);
                        if (childPath.PointCount > 0)
                        {
                            if (child.Transforms != null)
                                using (var matrix = child.Transforms.GetMatrix())
                                    childPath.Transform(matrix);

                            ret.AddPath(childPath, false);
                        }
                    }
                    else
                    {
                        var childPath = ((SvgVisualElement)child).Path(renderer);
                        childPath = childPath != null ? (GraphicsPath)childPath.Clone() : new GraphicsPath();

                        // Non-group element can have child element which we have to consider. i.e tspan in text element
                        if (child.Children.Count > 0)
                        {
                            var descendantPath = GetPaths(child, renderer);
                            if (descendantPath.PointCount > 0)
                                childPath.AddPath(descendantPath, false);
                        }

                        if (childPath.PointCount > 0)
                        {
                            if (child.Transforms != null)
                                using (var matrix = child.Transforms.GetMatrix())
                                    childPath.Transform(matrix);

                            ret.AddPath(childPath, false);
                        }
                    }
                }
            }

            return ret;
        }
    }

    internal interface ISvgRenderElement
    {
#if !NO_SDC
        void Render(ISvgRenderer renderer);
#endif
    }
}
#endif
