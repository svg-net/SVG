using System;
using System.Collections.Generic;
using System.Text;

namespace Svg.Transforms
{
    public sealed class SvgTranslate : SvgTransform
    {
        private float x;
        private float y;

        public float X
        {
            get { return x; }
            set { this.x = value; }
        }

        public float Y
        {
            get { return y; }
            set { this.y = value; }
        }

        public override System.Drawing.Drawing2D.Matrix Matrix
        {
            get
            {
                System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Translate(this.X, this.Y);
                return matrix;
            }
        }

        public SvgTranslate(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public SvgTranslate(float x)
            : this(x, 0.0f)
        {
        }
    }
}