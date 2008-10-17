using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed class SvgScale : SvgTransform
    {
        private float scaleFactorX;
        private float scaleFactorY;

        public float X
        {
            get { return this.scaleFactorX; }
            set { this.scaleFactorX = value; }
        }

        public float Y
        {
            get { return this.scaleFactorY; }
            set { this.scaleFactorY = value; }
        }

        public override System.Drawing.Drawing2D.Matrix Matrix
        {
            get
            {
                System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Scale(this.X, this.Y);
                return matrix;
            }
        }

        public SvgScale(float x) : this(x, x) { }

        public SvgScale(float x, float y)
        {
            this.scaleFactorX = x;
            this.scaleFactorY = y;
        }
    }
}
