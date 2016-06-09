using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Svg
{
    public interface IFactory
    {
        GraphicsPath CreateGraphicsPath();
        GraphicsPath CreateGraphicsPath(FillMode winding);
        Region CreateRegion(RectangleF rect);
        Pen CreatePen(Brush brush, float strokeWidth);
        Matrix CreateMatrix();
        Bitmap CreateBitmap(Image inputImage);
        Bitmap CreateBitmap(int width, int height);
        Graphics CreateGraphicsFromImage(Bitmap input);
        Graphics CreateGraphicsFromImage(Image image);
        ColorMatrix CreateColorMatrix(float[][] colorMatrixElements);
        ImageAttributes CreateImageAttributes();
        SolidBrush CreateSolidBrush(Color color);
        ColorBlend CreateColorBlend(int colourBlends);
        TextureBrush CreateTextureBrush(Bitmap image);
        LinearGradientBrush CreateLinearGradientBrush(PointF start, PointF end, Color startColor, Color endColor);
        PathGradientBrush CreatePathGradientBrush(GraphicsPath path);
        Matrix CreateMatrix(float i, float i1, float i2, float i3, float i4, float i5);
        StringFormat CreateStringFormatGenericTypographic();
        Font CreateFont(FontFamily fontFamily, float fontSize, FontStyle fontStyle, GraphicsUnit graphicsUnit);
        FontFamilyProvider GetFontFamilyProvider();
        Image CreateImageFromStream(Stream stream);
        Bitmap CreateBitmapFromStream(Stream stream);
    }
}