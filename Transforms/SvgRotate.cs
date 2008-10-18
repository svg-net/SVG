using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed class SvgRotate : SvgTransform
    {
        public float Angle
        {
            get;
            set;
        }

        public float CenterX
        {
            get;
            set;
        }

        public float CenterY
        {
            get;
            set;
        }

        public override Matrix Matrix
        {
            get
            {
                Matrix matrix = new Matrix();
                matrix.Translate(this.CenterX, this.CenterY);
                matrix.Rotate(this.Angle);
                matrix.Translate(-this.CenterX, -this.CenterY);
                return matrix;
            }
        }

        public SvgRotate(float angle)
        {
            this.Angle = angle;
        }

        public SvgRotate(float angle, float centerX, float centerY)
            : this(angle)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;
        }
    }
}