using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Svg.ExCSS
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}