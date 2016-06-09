using System.Globalization;

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

        public override Matrix Matrix
        {
            get
            {
                Matrix matrix = Factory.Instance.CreateMatrix();
                matrix.Translate(this.X, this.Y);
                return matrix;
            }
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "translate({0}, {1})", this.X, this.Y);
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


		public override object Clone()
		{
			return new SvgTranslate(this.x, this.y);
		}

    }
}