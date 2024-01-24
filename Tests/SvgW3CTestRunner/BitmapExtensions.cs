using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SvgW3CTestRunner
{
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    static class BitmapExtensions
    {
        public static DisposableImageData LockBitsDisposable(this Bitmap bitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            return new DisposableImageData(bitmap, rect, flags, format);
        }

        public class DisposableImageData : IDisposable
        {
            private readonly Bitmap _bitmap;
            private readonly BitmapData _data;

            internal DisposableImageData(Bitmap bitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
            {
                _bitmap = bitmap;
                _data = bitmap.LockBits(rect, flags, format);
            }

            public void Dispose()
            {
                _bitmap.UnlockBits(_data);
            }

            public IntPtr Scan0
            {
                get { return _data.Scan0; }
            }

            public int Stride
            {
                get { return _data.Stride; }
            }

            public int Width
            {
                get { return _data.Width; }
            }

            public int Height
            {
                get { return _data.Height; }
            }

            public PixelFormat PixelFormat
            {
                get { return _data.PixelFormat; }
            }

            public int Reserved
            {
                get { return _data.Reserved; }
            }
        }
    }

    /// <summary>
    /// Taken from https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
    /// and slightly modified.
    /// Image width and height, default threshold and handling of alpha values have been adapted.
    /// </summary>
    static class ExtensionMethods
    {
        private static readonly int ImageWidth = 64;
        private static readonly int ImageHeight = 64;

        public static float PercentageDifference(this Image img1, Image img2, byte threshold = 10)
        {
            byte[,] differences = img1.GetDifferences(img2);

            int diffPixels = 0;

            foreach (byte b in differences)
            {
                if (b > threshold) { diffPixels++; }
            }

            return diffPixels / (float)(differences.GetLength(0) * differences.GetLength(1));
        }

        public static Bitmap Resize(this Image originalImage, int newWidth, int newHeight)
        {
            if (originalImage.Width > originalImage.Height)
                newWidth = originalImage.Width * newHeight / originalImage.Height;
            else
                newHeight = originalImage.Height * newWidth / originalImage.Width;

            var smallVersion = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(smallVersion))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(originalImage, 0, 0, smallVersion.Width, smallVersion.Height);
            }

            return smallVersion;
        }

        public static byte[,] GetGrayScaleValues(this Bitmap img)
        {
            byte[,] grayScale = new byte[img.Width, img.Height];

            for (int y = 0; y < grayScale.GetLength(1); y++)
            {
                for (int x = 0; x < grayScale.GetLength(0); x++)
                {
                    var alpha = img.GetPixel(x, y).A;
                    var gray = img.GetPixel(x, y).R;
                    grayScale[x, y] = (byte)Math.Abs(gray * alpha / 255);
                }
            }
            return grayScale;
        }

        // the colormatrix needed to grayscale an image
        static readonly ColorMatrix ColorMatrix = new ColorMatrix(new float[][]
        {
            new float[] {.3f, .3f, .3f, 0, 0},
            new float[] {.59f, .59f, .59f, 0, 0},
            new float[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
        });

        public static Bitmap GetGrayScaleVersion(this Bitmap original)
        {
            // create a blank bitmap the same size as original
            // https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
            var newBitmap = new Bitmap(original.Width, original.Height);

            // get a graphics object from the new image
            using (var g = Graphics.FromImage(newBitmap))
            // create some image attributes
            using (var attributes = new ImageAttributes())
            {
                // set the color matrix attribute
                attributes.SetColorMatrix(ColorMatrix);

                // draw the original image on the new image
                // using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }

            return newBitmap;
        }

        public static byte[,] GetDifferences(this Image img1, Image img2)
        {
            using (var resizedThisOne = img1.Resize(ImageWidth, ImageHeight))
            using (var thisOne = resizedThisOne.GetGrayScaleVersion())
            using (var resizedTheOtherOne = img2.Resize(ImageWidth, ImageHeight))
            using (var theOtherOne = resizedTheOtherOne.GetGrayScaleVersion())
            {
                byte[,] differences = new byte[thisOne.Width, thisOne.Height];
                byte[,] firstGray = thisOne.GetGrayScaleValues();
                byte[,] secondGray = theOtherOne.GetGrayScaleValues();

                for (int y = 0; y < differences.GetLength(1); y++)
                {
                    for (int x = 0; x < differences.GetLength(0); x++)
                    {
                        differences[x, y] = (byte)Math.Abs(firstGray[x, y] - secondGray[x, y]);
                    }
                }
                return differences;
            }
        }
    }

}
