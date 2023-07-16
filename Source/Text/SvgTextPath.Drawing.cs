using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgTextPath : SvgTextBase
    {
        protected override GraphicsPath GetBaselinePath(ISvgRenderer renderer)
        {
            var path = this.OwnerDocument.IdManager.GetElementById(this.ReferencedPath) as SvgVisualElement;
            if (path == null) return null;
            var pathData = (GraphicsPath)path.Path(renderer).Clone();
            if (path.Transforms != null && path.Transforms.Count > 0)
                using (var matrix = path.Transforms.GetMatrix())
                    pathData.Transform(matrix);
            return pathData;
        }

        protected override float GetAuthorPathLength()
        {
            var path = this.OwnerDocument.IdManager.GetElementById(this.ReferencedPath) as SvgPath;
            if (path == null) return 0;
            return path.PathLength;
        }
    }
}
