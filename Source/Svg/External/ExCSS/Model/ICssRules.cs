using System.Collections.Generic;

namespace Svg.ExCSS.Model
{
    interface ISupportsRuleSets
    {
        List<RuleSet> RuleSets { get; }
    }
}