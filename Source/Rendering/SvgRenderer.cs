using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Svg
{
    /// <summary>
    /// Convenience wrapper around a graphics object
    /// </summary>
    public sealed class SvgRenderer : ISvgRenderer, IGraphicsProvider
    {
        private readonly Graphics _innerGraphics;
        private readonly bool _disposable;
        private readonly Image _image;

        private readonly Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();

        public void SetBoundable(ISvgBoundable boundable)
        {
            _boundables.Push(boundable);
        }
        public ISvgBoundable GetBoundable()
        {
            return _boundables.Count > 0 ? _boundables.Peek() : null;
        }
        public ISvgBoundable PopBoundable()
        {
            return _boundables.Pop();
        }

        public float DpiY
        {
            get { return _innerGraphics.DpiY; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ISvgRenderer"/> class.
        /// </summary>
        private SvgRenderer(Graphics graphics, bool disposable = true)
        {
            _innerGraphics = graphics;
            _disposable = disposable;
        }
        private SvgRenderer(Graphics graphics, Image image)
            : this(graphics)
        {
            _image = image;
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            _innerGraphics.DrawImage(image, destRect, srcRect, graphicsUnit);
        }
        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit, float opacity)
        {
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = opacity };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                var points = new[]
                {
                    destRect.Location,
                    new PointF(destRect.X + destRect.Width, destRect.Y),
                    new PointF(destRect.X, destRect.Y + destRect.Height)
                };
                _innerGraphics.DrawImage(image, points, srcRect, graphicsUnit, attributes);
            }
        }

        public void DrawImageUnscaled(Image image, Point location)
        {
            _innerGraphics.DrawImageUnscaled(image, location);
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            _innerGraphics.DrawPath(pen, path);
        }
        public void FillPath(Brush brush, GraphicsPath path)
        {
            _innerGraphics.FillPath(brush, path);
        }
        public Region GetClip()
        {
            return _innerGraphics.Clip;
        }
        public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
        {
            _innerGraphics.RotateTransform(fAngle, order);
        }
        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
        {
            _innerGraphics.ScaleTransform(sx, sy, order);
        }
        public void SetClip(Region region, CombineMode combineMode = CombineMode.Replace)
        {
            _innerGraphics.SetClip(region, combineMode);
        }
        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
        {
            _innerGraphics.TranslateTransform(dx, dy, order);
        }

        public SmoothingMode SmoothingMode
        {
            get { return _innerGraphics.SmoothingMode; }
            set { _innerGraphics.SmoothingMode = value; }
        }

        public Matrix Transform
        {
            get { return _innerGraphics.Transform; }
            set { _innerGraphics.Transform = value; }
        }

        public void Dispose()
        {
            if (_disposable)
                _innerGraphics.Dispose();
            if (_image != null)
                _image.Dispose();
        }

        Graphics IGraphicsProvider.GetGraphics()
        {
            return _innerGraphics;
        }

        private static Graphics CreateGraphics(Image image)
        {
            var g = Graphics.FromImage(image);
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.TextContrast = 1;
            return g;
        }

        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the new <see cref="ISvgRenderer"/>.</param>
        public static ISvgRenderer FromImage(Image image)
        {
            var g = CreateGraphics(image);
            return new SvgRenderer(g);
        }

        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to create the renderer from.</param>
        public static ISvgRenderer FromGraphics(Graphics graphics)
        {
            return new SvgRenderer(graphics, false);
        }

        public static ISvgRenderer FromNull()
        {
            var img = new Bitmap(1, 1);
            var g = CreateGraphics(img);
            return new SvgRenderer(g, img);
        }
    }
}
