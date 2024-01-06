using System.Collections.Generic;

#nullable enable

namespace Svg;

public class SvgOptions
{
    public SvgOptions()
    {
    }
    
    public SvgOptions(Dictionary<string, string> entities)
    {
        Entities = entities;
    }

    public Dictionary<string, string>? Entities { get; set; }
    public string? Css { get; set; }
}
