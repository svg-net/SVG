﻿#if !NO_SDC
using System.Drawing.Drawing2D;
#endif
using System.Globalization;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified shear vector to this Matrix.
    /// </summary>
    public sealed class SvgShear : SvgTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

#if !NO_SDC
        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Shear(X, Y);
                return matrix;
            }
        }
#endif

        public override string WriteToString()
        {
            return $"shear({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgShear(float x)
            : this(x, x)
        {
        }

        public SvgShear(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override object Clone()
        {
            return new SvgShear(X, Y);
        }
    }
}
