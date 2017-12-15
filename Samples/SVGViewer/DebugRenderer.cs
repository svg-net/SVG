using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Svg;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SVGViewer
{
    class DebugRenderer : ISvgRenderer
    {
        private Region _clip = new Region();
        private Matrix _transform = new Matrix();
        private Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();

        public void SetBoundable(ISvgBoundable boundable)
        {
            _boundables.Push(boundable);
        }
        public ISvgBoundable GetBoundable()
        {
            return _boundables.Peek();
        }
        public ISvgBoundable PopBoundable()
        {
            return _boundables.Pop();
        }


        public float DpiY
        {
            get { return 96; }
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit, float opacity)
        {

        }

        public void DrawImageUnscaled(Image image, Point location)
        {
            
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            var newPath = (GraphicsPath)path.Clone();
            newPath.Transform(_transform);

        }
        public void FillPath(Brush brush, GraphicsPath path)
        {
            var newPath = (GraphicsPath)path.Clone();
            newPath.Transform(_transform);
        }
        public Region GetClip()
        {
            return _clip;
        }
        public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
        {
            _transform.Rotate(fAngle, order);
        }
        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
        {
            _transform.Scale(sx, sy, order);
        }
        public void SetClip(Region region, CombineMode combineMode = CombineMode.Replace)
        {
            switch (combineMode)
            {
                case CombineMode.Intersect:
                    _clip.Intersect(region);
                    break;
                case CombineMode.Complement:
                    _clip.Complement(region);
                    break;
                case CombineMode.Exclude:
                    _clip.Exclude(region);
                    break;
                case CombineMode.Union:
                    _clip.Union(region);
                    break;
                case CombineMode.Xor:
                    _clip.Xor(region);
                    break;
                default:
                    _clip = region;
                    break;
            }
        }
        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
        {
            _transform.Translate(dx, dy, order);
        }        


        public SmoothingMode SmoothingMode
        {
            get { return SmoothingMode.Default; }
            set { /* Do Nothing */ }
        }

        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public void Dispose()
        {
            
        }
    }
}
