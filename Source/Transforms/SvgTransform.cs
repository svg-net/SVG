using System;
#if !NO_SDC
using System.Drawing.Drawing2D;
#endif

namespace Svg.Transforms
{
    public abstract class SvgTransform : ICloneable
    {
#if !NO_SDC
        public abstract Matrix Matrix { get; }
#endif
        public abstract string WriteToString();

        public abstract object Clone();

        #region Equals implementation

#if !NO_SDC
        public override bool Equals(object obj)
        {
            var other = obj as SvgTransform;
            if (other == null)
                return false;

            return Matrix.Equals(other.Matrix);
        }

        public override int GetHashCode()
        {
            return Matrix.GetHashCode();
        }

        public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
        {
            return !(lhs == rhs);
        }
#endif
        #endregion

        public override string ToString()
        {
            return WriteToString();
        }
    }
}
