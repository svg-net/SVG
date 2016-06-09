using System;
using System.Drawing.Imaging;

namespace Svg
{
    public interface ImageAttributes : IDisposable
    {
        void SetColorMatrix(ColorMatrix matrix);
        void SetColorMatrix(ColorMatrix colorMatrix, ColorMatrixFlag colorMatrixFlag, ColorAdjustType colorAdjustType);
    }
}