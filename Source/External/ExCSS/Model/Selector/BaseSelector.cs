
// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public abstract class BaseSelector
    {
        public sealed override string ToString()
        {
            return ToString(false,0);
        }

        public abstract string ToString(bool friendlyFormat, int indentation);
    }
}

