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
        private int _stdDeviation;
        private BlurType _blurType;
        
        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;

        public SvgGaussianBlur()
            : this(1, BlurType.Both)
        {
        }

        public SvgGaussianBlur(int stdDeviation)
            : this(stdDeviation, BlurType.Both)
        {
        }

        public SvgGaussianBlur(int stdDeviation, BlurType blurType) : base()
        {
            _stdDeviation = stdDeviation;
            _blurType = blurType;
            PreCalculate();
        }



        private void PreCalculate()
        {
            int sz = _stdDeviation * 2 + 1;
            _kernel = new int[sz];
            _multable = new int[sz, 256];
            for (int i = 1; i <= _stdDeviation; i++)
            {
                int szi = _stdDeviation - i;
                int szj = _stdDeviation + i;
                _kernel[szj] = _kernel[szi] = (szi + 1) * (szi + 1);
                _kernelSum += (_kernel[szj] + _kernel[szi]);
                for (int j = 0; j < 256; j++)
                {
                    _multable[szj, j] = _multable[szi, j] = _kernel[szj] * j;
                }
            }
            _kernel[_stdDeviation] = (_stdDeviation + 1) * (_stdDeviation + 1);
            _kernelSum += _kernel[_stdDeviation];
            for (int j = 0; j < 256; j++)
            {
                _multable[_stdDeviation, j] = _kernel[_stdDeviation] * j;
            }
        }

        public Bitmap Apply(Image inputImage)
        {
            using (RawBitmap src = new RawBitmap(new Bitmap(inputImage)))
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
                    if (_blurType != BlurType.VerticalOnly)
                    {
                        for (int i = 0; i < pixelCount; i++)
                        {
                            bsum = gsum = rsum = asum = 0;
                            read = i - _stdDeviation;
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

                            if (_blurType == BlurType.HorizontalOnly)
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

                    if (_blurType == BlurType.HorizontalOnly)
                    {
                        return dest.Bitmap;
                    }

                    int tempy;
                    index = 0;
                    for (int i = 0; i < src.Height; i++)
                    {
                        int y = i - _stdDeviation;
                        start = y * src.Width;
                        for (int j = 0; j < src.Width; j++)
                        {
                            bsum = gsum = rsum = asum = 0;
                            read = start + j;
                            tempy = y;
                            for (int z = 0; z < _kernel.Length; z++)
                            {
                                if (_blurType == BlurType.VerticalOnly)
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
        public int StdDeviation
        {
            get { return _stdDeviation; }
            set
            {
                if (value < 1)
                {
                    throw new InvalidOperationException("Radius must be greater then 0");
                }
                _stdDeviation = value;
                PreCalculate();
            }
        }


        public BlurType BlurType
        {
            get { return _blurType; }
            set
            {
                _blurType = value;
            }
        }


        
        public override Bitmap Process()
        {
            //Todo

            return null;
        }



        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGaussianBlur>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGaussianBlur;
            newObj.StdDeviation = this.StdDeviation;
            newObj.BlurType = this.BlurType;
            return newObj;
        }
    }
}