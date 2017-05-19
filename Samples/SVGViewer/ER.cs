
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Svg
{


    public sealed class ExplicitRenderer : System.IDisposable, IGraphicsProvider, ISvgRenderer
    {
        Graphics IGraphicsProvider.GetGraphics()
        {
            throw new System.NotImplementedException();
        }

        void System.IDisposable.Dispose()
        {
            throw new System.NotImplementedException();
        }


        float ISvgRenderer.DpiY
        {
            get { throw new System.NotImplementedException(); }
        }

        void ISvgRenderer.DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.DrawImageUnscaled(Image image, Point location)
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.DrawPath(Pen pen, GraphicsPath path)
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.FillPath(Brush brush, GraphicsPath path)
        {
            throw new System.NotImplementedException();
        }

        ISvgBoundable ISvgRenderer.GetBoundable()
        {
            throw new System.NotImplementedException();
        }

        Region ISvgRenderer.GetClip()
        {
            throw new System.NotImplementedException();
        }

        ISvgBoundable ISvgRenderer.PopBoundable()
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.RotateTransform(float fAngle, MatrixOrder order)
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.SetBoundable(ISvgBoundable boundable)
        {
            throw new System.NotImplementedException();
        }

        void ISvgRenderer.SetClip(Region region, CombineMode combineMode)
        {
            throw new System.NotImplementedException();
        }

        SmoothingMode ISvgRenderer.SmoothingMode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        Matrix ISvgRenderer.Transform
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        void ISvgRenderer.TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            throw new System.NotImplementedException();
        }


    }


}
