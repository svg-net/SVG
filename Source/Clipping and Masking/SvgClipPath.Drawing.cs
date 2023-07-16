using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgClipPath : SvgElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets this <see cref="SvgClipPath"/>'s region to be used as a clipping region.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="renderer"></param>
        /// <returns>A new <see cref="Region"/> containing the <see cref="Region"/> to be used for clipping.</returns>
        public Region GetClipRegion(SvgVisualElement owner, ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                _path = new GraphicsPath();

                foreach (var element in Children)
                    CombinePaths(_path, element, renderer);

                IsPathDirty = false;
            }

            var result = _path;
            if (ClipPathUnits == SvgCoordinateUnits.ObjectBoundingBox)
            {
                result = (GraphicsPath)_path.Clone();
                using (var transform = new Matrix())
                {
                    var bounds = owner.Bounds;
                    transform.Scale(bounds.Width, bounds.Height, MatrixOrder.Append);
                    transform.Translate(bounds.Left, bounds.Top, MatrixOrder.Append);
                    result.Transform(transform);
                }
            }

            return new Region(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="element"></param>
        /// <param name="renderer"></param>
        private void CombinePaths(GraphicsPath path, SvgElement element, ISvgRenderer renderer)
        {
            var graphicsElement = element as SvgVisualElement;
            if (graphicsElement != null)
            {
                var childPath = graphicsElement.Path(renderer);
                if (childPath != null)
                {
                    path.FillMode = graphicsElement.ClipRule == SvgClipRule.NonZero ? FillMode.Winding : FillMode.Alternate;

                    if (graphicsElement.Transforms != null)
                        using (var matrix = graphicsElement.Transforms.GetMatrix())
                            childPath.Transform(matrix);

                    if (childPath.PointCount > 0)
                        path.AddPath(childPath, false);
                }
            }

            foreach (var child in element.Children)
                CombinePaths(path, child, renderer);
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
        }
    }
}
