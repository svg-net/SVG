using System.Drawing;

/// <summary>
/// Filter primitive for feBlend
/// Definition taken from SVG1.1 standard
/// 
/// For all feBlend modes, the result opacity is computed as follows:
/// qr = 1 - (1-qa)*(1-qb)
/// For the compositing formulas below, the following definitions apply:
///
/// cr = Result color(RGB) - premultiplied
/// qa = Opacity value at a given pixel for image A
/// qb = Opacity value at a given pixel for image B
/// ca = Color(RGB) at a given pixel for image A - premultiplied
/// cb = Color(RGB) at a given pixel for image B - premultiplied
/// </summary>
namespace Svg.FilterEffects
{
    /// <summary>
    /// </summary>
    [SvgElement("feBlend")]
    public class SvgBlend : SvgFilterPrimitive
    {
        [SvgAttribute("in2")]
        public string Input2
        {
            get { return this.Attributes.GetAttribute<string>("in2"); }
            set { this.Attributes["in2"] = value; }
        }

        [SvgAttribute("mode")]
        public SvgBlendMode BlendMode
        {
            get { return this.Attributes.GetAttribute<SvgBlendMode>("mode", SvgBlendMode.Normal); }
            set { this.Attributes["mode"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            var inputImage1 = buffer.PremultipliedImage(this.Input);
            var inputImage2 = buffer.PremultipliedImage(this.Input2);
            var result = Apply(inputImage1, inputImage2);
            buffer[this.Result] = result;
            buffer.SetAlphaState(this.Result, AlphaState.Premultiplied);
        }

        private Bitmap Apply(Bitmap inputImage1, Bitmap inputImage2)
        {
            var bitmapSrc1 = inputImage1 as Bitmap;
            var bitmapSrc2 = inputImage2 as Bitmap;

            using (var src1 = new RawBitmap(bitmapSrc1))
            using (var src2 = new RawBitmap(bitmapSrc2))
            {
                using (var dest = new RawBitmap(new Bitmap(inputImage1.Width, inputImage1.Height)))
                {
                    switch (BlendMode)
                    {
                        case SvgBlendMode.Normal:
                            return ApplyNormal(src1, src2, dest);
                        case SvgBlendMode.Multiply:
                            return ApplyMultiply(src1, src2, dest);
                        case SvgBlendMode.Screen:
                            return ApplyScreen(src1, src2, dest);
                        case SvgBlendMode.Darken:
                            return ApplyDarken(src1, src2, dest);
                        case SvgBlendMode.Lighten:
                            return ApplyLighten(src1, src2, dest);
                    }
                }
            }
            return null;
        }

        // to do: special handling for cases of src1 and/or src2 being alpha only (performance)

        private Bitmap ApplyNormal(RawBitmap src1, RawBitmap src2, RawBitmap dest)
        {
            int pixelCount = src1.Width * src1.Height; // both bitmaps must have the same size
            int index = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                var alpha1 = src1.ArgbValues[index + 3] / 255.0;
                var alpha2 = src2.ArgbValues[index + 3] / 255.0;
                // cr = (1 - qa) * cb + ca
                dest.ArgbValues[index] = (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)((1 - (1 - alpha1) * (1 - alpha2)) * 255.0 + 0.5);
                ++index;
            }
            return dest.Bitmap;
        }

        private Bitmap ApplyMultiply(RawBitmap src1, RawBitmap src2, RawBitmap dest)
        {
            int pixelCount = src1.Width * src1.Height; // both bitmaps must have the same size
            int index = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                var alpha1 = src1.ArgbValues[index + 3] / 255.0;
                var alpha2 = src2.ArgbValues[index + 3] / 255.0;
                // cr = (1 - qa) * cb + (1 - qb) * ca + ca * cb
                dest.ArgbValues[index] = (byte)((1 - alpha1) * src2.ArgbValues[index]
                    + (1 - alpha2) * src1.ArgbValues[index]
                    + src1.ArgbValues[index] * src2.ArgbValues[index] / 255.0 + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)((1 - alpha1) * src2.ArgbValues[index]
                    + (1 - alpha2) * src1.ArgbValues[index]
                    + src1.ArgbValues[index] * src2.ArgbValues[index] / 255.0 + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)((1 - alpha1) * src2.ArgbValues[index]
                    + (1 - alpha2) * src1.ArgbValues[index]
                    + src1.ArgbValues[index] * src2.ArgbValues[index] / 255.0 + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)((1 - (1 - alpha1) * (1 - alpha2)) * 255.0 + 0.5);
                ++index;
            }
            return dest.Bitmap;
        }

        private Bitmap ApplyScreen(RawBitmap src1, RawBitmap src2, RawBitmap dest)
        {
            int pixelCount = src1.Width * src1.Height; // both bitmaps must have the same size
            int index = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                // cr = cb + ca - ca * cb
                dest.ArgbValues[index] = (byte)(src2.ArgbValues[index] + src1.ArgbValues[index] -
                    src2.ArgbValues[index] * src1.ArgbValues[index] / 255.0 + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)(src2.ArgbValues[index] + src1.ArgbValues[index] -
                    src2.ArgbValues[index] * src1.ArgbValues[index] / 255.0 + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)(src2.ArgbValues[index] + src1.ArgbValues[index] -
                    src2.ArgbValues[index] * src1.ArgbValues[index] / 255.0 + 0.5);
                ++index;
                dest.ArgbValues[index] = (byte)(255 - (255 - src1.ArgbValues[index]) 
                    * (255 - src2.ArgbValues[index]) / 255.0 + 0.5);
                ++index;
            }
            return dest.Bitmap;
        }

        private Bitmap ApplyLighten(RawBitmap src1, RawBitmap src2, RawBitmap dest)
        {
            int pixelCount = src1.Width * src1.Height; // both bitmaps must have the same size
            int index = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                var alpha1 = src1.ArgbValues[index + 3] / 255.0;
                var alpha2 = src2.ArgbValues[index + 3] / 255.0;
                // cr = Max ((1 - qa) * cb + ca, (1 - qb) * ca + cb)
                dest.ArgbValues[index] = System.Math.Max(
                    (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5),
                    (byte)((1 - alpha2) * src1.ArgbValues[index] + src2.ArgbValues[index] + 0.5));
                ++index;
                dest.ArgbValues[index] = System.Math.Max(
                    (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5),
                    (byte)((1 - alpha2) * src1.ArgbValues[index] + src2.ArgbValues[index] + 0.5));
                ++index;
                dest.ArgbValues[index] = System.Math.Max(
                    (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5),
                    (byte)((1 - alpha2) * src1.ArgbValues[index] + src2.ArgbValues[index] + 0.5));
                ++index;
                dest.ArgbValues[index] = (byte)((1 - (1 - alpha1) * (1 - alpha2)) * 255.0 + 0.5);
                ++index;
            }
            return dest.Bitmap;
        }

        private Bitmap ApplyDarken(RawBitmap src1, RawBitmap src2, RawBitmap dest)
        {
            int pixelCount = src1.Width * src1.Height; // both bitmaps must have the same size
            int index = 0;
            for (int i = 0; i < pixelCount; i++)
            {
                var alpha1 = src1.ArgbValues[index + 3] / 255.0;
                var alpha2 = src2.ArgbValues[index + 3] / 255.0;
                // cr = Min ((1 - qa) * cb + ca, (1 - qb) * ca + cb)
                dest.ArgbValues[index] = System.Math.Min(
                    (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5),
                    (byte)((1 - alpha2) * src1.ArgbValues[index] + src2.ArgbValues[index] + 0.5));
                ++index;
                dest.ArgbValues[index] = System.Math.Min(
                    (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5),
                    (byte)((1 - alpha2) * src1.ArgbValues[index] + src2.ArgbValues[index] + 0.5));
                ++index;
                dest.ArgbValues[index] = System.Math.Min(
                    (byte)((1 - alpha1) * src2.ArgbValues[index] + src1.ArgbValues[index] + 0.5),
                    (byte)((1 - alpha2) * src1.ArgbValues[index] + src2.ArgbValues[index] + 0.5));
                ++index;
                dest.ArgbValues[index] = (byte)((1 - (1 - alpha1) * (1 - alpha2)) * 255.0 + 0.5);
                ++index;
            }
            return dest.Bitmap;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgOffset>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgBlend;
            newObj.Input2 = this.Input2;
            newObj.BlendMode = this.BlendMode;

            return newObj;
        }
    }
}