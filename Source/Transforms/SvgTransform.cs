using System;

namespace Svg.Transforms
{
    public abstract partial class SvgTransform : ICloneable
    {
        public abstract string WriteToString();

        public abstract object Clone();

        public override string ToString()
        {
            return WriteToString();
        }
    }
}
