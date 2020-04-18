using System;
using System.Drawing;

namespace Svg.FilterEffects
{
    public enum BlurType
    {
        Both,
        HorizontalOnly,
        VerticalOnly,
    }

    [SvgElement("feGaussianBlur")]
    public class SvgGaussianBlur : SvgFilterPrimitive
    {
        private float _stdDeviationX = float.NaN;
        private float _stdDeviationY = float.NaN;
        private bool _isPrecalculated = false;
        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;

        private void PreCalculate()
        {
            float stdDeviationX = 0f;
            float stdDeviationY = 0f;

            if (StdDeviation.Count == 1)
            {
                stdDeviationX = StdDeviation[0];
                stdDeviationY = stdDeviationX;
            }
            else if (StdDeviation.Count == 2)
            {
                stdDeviationX = StdDeviation[0];
                stdDeviationY = StdDeviation[1];
            }

            if (stdDeviationX < 0f || stdDeviationY < 0f)
            {
                _isPrecalculated = false;
                return;
            }

            if (_stdDeviationX == stdDeviationX && _stdDeviationY == stdDeviationY)
                return;

            int sz = (int)(stdDeviationX * 2 + 1);
            _kernel = new int[sz];
            _multable = new int[sz, 256];
            for (int i = 1; i <= stdDeviationX; i++)
            {
                int szi = (int)(stdDeviationX - i);
                int szj = (int)(stdDeviationX + i);
                _kernel[szj] = _kernel[szi] = (szi + 1) * (szi + 1);
                _kernelSum += (_kernel[szj] + _kernel[szi]);
                for (int j = 0; j < 256; j++)
                {
                    _multable[szj, j] = _multable[szi, j] = _kernel[szj] * j;
                }
            }
            _kernel[(int)stdDeviationX] = (int)((stdDeviationX + 1) * (stdDeviationX + 1));
            _kernelSum += _kernel[(int)stdDeviationX];
            for (int j = 0; j < 256; j++)
            {
                _multable[(int)stdDeviationX, j] = _kernel[(int)stdDeviationX] * j;
            }

            _stdDeviationX = stdDeviationX;
            _stdDeviationY = stdDeviationY;
            _isPrecalculated = true;
        }

        public Bitmap Apply(Image inputImage)
        {
            var bitmapSrc = inputImage as Bitmap;
            if (bitmapSrc == null) bitmapSrc = new Bitmap(inputImage);

            PreCalculate();
            if (!_isPrecalculated)
            {
                return bitmapSrc;
            }

            using (RawBitmap src = new RawBitmap(bitmapSrc))
            {
                using (RawBitmap dest = new RawBitmap(new Bitmap(inputImage.Width, inputImage.Height)))
                {
                    int pixelCount = src.Width * src.Height;
                    int[] b = new int[pixelCount];
                    int[] g = new int[pixelCount];
                    int[] r = new int[pixelCount];
                    int[] a = new int[pixelCount];

                    int[] b2 = new int[pixelCount];
                    int[] g2 = new int[pixelCount];
                    int[] r2 = new int[pixelCount];
                    int[] a2 = new int[pixelCount];

                    int ptr = 0;
                    for (int i = 0; i < pixelCount; i++)
                    {
                        b[i] = src.ArgbValues[ptr];
                        g[i] = src.ArgbValues[++ptr];
                        r[i] = src.ArgbValues[++ptr];
                        a[i] = src.ArgbValues[++ptr];
                        ptr++;
                    }

                    int bsum;
                    int gsum;
                    int rsum;
                    int asum;
                    int read;

                    int start = 0;
                    int index = 0;
                    if (_stdDeviationX > 0f)
                    {
                        for (int i = 0; i < pixelCount; i++)
                        {
                            bsum = gsum = rsum = asum = 0;
                            read = (int)(i - _stdDeviationX);
                            for (int z = 0; z < _kernel.Length; z++)
                            {
                                if (read < start)
                                {
                                    ptr = start;
                                }
                                else if (read > start + src.Width - 1)
                                {
                                    ptr = start + src.Width - 1;
                                }
                                else
                                {
                                    ptr = read;
                                }
                                bsum += _multable[z, b[ptr]];
                                gsum += _multable[z, g[ptr]];
                                rsum += _multable[z, r[ptr]];
                                asum += _multable[z, a[ptr]];

                                ++read;
                            }
                            b2[i] = (bsum / _kernelSum);
                            g2[i] = (gsum / _kernelSum);
                            r2[i] = (rsum / _kernelSum);
                            a2[i] = (asum / _kernelSum);

                            if (_stdDeviationX > 0f && _stdDeviationY <= 0f)
                            {
                                dest.ArgbValues[index] = (byte)(bsum / _kernelSum);
                                dest.ArgbValues[++index] = (byte)(gsum / _kernelSum);
                                dest.ArgbValues[++index] = (byte)(rsum / _kernelSum);
                                dest.ArgbValues[++index] = (byte)(asum / _kernelSum);
                                index++;
                            }

                            if (i > 0 && i % src.Width == 0)
                            {
                                start += src.Width;
                            }
                        }
                    }

                    if (_stdDeviationX > 0f && _stdDeviationY <= 0f)
                    {
                        return dest.Bitmap;
                    }

                    int tempy;
                    index = 0;
                    for (int i = 0; i < src.Height; i++)
                    {
                        int y = (int)(i - _stdDeviationY);
                        start = y * src.Width;
                        for (int j = 0; j < src.Width; j++)
                        {
                            bsum = gsum = rsum = asum = 0;
                            read = start + j;
                            tempy = y;
                            for (int z = 0; z < _kernel.Length; z++)
                            {
                                if (_stdDeviationX <= 0f && _stdDeviationY > 0f)
                                {
                                    if (tempy < 0)
                                    {
                                        ptr = j;
                                    }
                                    else if (tempy > src.Height - 1)
                                    {
                                        ptr = pixelCount - (src.Width - j);
                                    }
                                    else
                                    {
                                        ptr = read;
                                    }
                                    bsum += _multable[z, b[ptr]];
                                    gsum += _multable[z, g[ptr]];
                                    rsum += _multable[z, r[ptr]];
                                    asum += _multable[z, a[ptr]];
                                }
                                else
                                {
                                    if (tempy < 0)
                                    {
                                        ptr = j;
                                    }
                                    else if (tempy > src.Height - 1)
                                    {
                                        ptr = pixelCount - (src.Width - j);
                                    }
                                    else
                                    {
                                        ptr = read;
                                    }
                                    bsum += _multable[z, b2[ptr]];
                                    gsum += _multable[z, g2[ptr]];
                                    rsum += _multable[z, r2[ptr]];
                                    asum += _multable[z, a2[ptr]];
                                }
                                read += src.Width;
                                ++tempy;
                            }

                            dest.ArgbValues[index] = (byte)(bsum / _kernelSum);
                            dest.ArgbValues[++index] = (byte)(gsum / _kernelSum);
                            dest.ArgbValues[++index] = (byte)(rsum / _kernelSum);
                            dest.ArgbValues[++index] = (byte)(asum / _kernelSum);
                            index++;
                        }
                    }
                    return dest.Bitmap;
                }
            }
        }

        /// <summary>
        /// Gets or sets the radius of the blur (only allows for one value - not the two specified in the SVG Spec)
        /// </summary>
        [SvgAttribute("stdDeviation")]
        public SvgNumberCollection StdDeviation
        {
            get { return GetAttribute("stdDeviation", false, new SvgNumberCollection() { 0f }); }
            set { Attributes["stdDeviation"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            var inputImage = buffer[this.Input];
            var result = Apply(inputImage);
            buffer[this.Result] = result;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGaussianBlur>();
        }
    }
}
