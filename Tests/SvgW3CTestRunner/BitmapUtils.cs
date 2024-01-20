using System.Drawing.Imaging;
using System.Drawing;
using System;

namespace SvgW3CTestRunner
{
    internal static class BitmapUtils
    {
#if NET5_0_OR_GREATER
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
        public static unsafe Bitmap PixelDiff(Bitmap a, Bitmap b)
        {
            var width = Math.Min(a.Width, b.Width);
            var height = Math.Min(a.Height, b.Height);
            Bitmap output = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(Point.Empty, new Size(width, height));
            using (var aData = a.LockBitsDisposable(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            using (var bData = b.LockBitsDisposable(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            using (var outputData = output.LockBitsDisposable(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb))
            {
                byte* aPtr = (byte*)aData.Scan0;
                byte* bPtr = (byte*)bData.Scan0;
                byte* outputPtr = (byte*)outputData.Scan0;
                int len = aData.Stride * aData.Height;
                for (int i = 0; i < len; i++)
                {
                    // For alpha use the average of both images (otherwise pixels with the same alpha won't be visible)
                    if ((i + 1) % 4 == 0)
                        *outputPtr = (byte)((*aPtr + *bPtr) / 2);
                    else
                        *outputPtr = (byte)~(*aPtr ^ *bPtr);

                    outputPtr++;
                    aPtr++;
                    bPtr++;
                }
            }
            return output;
        }

    }
}
