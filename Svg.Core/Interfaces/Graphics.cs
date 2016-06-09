
using System;
using System.Drawing;

namespace Svg
{
    public interface Graphics : IDisposable
    {
        void DrawImage(Bitmap bitmap, Rectangle rectangle, int x, int y, int width, int height, GraphicsUnit pixel);
        void DrawImage(Bitmap bitmap, Rectangle rectangle, int x, int y, int width, int height, GraphicsUnit pixel, ImageAttributes attributes);
        void Flush();
        void Save();
        TextRenderingHint TextRenderingHint { get; set; }
        float DpiY { get; }
        Region Clip { get; }
        SmoothingMode SmoothingMode { get; set; }
        Matrix Transform { get; set; }
        void DrawImage(Image bitmap, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit);
        void DrawImageUnscaled(Image image, Point location);
        void DrawPath(Pen pen, GraphicsPath path);
        void FillPath(Brush brush, GraphicsPath path);
        void RotateTransform(float fAngle, MatrixOrder order);
        void ScaleTransform(float sx, float sy, MatrixOrder order);
        void SetClip(Region region, CombineMode combineMode);
        void TranslateTransform(float dx, float dy, MatrixOrder order);
        Region[] MeasureCharacterRanges(string text, Font font, Rectangle rectangle, StringFormat format);
    }
}