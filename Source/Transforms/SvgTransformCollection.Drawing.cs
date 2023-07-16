using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public partial class SvgTransformCollection
    {
        /// <summary>
        /// Multiplies all matrices
        /// </summary>
        /// <returns>The result of all transforms</returns>
        public Matrix GetMatrix()
        {
            var transformMatrix = new Matrix();

            foreach (var transform in this)
                using (var matrix = transform.Matrix)
                    transformMatrix.Multiply(matrix);

            return transformMatrix;
        }
    }
}
