﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified skew vector to this Matrix.
    /// </summary>
    public sealed class SvgSkew : SvgTransform
    {
        private float angleX, angleY;

        public float AngleX
        {
            get { return this.angleX; }
            set { this.angleX = value; }
        }

        public float AngleY
        {
            get { return this.angleY; }
            set { this.angleY = value; }
        }

        public override Matrix Matrix
        {
            get
            {
                Matrix matrix = new Matrix();
                matrix.Shear(this.AngleX, this.AngleY);
                return matrix;
            }
        }

        public SvgSkew(float x, float y)
        {
            this.angleX = x;
            this.angleY = y;
        }
    }
}