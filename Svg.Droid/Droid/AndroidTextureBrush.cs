using Android.Graphics;

namespace Svg.Droid
{
    public class AndroidTextureBrush : TextureBrush, IAndroidShader
    {
        private AndroidBitmap _image;
        private BitmapShader _shader;

        public AndroidTextureBrush(AndroidBitmap image)
        {
            _image = image;
        }

        public void Dispose()
        {
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
            if (_shader != null)
            {
                _shader.Dispose();
                _shader = null;
            }
        }
        // TODO LX what about Transform?
        public Matrix Transform { get; set; }
        public void ApplyTo(Paint paint)
        {
            if (_shader != null)
            {
                _shader.Dispose();
                _shader = null;
            }

            _shader = new BitmapShader(_image.Image, Shader.TileMode.Clamp, Shader.TileMode.Clamp);

            paint.SetShader(_shader);
        }
    }
}