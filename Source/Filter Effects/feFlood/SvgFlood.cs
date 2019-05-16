using System;
using System.Drawing;

namespace Svg.FilterEffects
{
    [SvgElement("feFlood")]
    public class SvgFlood : SvgFilterPrimitive
    {
        /// <summary>
        /// The color to use across the entire filter primitive subregion.
        /// </summary>
        [SvgAttribute("flood-color")]
        public SvgPaintServer FloodColor { get; set; }


        /// <summary>
        /// The opacity value to use across the entire filter primitive subregion.
        /// </summary>
        [SvgAttribute("flood-opacity")]
        public float FloodOpacity { get; set; }


        public override void Process(ImageBuffer buffer)
        {
            var inputImage = buffer[this.Input];
            var result = Apply(inputImage);
            buffer[this.Result] = result;
            buffer.SetAlphaState(this.Result, AlphaState.Premultiplied);
        }

        private Bitmap Apply(Bitmap inputImage)
        {
            var bitmapSrc = inputImage as Bitmap;

            using (var src = new RawBitmap(bitmapSrc))
            {
                using (var dest = new RawBitmap(new Bitmap(inputImage.Width, inputImage.Height)))
                {
                    var color = SvgDeferredPaintServer.TryGet<SvgColourServer>(this.FloodColor, this).Colour;
                    var floodAlpha = this.FloodOpacity;
                    var r = (byte)(color.R * floodAlpha + 0.5);
                    var b = (byte)(color.B * floodAlpha + 0.5);
                    var g = (byte)(color.G * floodAlpha + 0.5);
                    int pixelCount = src.Width * src.Height;
                    int index = 0;
                    for (int i = 0; i < pixelCount; i++)
                    {
                        dest.ArgbValues[index] = b;
                        ++index;
                        dest.ArgbValues[index] = g;
                        ++index;
                        dest.ArgbValues[index] = r;
                        ++index;
                        dest.ArgbValues[index] = (byte)(src.ArgbValues[index] * floodAlpha + 0.5);
                        ++index;
                    }
                    return dest.Bitmap;
                }
            }
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgOffset>();
        }


        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFlood;
            newObj.FloodColor = this.FloodColor;
            newObj.FloodOpacity = this.FloodOpacity;

            return newObj;
        }

    }
}
