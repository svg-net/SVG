using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Svg.Droid;

namespace Svg
{
    public class Factory : IFactory
    {
        public static IFactory Instance { get; set; }
        public GraphicsPath CreateGraphicsPath()
        {
            return new AndroidGraphicsPath();
        }

        public GraphicsPath CreateGraphicsPath(FillMode fillmode)
        {
            return new AndroidGraphicsPath(fillmode);
        }

        public Region CreateRegion()
        {
            throw new NotImplementedException();
        }

        public Region CreateRegion(RectangleF rect)
        {
            throw new System.NotImplementedException();
        }

        public Pen CreatePen(Brush brush, float strokeWidth)
        {
            throw new System.NotImplementedException();
        }

        public Matrix CreateMatrix()
        {
            return new AndroidMatrix();
        }
        public Matrix CreateMatrix(float i, float i1, float i2, float i3, float i4, float i5)
        {
            return new AndroidMatrix(i, i1, i2, i3, i4, i5);
        }

        public Bitmap CreateBitmap(Image inputImage)
        {
            return new AndroidBitmap(inputImage);
        }

        public Bitmap CreateBitmap(int width, int height)
        {
            return new AndroidBitmap(width, height);
        }

        public Graphics CreateGraphicsFromImage(Bitmap input)
        {
            throw new System.NotImplementedException();
        }

        public Graphics CreateGraphicsFromImage(Image image)
        {
            throw new System.NotImplementedException();
        }

        public ColorMatrix CreateColorMatrix(float[][] colorMatrixElements)
        {
            throw new System.NotImplementedException();
        }

        public ImageAttributes CreateImageAttributes()
        {
            throw new System.NotImplementedException();
        }

        public SolidBrush CreateSolidBrush(Color transparent)
        {
            throw new System.NotImplementedException();
        }

        public ColorBlend CreateColorBlend(int colourBlends)
        {
            return new ColorBlend(colourBlends);
        }

        public TextureBrush CreateTextureBrush(Bitmap image)
        {
            throw new System.NotImplementedException();
        }

        public LinearGradientBrush CreateLinearGradientBrush(PointF effectiveStart, PointF effectiveEnd, Color transparent, Color color)
        {
            throw new System.NotImplementedException();
        }

        public PathGradientBrush CreatePathGradientBrush(GraphicsPath path)
        {
            throw new System.NotImplementedException();
        }


        public StringFormat CreateStringFormatGenericTypographic { get; set; }
        public Font CreateFont(FontFamily fontFamily, float fontSize, FontStyle fontStyle, GraphicsUnit graphicsUnit)
        {
            throw new System.NotImplementedException();
        }

        public FontFamilyProvider GetFontFamilyProvider()
        {
            throw new System.NotImplementedException();
        }

        public Image CreateImageFromStream(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public Bitmap CreateBitmapFromStream(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}