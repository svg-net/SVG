using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;

namespace Svg.FilterEffects
{
    public partial class SvgColourMatrix : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            var inputImage = buffer[this.Input];

            if (inputImage == null)
                return;

            float[][] colorMatrixElements;
            float value;
            switch (this.Type)
            {
                case SvgColourMatrixType.HueRotate:
                    value = (string.IsNullOrEmpty(this.Values) ? 0 : float.Parse(this.Values, NumberStyles.Any, CultureInfo.InvariantCulture));
                    colorMatrixElements = new float[][] {
                        new float[] {(float)(0.213 + Math.Cos(value) * +0.787 + Math.Sin(value) * -0.213),
                            (float)(0.715 + Math.Cos(value) * -0.715 + Math.Sin(value) * -0.715),
                            (float)(0.072 + Math.Cos(value) * -0.072 + Math.Sin(value) * +0.928), 0, 0},
                        new float[] {(float)(0.213 + Math.Cos(value) * -0.213 + Math.Sin(value) * +0.143),
                            (float)(0.715 + Math.Cos(value) * +0.285 + Math.Sin(value) * +0.140),
                            (float)(0.072 + Math.Cos(value) * -0.072 + Math.Sin(value) * -0.283), 0, 0},
                        new float[] {(float)(0.213 + Math.Cos(value) * -0.213 + Math.Sin(value) * -0.787),
                            (float)(0.715 + Math.Cos(value) * -0.715 + Math.Sin(value) * +0.715),
                            (float)(0.072 + Math.Cos(value) * +0.928 + Math.Sin(value) * +0.072), 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    };
                    break;
                case SvgColourMatrixType.LuminanceToAlpha:
                    colorMatrixElements = new float[][] {
                        new float[] {0, 0, 0, 0, 0},
                        new float[] {0, 0, 0, 0, 0},
                        new float[] {0, 0, 0, 0, 0},
                        new float[] {0.2125f, 0.7154f, 0.0721f, 0, 0},
                        new float[] {0, 0, 0, 0, 1}
                    };
                    break;
                case SvgColourMatrixType.Saturate:
                    value = (string.IsNullOrEmpty(this.Values) ? 1 : float.Parse(this.Values, NumberStyles.Any, CultureInfo.InvariantCulture));
                    colorMatrixElements = new float[][] {
                        new float[] {(float)(0.213+0.787*value), (float)(0.715-0.715*value), (float)(0.072-0.072*value), 0, 0},
                        new float[] {(float)(0.213-0.213*value), (float)(0.715+0.285*value), (float)(0.072-0.072*value), 0, 0},
                        new float[] {(float)(0.213-0.213*value), (float)(0.715-0.715*value), (float)(0.072+0.928*value), 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    };
                    break;
                default: // Matrix
                    var parts = this.Values.Split(new char[] { ' ', '\t', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    colorMatrixElements = new float[5][];
                    for (int i = 0; i < 4; i++)
                    {
                        colorMatrixElements[i] = parts.Skip(i * 5).Take(5).Select(
                            v => float.Parse(v, NumberStyles.Any, CultureInfo.InvariantCulture)).ToArray();
                    }
                    colorMatrixElements[4] = new float[] { 0, 0, 0, 0, 1 };
                    break;
            }

            var colorMatrix = new ColorMatrix(colorMatrixElements);
            using (var imageAttrs = new ImageAttributes())
            {
                imageAttrs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                var result = new Bitmap(inputImage.Width, inputImage.Height);
                using (var g = Graphics.FromImage(result))
                {
                    g.DrawImage(inputImage, new Rectangle(0, 0, inputImage.Width, inputImage.Height),
                        0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel, imageAttrs);
                    g.Flush();
                }
                buffer[this.Result] = result;
            }
        }
    }
}
