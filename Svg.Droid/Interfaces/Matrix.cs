
using System;
using System.Drawing;

namespace Svg
{
    public interface Matrix : IDisposable
    {
        void Scale(float width, float height);
        void Scale(float width, float height, MatrixOrder append);
        void Translate(float left, float top, MatrixOrder append);
        void TransformVectors(PointF[] points);
        void Translate(float left, float top);
        void Multiply(Matrix matrix);
        void TransformPoints(PointF[] points);
        void RotateAt(float f, PointF midPoint, MatrixOrder prepend);
        void Rotate(float fAngle, MatrixOrder append);
        Matrix Clone();
        float[] Elements { get; set; }
        void Rotate(float fAngle);
        void Shear(float f, float f1);
    }
}