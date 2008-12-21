using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Svg
{
    public sealed class SvgRenderer : IDisposable
    {
        private Graphics _innerGraphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRenderer"/> class.
        /// </summary>
        private SvgRenderer()
        {
        }

        public Region Clip
        {
            get { return this._innerGraphics.Clip; }
            set { this._innerGraphics.Clip = value; }
        }

        /// <summary>
        /// Creates a new <see cref="SvgRenderer"/> from the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the new <see cref="SvgRenderer"/>.</param>
        public static SvgRenderer FromImage(Image image)
        {
            SvgRenderer renderer = new SvgRenderer();
            renderer._innerGraphics = Graphics.FromImage(image);
            return renderer;
        }

        /// <summary>
        /// Creates a new <see cref="SvgRenderer"/> from the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to create the renderer from.</param>
        public static SvgRenderer FromGraphics(Graphics graphics)
        {
            SvgRenderer renderer = new SvgRenderer();
            renderer._innerGraphics = graphics;
            return renderer;
        }

        public void DrawImageUnscaled(Image image, Point location)
        {
            this._innerGraphics.DrawImageUnscaled(image, location);
        }

        public void SetClip(Region region)
        {
            this._innerGraphics.SetClip(region, CombineMode.Complement);
        }

        public void FillPath(Brush brush, GraphicsPath path)
        {
            this._innerGraphics.FillPath(brush, path);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            this._innerGraphics.DrawPath(pen, path);
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            this._innerGraphics.TranslateTransform(dx, dy, order);
        }

        public void TranslateTransform(float dx, float dy)
        {
            this.TranslateTransform(dx, dy, MatrixOrder.Append);
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            this._innerGraphics.ScaleTransform(sx, sy, order);
        }

        public void ScaleTransform(float sx, float sy)
        {
            this.ScaleTransform(sx, sy, MatrixOrder.Append);
        }

        public SmoothingMode SmoothingMode
        {
            get { return this._innerGraphics.SmoothingMode; }
            set { this._innerGraphics.SmoothingMode = value; }
        }

        public PixelOffsetMode PixelOffsetMode
        {
            get { return this._innerGraphics.PixelOffsetMode; }
            set { this._innerGraphics.PixelOffsetMode = value; }
        }

        public CompositingQuality CompositingQuality
        {
            get { return this._innerGraphics.CompositingQuality; }
            set { this._innerGraphics.CompositingQuality = value; }
        }

        public TextRenderingHint TextRenderingHint
        {
            get { return this._innerGraphics.TextRenderingHint; }
            set { this._innerGraphics.TextRenderingHint = value; }
        }

        public int TextContrast
        {
            get { return this._innerGraphics.TextContrast; }
            set { this._innerGraphics.TextContrast = value; }
        }

        public Matrix Transform
        {
            get { return this._innerGraphics.Transform; }
            set { this._innerGraphics.Transform = value; }
        }

        public void Save()
        {
            this._innerGraphics.Save();
        }

        public void Dispose()
        {
            this._innerGraphics.Dispose();
        }
    }
}