using System;
using System.Collections.Generic;
using System.Text;

namespace Svg.Transforms
{
    public sealed class SvgRotate : SvgTransform
    {
        private float angle;

        public float Angle
        {
            get { return this.angle; }
            set { this.angle = value; }
        }

        public override System.Drawing.Drawing2D.Matrix Matrix
        {
            get
            {
                System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Rotate(this.Angle);
                return matrix;
            }
        }

        public SvgRotate(float angle)
        {
            this.angle = angle;
        }
    }
}