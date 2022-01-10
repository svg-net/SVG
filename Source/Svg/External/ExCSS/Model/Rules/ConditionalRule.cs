
// ReSharper disable once CheckNamespace
namespace Svg.ExCSS
{
    public abstract class ConditionalRule : AggregateRule
    {
        public virtual string Condition
        {
            get { return string.Empty; }
            set { }
        }
    }
}
