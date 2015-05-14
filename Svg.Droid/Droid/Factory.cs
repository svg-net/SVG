using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Android.Graphics;
using Svg.Droid;
using Color = System.Drawing.Color;
using PointF = System.Drawing.PointF;

namespace Svg
{
    public class Factory : IFactory
    {
        public static IFactory Instance = new Factory();

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
            return new AndroidPen(brush, strokeWidth);
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
            var bitmap = (AndroidBitmap)input;
            return new AndroidGraphics(bitmap);
        }

        public Graphics CreateGraphicsFromImage(Image image)
        {
            var bitmap = (AndroidBitmap) image;
            return new AndroidGraphics(bitmap);
        }

        public ColorMatrix CreateColorMatrix(float[][] colorMatrixElements)
        {
            return new AndroidColorMatrix(colorMatrixElements);
        }

        public ImageAttributes CreateImageAttributes()
        {
            throw new System.NotImplementedException();
        }

        public SolidBrush CreateSolidBrush(Color color)
        {
            return new AndroidSolidBrush(color.ToColor());
        }

        public ColorBlend CreateColorBlend(int colourBlends)
        {
            return new ColorBlend(colourBlends);
        }

        public TextureBrush CreateTextureBrush(Bitmap image)
        {
            return new AndroidTextureBrush((AndroidBitmap) image);
        }

        public LinearGradientBrush CreateLinearGradientBrush(PointF start, PointF end, Color startColor, Color endColor)
        {
            return new AndroidLinearGradientBrush(start.ToPointF(), end.ToPointF(), startColor.ToColor(), endColor.ToColor());
        }

        public PathGradientBrush CreatePathGradientBrush(GraphicsPath path)
        {
            throw new System.NotImplementedException();
        }

        public StringFormat CreateStringFormatGenericTypographic()
        {
            throw new NotImplementedException();
        }

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
            var bitmap = BitmapFactory.DecodeStream(stream);
            return new AndroidBitmap(bitmap);
        }

        public Bitmap CreateBitmapFromStream(Stream stream)
        {
            var bitmap = BitmapFactory.DecodeStream(stream);
            return new AndroidBitmap(bitmap);
        }
    }
}