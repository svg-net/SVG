
using System.Drawing;

namespace Svg
{
    public interface Bitmap : Image
    {
        int Width { get; set; }
        int Height { get; set; }
        BitmapData LockBits(Rectangle rectangle, ImageLockMode lockmode, PixelFormat pixelFormat);
        void UnlockBits(BitmapData bitmapData);
        void Save(string path);
    }
}