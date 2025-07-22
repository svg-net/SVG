using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Svg.DataTypes;

namespace Svg
{
    public partial struct SvgPoint
    {
        private SvgUnit x;
        private SvgUnit y;

        public SvgUnit X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public SvgUnit Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public bool IsEmpty()
        {
            return (this.X.Value == 0.0f && this.Y.Value == 0.0f);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (!(obj.GetType() == typeof(SvgPoint))) return false;

            var point = (SvgPoint)obj;
            return (point.X.Equals(this.X) && point.Y.Equals(this.Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

#if NET6_0_OR_GREATER
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(SvgUnitConverter))]
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "DynamicDependency keeps converter safe")]
#endif
        public SvgPoint(string x, string y)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(SvgUnit));

            this.x = (SvgUnit)converter.ConvertFrom(x)!;
            this.y = (SvgUnit)converter.ConvertFrom(y)!;
        }

        public SvgPoint(SvgUnit x, SvgUnit y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
